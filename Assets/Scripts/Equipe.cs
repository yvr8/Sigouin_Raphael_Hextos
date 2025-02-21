using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E : MonoBehaviour
{
    public GameObject prefabFantassin;
    List<Unite> unites = new List<Unite>();
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Renforcer", 1f, 15f);
    }

    void Renforcer()
    {
        int nbUniteBase = 5;
        int nbTours = 2;
        int nbUnitesMax = 15;

        for (int i = 0;
             i < nbUniteBase + nbTours &&
             unites.Count < nbUnitesMax;
             i++)
        {
            // Instancier une unite
            ;
            // Ajouter l'unite a la liste des unites

        }
    }
}
