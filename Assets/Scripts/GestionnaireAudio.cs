using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Permet d'eviter un porbleme cause par l'execution de son dans les unites quand elles sont supprime.
/// </summary>
public class GestionnaireAudio : MonoBehaviour
{
    public AudioSource source;
    public AudioClip sonExplosion;
    public AudioClip sonAttaque;
    public AudioClip sonDommage;
    public AudioClip sonMort;
    
    void Start()
    {
        source = GetComponent<AudioSource>();   
    }
    
    public void PlaySonExplosion()
    {
        source.PlayOneShot(sonExplosion);
    }
    
    public void PlaySonMort()
    {
        source.PlayOneShot(sonMort);
    }
    
    public void PlaySonDommage()
    {
        source.PlayOneShot(sonDommage);
    }
    
    public void PlaySonAttaque()
    {
        source.PlayOneShot(sonAttaque);
    }
}
