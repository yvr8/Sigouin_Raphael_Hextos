
using UnityEngine;

public class PingPongStar : MonoBehaviour
    {
        [field: SerializeField] private Etats etatActuel;
        [field: SerializeField] protected Unite nemesis;
        protected Unite unite;
        private float rayonAggression;
    enum Etats
    {
        Attente,
        Marche,
        Combat
    }
    
    void Start()
    {
        etatActuel = Etats.Attente;
        unite = GetComponent<Unite>();
        rayonAggression = 5f;
    }

    void Update()
    {
        switch (etatActuel)
        {
            case Etats.Attente:
                Update_EtatAttente();
                break;
            case Etats.Marche:
                Update_EtatMarche();
                break;
            case Etats.Combat:
                Update_EtatCombat();
                break;
        }
    }

    void Update_EtatAttente()
    {
        // choisir quoi faire
        int decision = Random.Range(1, 4);
        switch (decision)
        {
            // choisi une position aleatoire sur la map et si dirige
            case 1:
            {
                GetComponent<SpriteRenderer>().color = Color.green;
                
                Vector2 destination = new Vector2(
                    unite.transform.position.x + Random.Range(-10f, 10f),
                    unite.transform.position.y + Random.Range(-10f, 10f)
                    );

                unite.SetDestination(destination);
                etatActuel = Etats.Marche;
                break;    
            }
            
            // Choisi une position aleatoire autour d'une tour
            case 2:
            {
                GetComponent<SpriteRenderer>().color = Color.blue;
                TourRavitaillement tour = unite.equipe.tours[Random.Range(0, unite.equipe.tours.Length)];
                
                Vector2 destination = new Vector2(
                    tour.transform.position.x + Random.Range(-3f, 3f),
                    tour.transform.position.y + Random.Range(-3f, 3f)
                );
                
                unite.SetDestination(destination);
                etatActuel = Etats.Marche;
                break;
            }
            
            // Choisi une cible proche et essaye de la tuer.
            case 3:
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                
                Collider2D[] colliders = Physics2D.OverlapCircleAll(unite.transform.position, rayonAggression);

                foreach (Collider2D collider in colliders)
                {
                    Unite component = collider.gameObject.GetComponent<Unite>(); 
                    if (component != null && component.equipe != unite.equipe)
                    {
                        // Transition vers combat
                        nemesis = collider.gameObject.GetComponent<Unite>();
                        etatActuel = Etats.Combat;
                        break;
                    }
                }
                break;
            }
        }
    }

    void Update_EtatMarche()
    {   
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, unite.rayonAttaque + unite.distanceAttaque);
        // Si un ennemi à proximité, je l'attaque
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.GetComponent("Unite") != null &&
                collider.gameObject.GetComponent<Unite>().equipe != unite.equipe)    
            {
                unite.Attaquer(collider.gameObject.GetComponent<Unite>().transform.position);
            }
        }
        // Arrivé à destination, on retourne en attente
        if (UniteAtteintDestination())
        {
            etatActuel = Etats.Attente;
            return;
        }
    }

    void Update_EtatCombat()
    {
        if (nemesis)
        {
            // Se déplacer vers elle
            unite.SetDestination(nemesis.transform.position);
            // Tenter de l'attaquer
            unite.Attaquer(nemesis.transform.position);
        }
        else
        {
            etatActuel = Etats.Attente;
        }
    }
    
    // Indique si l'unité à atteint sa destination
    bool UniteAtteintDestination()
    {
        // Vérifier qu'il n'y a pas un chemin en cours de calcul
        if (!unite.agent.pathPending)
        {
            return unite.agent.remainingDistance <= unite.agent.stoppingDistance;
        }
        return false;
    }
}