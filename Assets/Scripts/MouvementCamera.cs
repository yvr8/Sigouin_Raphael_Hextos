using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class MouvementCamera : MonoBehaviour
{
    private Vector2 directionDeplacement;
    public float vitesse;
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 directionDeplacement = new Vector3(horizontal, vertical, 0f);

        transform.position += Time.deltaTime * directionDeplacement * vitesse;
    }
}
