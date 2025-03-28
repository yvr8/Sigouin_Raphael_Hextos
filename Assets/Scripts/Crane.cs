using Unity.VisualScripting;
using UnityEngine;

public class Crane : MonoBehaviour
{
    void Start()
    {
        //supprimer de crane apres un certain temps
        Destroy(gameObject, 2.5f);
    }
}