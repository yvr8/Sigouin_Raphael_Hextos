using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Ce que vous pouvez faire
/// -------------------------------
/// 1. Modifier le contenu des fonctions
/// 2. Créer des surcharges de fonctions
/// 
/// Ce que vous ne pouvez PAS faire
/// --------------------------------
/// 1. Modifier les noms de fonctions
/// 2. Modifier les paramètres des fonctions
/// 3. Modifier les noms des variables
/// </summary>
public class Unite : MonoBehaviour
{
    // Attributs
    [field: SerializeField]
    public float pointsVie { get; protected set; }
    public float pointsVieMax { get; protected set; }
    public float vitesseDeplacement { get; protected set; }
    public Vector2 force { get; protected set; }
    public float delaiAttaque { get; protected set; }
    public float distanceAttaque { get; protected set; }
    public float rayonAttaque { get; protected set; }

    public NavMeshAgent agent { get; private set; }
    public Animator animator { get; protected set; }
    public SpriteRenderer spriteRenderer {get; protected set; }

    // Timestamp de la derniereAttaque
    public float tsDerniereAttaque { get; protected set; }

    // Timestamp de la création de l'unité
    public float tsCreation { get; private set; }
    
    // Equipe de l'unité
    public Equipe equipe { get; private set; }
    public GameObject prefabCrane;

    void Start()
    {
        SetAttributs();
    }

    void Update()
    {
        AnimeMouvement();
    }
    protected virtual void SetAttributs()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        tsCreation = Time.time;
        pointsVieMax = 100.0f;
        pointsVie = pointsVieMax;
        force = new Vector2(1.0f, 3.0f);
        delaiAttaque = 1.0f;
        distanceAttaque = 2.0f;
        rayonAttaque = 1.0f;
    }
    
    // Assigner l'équipe de l'unité
    public void SetEquipe(Equipe equipe)
    {
        this.equipe = equipe;
    }

    // Demander à l'agent de se rendre à une destination
    public void SetDestination(Vector2 destination)
    {
        agent.SetDestination(destination);
    }
    
    // Indique si l'unité peut attaquer (selon le delaiAttaque)
    public bool AttaqueEstPrete()
    {
        return Time.time >= tsDerniereAttaque + delaiAttaque;
    }

    public void Attaquer(Vector2 position)
    {
        // Vérifier si le délaiAttaque le permet
        if (!AttaqueEstPrete())
        {
            return;
        }
        
        // Vérifier si la distanceAttaque le permet
        if (Vector2.Distance(transform.position, position) > distanceAttaque)
        {
            return;
        }
        
        // Effectuer l'attaque (avec des dégats aléatoires) et executer l'animation
        InfligerDegats(position);
        AnimeAttaque();
        
        // Remettre le timestamp à "maintenant"
        tsDerniereAttaque = Time.time;
    }
    protected virtual void InfligerDegats(Vector2 position)
    {
        // Récupérer les colliders à proximité de position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, rayonAttaque);
        
        // Vérifier chaque collider un par un
        foreach (Collider2D collider in colliders)
        {
            if (// Vérifier s'il s'agit d'une unité
                collider.gameObject.GetComponent<Unite>() is not null
                // Vérifier si elle est dans l'équipe adverse
                && collider.gameObject.GetComponent<Unite>().equipe != equipe)
            { 
                // Infliger des dégats
                collider.gameObject.GetComponent<Unite>().SubirDegats(Random.Range(force.x, force.y));
            }
        }
    }

    public void SubirDegats(float degats)
    {
        // Vérifier l'immunité de 2 secondes
        if (Time.time < tsCreation + 2f)
            return;
        
        // Subir des dégats
        pointsVie -= degats;
        
        // Vérifier si l'unité est morte
        if (pointsVie <= 0f)
        {
            //Faire apparaitre le crane
            Instantiate(prefabCrane, transform.position, Quaternion.identity);
            
            // Aviser l'équipe
            equipe.UniteMorte(this);
            
            // Disparition
            Destroy(gameObject);
        }
    }

    void AnimeMouvement()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("enDeplacement", true);
        }
        else
        {
            animator.SetBool("enDeplacement", false);
        }   
        if (agent.velocity.x > 0.5f)
        {
            spriteRenderer.flipX = false;
        }
        else if (agent.velocity.x < -0.5f)
        {
            spriteRenderer.flipX= true;
        }   
    }

    void AnimeAttaque()
    {
        animator.SetTrigger("attaque");
    }
}