using Unity.VisualScripting;
using UnityEngine;

public class Crane : MonoBehaviour
{
    private GestionnaireAudio audioPlayer;
    void Start()
    {
        audioPlayer = FindObjectOfType<GestionnaireAudio>();
        audioPlayer.PlaySonMort();
        //supprimer de crane apres un certain temps
        Destroy(gameObject, 2.5f);
    }
}