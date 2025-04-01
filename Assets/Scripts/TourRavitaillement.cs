using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TourRavitaillement : MonoBehaviour
{
    [field: SerializeField] public Equipe proprietaire { get; private set; }

    private void Start()
    {
        InvokeRepeating("VerifierProprietaire", 0f, .5f);
    }

    void VerifierProprietaire()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3.0f);

        Dictionary<Equipe, int> compteurEquipe = new Dictionary<Equipe, int>();

        foreach (Collider2D collider in colliders)
        {
            Unite unite = collider.GetComponent<Unite>();
            if (unite != null)
            {
                Equipe equipe = unite.equipe;

                if (!compteurEquipe.ContainsKey(equipe))
                {
                    compteurEquipe[equipe] = 1;
                }
                else
                {
                    compteurEquipe[equipe]++;
                }
            }
        }

        // Now find the Equipe with the highest count
        int nombreDeTroupe = 0;
        Equipe equipeAvecPlusTroupe = null;

        foreach (KeyValuePair<Equipe, int> equipe in compteurEquipe)
        {
            if (equipe.Value > nombreDeTroupe)
            {
                nombreDeTroupe = equipe.Value;
                equipeAvecPlusTroupe = equipe.Key;
            }
        }

        // definir le proprietaire
        if (equipeAvecPlusTroupe != null)
        {
            proprietaire = equipeAvecPlusTroupe;
        }
    }
}
