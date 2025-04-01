using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    private Animator animator;
    private Vector2 positionInitial;
    private Vector2 positionFinal;
    private Vector2 directionNormale;
    private Vector2 deplacement;
    private float gravite;
    private float velocite;
    private float tempsEcoule;
    private float tempsFinal;
    private float rayonAttaque;
    private float force;
    private bool executer;
    
    // Update is called once per frame
    void Update()
    {
        Deplacement();
    }

    public void Init(Vector2 positionFinal, float rayonAttaque = 3.0f, float force = 5.0f, float velocite = 4.0f, float gravite = -4.8f)
    {
        animator = GetComponent<Animator>();
        this.positionInitial = transform.position;
        this.positionFinal = positionFinal;
        this.velocite = velocite;
        this.gravite = gravite;
        this.force = force;
        this.directionNormale = (this.positionFinal - this.positionInitial).normalized;
        this.tempsFinal = (this.positionFinal - this.positionInitial).magnitude / this.velocite;
        this.tempsEcoule = 0;
        this.executer = true;
    }
    
    private void Deplacement()
    {
        if (executer)
        {
            tempsEcoule += Time.deltaTime;
            deplacement.x = positionInitial.x + velocite * tempsEcoule * directionNormale.x;
            deplacement.y = positionInitial.y + velocite * tempsEcoule * directionNormale.y + gravite * tempsEcoule * tempsEcoule - gravite * tempsFinal * tempsEcoule;
            transform.position = deplacement;
            if (tempsEcoule >= tempsFinal)
            {
                Explosion();
                executer = false;
            }
        }
    }
    private void Explosion()
    {
        animator.SetTrigger("explosion");
        Destroy(gameObject, 1.0f);
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(positionFinal, rayonAttaque);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.GetComponent<Unite>() is not null)
            {
                // Infliger des dégats
                collider.gameObject.GetComponent<Unite>().SubirDegats(force);
            }
        }
    }
}
