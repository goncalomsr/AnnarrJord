using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narration : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource narration;
    public AudioSource musicHandler;
    public int currentClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "narratorPoints")
        {
            narration.clip = clips[currentClip];
            narration.Play();
            currentClip++;
        }
        if (other.tag == "musicStart")
        {
            musicHandler.Play();
        }
        if (other.tag == "musicStop")
        {
            musicHandler.Stop();
        }
    }
}
