using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioClip explosionClip;
    public AudioClip playerhitClip;
    public AudioSource audioSource;
    private float volumeDefault;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        volumeDefault = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //audioSource.Play();
        }
    }

    public void shipExplode()
    {
        audioSource.volume = 2;
        audioSource.PlayOneShot(explosionClip);
        audioSource.volume = volumeDefault;
    }

    public void playerHit()
    {
        audioSource.PlayOneShot(playerhitClip);
    }

}
