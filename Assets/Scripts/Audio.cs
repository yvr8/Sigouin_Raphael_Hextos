using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioClip[] listeMusique;
    private AudioSource sourceAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        sourceAudio = GetComponent<AudioSource>();
        lancerMusique();
    }

    // Update is called once per frame
    void Update()
    {
        if (!sourceAudio.isPlaying)
        {
            lancerMusique();
        }
    }

    void lancerMusique()
    {
        sourceAudio.clip = listeMusique[Random.Range(0, listeMusique.Length)];
        sourceAudio.Play();
    }
}
