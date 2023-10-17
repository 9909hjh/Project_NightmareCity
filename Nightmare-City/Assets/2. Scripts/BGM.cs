using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioSource aud;

    public GameObject monsterPrefab;
    public AudioClip Music;

    private void Start()
    {
        aud = GetComponent<AudioSource>();

        aud.clip = Music;
        aud.Play();
    }

    
}
