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
    // variable pour definir les stats des unites.
    public float pointsVie { get; protected set; }
    public float pointsVieMax { get; protected set; }
    public float vitesseDeplacement { get; protected set; }
    public Vector2 force { get; protected set; }
    public float delaiAttaque { get; protected set; }
    public float distanceAttaque { get;  protected set; }
    public float rayonAttaque { get; protected set; }
    // variable de temps
    public float tsDerniereAttaque { get; protected set; }
    public float tsCreation { get; protected set; }
    // Objet qui apparait a la mort de l'unite  
    public GameObject prefabCrane;
    // Equipe de l'unité
    public Equipe equipe { get; protected set; }
    // utilise par Sapeur.cs pour dire au script lalistar que l'unite est un sapeur
    public bool estSapeur {get; protected set;}
    // Objects qui sont necessaire pour les differentes fonctionnalites d'une unite
    public NavMeshAgent agent { get;  protected set; }
    protected Animator animator { get;  set; }
    protected SpriteRenderer spriteRenderer {get;  set; }
    protected GestionnaireAudio audioPlayer;

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
        // Objects qui sont necessaire pour les differentes fonctionnalites d'une unite
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioPlayer = FindObjectOfType<GestionnaireAudio>();
        
        
        tsCreation = Time.time;
        // creation des differentes stats de l'unite
        pointsVieMax = Random.Range(50f, 150f);
        force = new Vector2(Random.Range(1f, 2f), Random.Range(2f, 5f));
        delaiAttaque = Random.Range(1f, 1.5f);
        distanceAttaque = Random.Range(1.5f, 2.5f);
        rayonAttaque = Random.Range(0.5f, 1.5f);
        vitesseDeplacement = Random.Range(3f, 4f);
        // autres
        pointsVie = pointsVieMax;
        estSapeur = false;
        agent.speed = vitesseDeplacement;
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
        audioPlayer.PlaySonAttaque();
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

        audioPlayer.PlaySonDommage();
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