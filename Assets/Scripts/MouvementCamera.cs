using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementCamera : MonoBehaviour
{
    private Vector2 directionDeplacement;
    public float vitesse
    void Update()
    {
        Vector2 directionDeplacement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        float vitesse = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        transform.position += directionDeplacement * vitesse * Time.deltaTime;
    }
}
