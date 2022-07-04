using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSorce;

    public AudioClip coinSound;

    public AudioClip jumpSound;

    public AudioClip hitSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSorce=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlaySFX(AudioClip sfx)
    {
        audioSorce.PlayOneShot(sfx);
    }
}
