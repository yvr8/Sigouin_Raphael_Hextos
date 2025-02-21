using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentNavigation : MonoBehaviour
{
    NavMeshAgent agent;
    
    void Start()
    {   
        //assigne l'agent (qui se trouve sur le meme objet)
        agent = GetComponent<NavMeshAgent>();
        
        // Appele la fonction deplacer a toutes les 5 secondes
        InvokeRepeating("Deplacer", 0f, 0.5f);
    }

    void Deplacer()
    {
        //trouver une destination aleatoire.
        Vector2 destination = new Vector2(Random.Range(-8.5f, 14.5f), Random.Range(-4.5f, 8.5f));
        //diriger l'agent vers la destination.
        agent.SetDestination(destination);
    }
}
