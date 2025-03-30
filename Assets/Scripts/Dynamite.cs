using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    // valeur a definir au moment de l'instantiation
    public Vector3 positionInitial;
    public Vector3 positionFinal;
    [field: SerializeField]private float rayonExplosion;
    public float gravite;
    public float velocite;
    public float force; 
    // variable utilise pour les calcules
    [field: SerializeField]private Vector3 directionNormal;
    [field: SerializeField]private Vector3 deplacement;
    [field: SerializeField]private float tempsEcoule;
    [field: SerializeField]private float tempsFinal;
    public bool executer;
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Deplacement();
    }

    void Init()
    {
        velocite = 0.1f;
        gravite = -0.5f;
        deplacement = Vector3.zero;
        directionNormal = Vector3.zero;
    }
    
    public void Attaquer(Vector2 position, float rayon, float force)
    {
        // valeur auto
        positionInitial = transform.position;
        // valeur du point de la zone d'attaque
        positionFinal = position;
        // direction normalize pour utilisation dans les calcules.
        directionNormal = Vector3.Normalize(positionFinal - positionInitial);
        tempsFinal = (positionFinal - positionInitial).magnitude;
        tempsEcoule = 0;
        // lancer les calcule de la position de la dynamite
        executer = true;
    }
    private void Deplacement()
    {
        if (executer)
        {
            tempsEcoule += Time.deltaTime;
            deplacement.x = positionInitial.x + velocite * directionNormal.x;
            deplacement.y = positionInitial.y + velocite * directionNormal.y;
            transform.position = deplacement;
            if (tempsEcoule <= tempsFinal)
            {
                Explosion();
                executer = false;
            }
        }
    }
    /*+ gravite * tempsEcoule * tempsEcoule - gravite * tempsEcoule*/
    private void Explosion()
    {
        Destroy(gameObject, 1.0f);
    }
}
