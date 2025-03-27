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
    [field: SerializeField]
    public float pointsVieMax { get; set; } = 100;
    [field: SerializeField]
    public float vitesseDeplacement { get; protected set; } = 5;
    public Vector2 force { get; protected set; } = new Vector2(1, 2);
    [field: SerializeField]
    public float delaiAttaque { get; protected set; } = 1;
    public float distanceAttaque { get; protected set; } = 3;
    public float rayonAttaque { get; protected set; } = 3;

    public NavMeshAgent agent { get; private set; }

    // Timestamp de la derniereAttaque
    public float tsDerniereAttaque { get; protected set; } = 0;

    // Timestamp de la création de l'unité
    public float tsCreation { get; private set; }
    
    // Equipe de l'unité
    public Equipe equipe { get; private set; }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tsCreation = Time.time;
        pointsVie = pointsVieMax;
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
        
        // Effectuer l'attaque (avec des dégats aléatoires)
        InfligerDegats(position, Random.Range(force.x, force.y));
        
        // Remettre le timestamp à "maintenant"
        tsDerniereAttaque = Time.time;
    }
    protected virtual void InfligerDegats(Vector2 position, float degats)
    {
        // Récupérer les colliders à proximité de position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, rayonAttaque);
        
        // Vérifier chaque collider un par un
        foreach (Collider2D collider in colliders)
        {
            if (// Vérifier s'il s'agit d'une unité
                collider.gameObject.GetComponent<Unite>() is not null
                // Vérifier si elle est dans l'équipe adverse
                && collider.gameObject.GetComponent<Unite>().equipe == equipe)
            { 
                // Infliger des dégats
                collider.gameObject.GetComponent<Unite>().SubirDegats(degats);
            }
        }
    }

    void SubirDegats(float degats)
    {
        // Vérifier l'immunité de 2 secondes
        if (Time.time < tsCreation + 2f)
            return;
        
        // Subir des dégats
        pointsVie -= degats;
        
        // Vérifier si l'unité est morte
        if (pointsVie <= 0f)
        {
            // Aviser l'équipe
            equipe.UniteMorte(this);
            
            // Disparition
            Destroy(gameObject);
        }
    }
}