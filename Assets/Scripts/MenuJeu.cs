using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuJeu : MonoBehaviour
{
    public Equipe equipeBleu;
    public Equipe equipeRouge;

    public TMP_Text timer;
    public TMP_Text scoreRouge;
    public TMP_Text scoreBleu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // timer
        TimeSpan time = TimeSpan.FromSeconds(Time.realtimeSinceStartup);
        timer.text = time.ToString(@"mm\:ss");
        
        // equipe rouge
        scoreBleu.text = ConstruireString(equipeBleu);
        scoreRouge.text = ConstruireString(equipeRouge);
    }

    string ConstruireString(Equipe equipe)
    {
        int nbTour = 0;
        
        foreach (TourRavitaillement tour in equipe.tours)
        {
            if (tour.proprietaire == equipe)
            {
                nbTour++;
            }
        }
        return $"Vie :  {equipe.nbVieRestantes}\nUnité : {equipe.unites.Count}\nTour : {nbTour}";
    }
}
    
