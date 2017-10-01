using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaAudio : MonoBehaviour
{
    public AudioClip m_lavaSound;
    private AudioSource m_audioSource;

    public float m_waitTime;
    public float m_repeatTime;

    // Use this for initialization
    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        InvokeRepeating("PlayLavaSound", m_waitTime, m_repeatTime);
    }

    private void PlayLavaSound()
    {
        m_audioSource.PlayOneShot(m_lavaSound, 1.0f);
    }
}