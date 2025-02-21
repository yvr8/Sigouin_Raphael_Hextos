using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Ce que vous pouvez faire
/// ------------------------
/// 1. Modifier le contenu des fonctions
/// 2. Creer des surcharges de fonctions
/// 
/// Ce que vous ne pouvez pas faire
/// -------------------------------
/// 1. Modifier les noms de fontions
/// 2. Modifier les paramettres des fonctions
/// 3. Modifier les noms des variables
/// </summary>
public class Unite : MonoBehaviour
{
    // Start is called before the first frame update
    
    // Attributs 
    public float pointsVie { get; private set; }
    public float pointVieMax { get; private set; }
    public float vitesseDeplacement { get; private set; }
    public Vector2 force { get; private set; }
    public float delaiAttaque { get; private set; }
    public float distanceAttaque { get; private set; }
    public float rayonAttaque { get; private set; }
    
    // TS = Time Stamp (de la derniere attaque)
    private float tsDerniereAttaque;
    private float tsCreation;
    private NavMeshAgent agent;
    
    
    public 
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tsCreation = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Donne une destination a l'agent
    public void SetDestination(Vector2 destination)
    {
        agent.SetDestination(destination);
    }

    public void Attaquer(Vector2 position)
    {
        // Verifier si le delai d'attaque le permet
        if (!AttaqueEstPrete())
            return;
        // Verifier si la distance d'attaque le permet
        if (Vector2.Distance(transform.position, position) > distanceAttaque)
            return;
        // Effectuer l'attaque
        InfligerDegats(position, Random.Range(force.x, force.y));
        tsDerniereAttaque = Time.time ;
    }

    public bool AttaqueEstPrete()
    {
        // Time.time: 42, ts: 40, delai: 2
        return Time.time > tsDerniereAttaque + delaiAttaque;
    }

    protected virtual void InfligerDegats(Vector2 position, float degats)
    {
        // Recuperer les colliders a proximite de position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, rayonAttaque);
        // Verifier chaque colliders un par un
        
        // Verifier s'il s'agit d'une unite
        
        // Verifier s'il est dans l'equipe adverse
    }

    void SubirDegats(float degats)
    {
        // Verifier l'immunite de 2 secondes
        if (Time.time < tsCreation + 2f)
            return;
        
        pointsVie -= degats;

        if (pointsVie <= 0f)
        {
            //TODO : mouwiw, Kapute, Finito
        }
    }
}
