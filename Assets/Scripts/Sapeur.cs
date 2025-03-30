using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.AI;
public class Sapeur : Unite
{
    public GameObject prefabDynamite;
    protected override void SetAttributs()
    {
        // Objects qui sont necessaire pour les differentes fonctionnalites d'une unite
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        tsCreation = Time.time;
        // creation des differentes stats de l'unite
        pointsVieMax = Random.Range(50f, 150f);
        force = new Vector2(Random.Range(1f, 2f), Random.Range(2f, 5f));
        delaiAttaque = Random.Range(2.5f, 3.5f);
        distanceAttaque = Random.Range(3f, 5f);
        rayonAttaque = Random.Range(1.5f, 2.5f);
        vitesseDeplacement = Random.Range(1f, 2f);
        // autres
        pointsVie = pointsVieMax;
        estSapeur = true;
        agent.speed = vitesseDeplacement;
    }

    protected override void InfligerDegats(Vector2 position)
    {
        GameObject dynamite = Instantiate(prefabDynamite, transform.position, Quaternion.identity);
        Dynamite dynamiteScript = dynamite.GetComponent<Dynamite>();
        dynamiteScript.Attaquer(position, rayonAttaque, Random.Range(force.x, force.y));
    }
}
