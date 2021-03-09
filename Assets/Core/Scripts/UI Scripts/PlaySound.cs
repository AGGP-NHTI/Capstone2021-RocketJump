using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource source;
    public List<AudioClip> clips = new List<AudioClip>();
    
   
    public void PlayClip(int i)
    {
        source.PlayOneShot(clips[i]);
    }
}
