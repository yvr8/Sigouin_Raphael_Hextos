using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RochePierreGravierTerrePlasmaFeuElectriqueFoudreEauBulleArbreLobotomieLaboratoireImprimanteClavierUsbMacChargeurUltraStar : MonoBehaviour
{
    //Liste des etats du state machine
    enum Etats
    {
        attente,
        marche,
        combat
    }
    public Unite unite;
    [SerializeField] Etats etatActuel;
    private Unite nemesis;
    private Vector2 destinationInitiale;
    void Start()
    {
        etatActuel = Etats.attente;
        unite = GetComponent<Unite>();
    }
    void Update()
    {
        switch (etatActuel)
        {
            case Etats.attente:
                Update_EtatAttente();
                break;
            case Etats.marche:
                Update_EtatMarche();
                break;
            case Etats.combat:
                Update_EtatCombat();
                break;
        }
    }

    void Update_EtatAttente()
    {
        //Trouver une tour enemie
        foreach (var tour in unite.equipe.tourRavitaillements)
        {
            if (tour.proprietaire != unite.equipe)
            {
                unite.SetDestination(tour.transform.position);
                etatActuel = Etats.marche;
                return;
            }
        }
        //etat cul de sac : se diriger vers une tour aleatoire
        int indexTourAleatoire = Random.Range(0, unite.equipe.tourRavitaillements.Length);
        unite.SetDestination(unite.equipe.tourRavitaillements[indexTourAleatoire].transform.position);
        etatActuel = Etats.marche;
        return;
    }

    void Update_EtatMarche()
    {
        //TODO : si un enemie proche > attaque
        //  Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, rayonAttaque);

        //sauvegarder la destination initial
        destinationInitiale = unite.agent.destination;
        //arrive a destinatiion > attente
        if (UniteAtteintDestination())
        {
            etatActuel = Etats.attente;
            return;
        }
    }

    void Update_EtatCombat()
    {
        //TODO : this
        if (nemesis != null)
        {
            unite.SetDestination(nemesis.transform.position);
            //assigner une cible
            nemesis = new Unite(); // assigner l'enemie 
            //essayer de tapper
            unite.Attaquer(nemesis.transform.position);
            //si la cible meurt je contonue vers ma destionation
        }
        //si l'enemie meurt je continue vers ma destination > marche
        unite.SetDestination(destinationInitiale);
        etatActuel = Etats.marche;
    }

    bool UniteAtteintDestination()
    {
        //verifier si il y a un chemin en cours de calcule
        if (!unite.agent.pathPending)
        {
            return unite.agent.remainingDistance <= unite.agent.stoppingDistance;
        }
        return false;
    }
}
