using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSetup : MonoBehaviour
{
    AudioListener listener;
    public GameObject AudioPrefab;
    public AudioClip clip;

    void Start()
    {
        
        listener = SearchAudio();
        
        if (listener == null)
        {
            GameObject go = Instantiate(AudioPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            listener = go.GetComponent<AudioListener>();
        }
        foreach (CharacterIcons i in FindObjectsOfType<CharacterIcons>())
        {
            i.source = listener.GetComponent<AudioSource>();
            i.clip = clip;
        }
    }

    public AudioListener SearchAudio()
    {
        AudioListener l = null;
        foreach (AudioListener p in FindObjectsOfType<AudioListener>())
        {
            l = p;
        }
        return l;
    }
    
}
