using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TourRavitaillement : MonoBehaviour
{
    public Equipe proprietaire { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void VerifierProprietaire()
    {
        // Compte le nombre de troupe de chaque equipe autour de la tour 
        Dictionary<Equipe, int> comptageEquipe= new Dictionary<Equipe, int>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D collider in colliders)
        {
            Unite component = collider.gameObject.GetComponent<Unite>();
            if (!(component.equipe))
            {
                comptageEquipe.Add(component.equipe, 1);    
            }
            else
            {
                comptageEquipe[component.equipe]++;   
            }
        }
        // Trouve l'equipe avec le plus de troupe autour de la tour
        Equipe equipeAvecLePlusDeTroupe = null;
        int intMax = 0;

        foreach (var valeur in comptageEquipe)
        {
            if (valeur.Value > intMax)
            {
                intMax = valeur.Value;
                equipeAvecLePlusDeTroupe = valeur.Key;
            }
        }

        if (comptageEquipe[equipeAvecLePlusDeTroupe] > 0)
        {
            proprietaire = equipeAvecLePlusDeTroupe;    
        }
        else
        {
            proprietaire = null;
        }
    }
}
