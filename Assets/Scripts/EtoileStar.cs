using UnityEngine;

public class EtoileStar : MonoBehaviour
{
    // Liste des états du state machine
    enum Etats
    {
        Attente,
        Marche,
        Combat
    }

    [SerializeField] Etats etatActuel;
    protected Unite unite;
    // Cible ennemi que l'unité pourchasse
    [SerializeField] Unite nemesis;
    private Vector2 destinationInitiale;
    
    void Start()
    {
        etatActuel = Etats.Attente;
        unite = GetComponent<Unite>();
        Collider2D collider = GetComponent<Collider2D>();
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
        // Trouver une tour n'appartenant pas à mon équipe > S'y diriger
        foreach (var tour in unite.equipe.tours)
        {
            // Comparer le propriétaire de la tour à mon équipe
            if (tour.proprietaire != unite.equipe)
            {
                // S'y diriger
                unite.SetDestination(tour.transform.position);
                etatActuel = Etats.Marche;
                return;
            }
        }

        // Se diriger vers une tour aléatoire
        int indexTourAleatoire = Random.Range(0, unite.equipe.tours.Length);

        // S'y diriger
        unite.SetDestination(unite.equipe.tours[indexTourAleatoire].transform.position);
        etatActuel = Etats.Marche;
        return;
    }

    void Update_EtatMarche()
    {
        // TODO: Si un ennemi à proximité, je l'attaque
        
        // Assigner une cible
        Collider2D[] listeObject = Physics2D.OverlapCircleAll(transform.position, 8f);
        foreach (Collider2D collider in listeObject)
        {
            if (nemesis is null && unite.equipe != collider.gameObject.GetComponent<Unite>().equipe)
            {
                nemesis = collider.gameObject.GetComponent<Unite>();
            }
            // Sauvegarder la destination où l'unité allait
            destinationInitiale = unite.agent.destination;
            // Transition vers combat
            etatActuel = Etats.Combat;
            return;
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
        // Tant que ma cible est en vie
        if (nemesis is not null)
        {
            // Se déplacer vers elle
            unite.SetDestination(nemesis.transform.position);

            // Tenter de l'attaquer
            unite.Attaquer(nemesis.transform.position);

            return;
        }

        // Si la cible meurt, je continue vers ma destination initiale
        unite.SetDestination(destinationInitiale);
        etatActuel = Etats.Marche;

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