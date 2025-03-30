using System.Collections.Generic;
using UnityEngine;

public class Equipe : MonoBehaviour
{
    public GameObject prefabFantassin;
    public GameObject prefabSappeur;
    public List<Unite> unites = new List<Unite>();
    public TourRavitaillement[] tours { get; private set; }
    public int nbVieRestantes = 100;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Renforcer", 1.0f, 15.0f);
        //assigner la liste des tours 
        tours = FindObjectsOfType<TourRavitaillement>();
    }

    public void Renforcer() 
    {
        int nbUniteBase = 5;
        int nbTours = 0;
        int nbUnitesMax = 15;
    
        //Compter le nombre de tours qui m'appartiennent
        foreach (var tour in tours)
        {
            if (tour.proprietaire == this)
            {
                nbTours++;
            }
        }
        
        for (int i = 0;
             i < nbUniteBase + nbTours &&
             unites.Count < nbUnitesMax;
             i++)
        {
            GameObject prefab;
            if (Random.Range(1, 3) == 1)
            {
                prefab = prefabFantassin;
            }
            else
            {
                prefab = prefabSappeur;
            }

            // Instancier une unite
            Unite newUnite = Instantiate(
                prefab, 
                transform.position,
                Quaternion.identity).
                GetComponent<Unite>();
            
            // Ajouter l'unite a la liste des unites
            unites.Add(newUnite);
            
                // Assigner l'unite a son equipe
            newUnite.SetEquipe(this);
        }
    }
    public void UniteMorte(Unite defunt)
    {
        unites.Remove(defunt);
            
        nbVieRestantes--;

        if (nbVieRestantes <= 0)
        {
            Debug.Log("Je n'ai plus de vie");
        }
    }
}
