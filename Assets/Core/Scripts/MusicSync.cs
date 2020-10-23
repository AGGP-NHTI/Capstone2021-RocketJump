using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSync : MonoBehaviour
{
    //Beats per Minute.
    public float BPM = 100f;
    //how much off the beat the player can be before they are considered "Off Beat".
    public float range = 0.1f;

    public AudioSource source;
    bool paused = false;
    double nextTime = 0;

    //whether the player is on beat
    public bool OnBeat = false;
    //how much time is given to the system to call the functions
    public float calltime = 1;

    public List<MusicalObject> m_objects = new List<MusicalObject>();

    void Start()
    {
        foreach (MusicalObject m in FindObjectsOfType<MusicalObject>())
        {
            m_objects.Capacity++;
            m_objects.Add(m);
        }
        nextTime = AudioSettings.dspTime;        
    }

    void Update()
    {
        if (!paused)
        {
            if (AudioSettings.dspTime + calltime > nextTime)
            {
                StartCoroutine(Earlybeat());
                StartCoroutine(beat());
                StartCoroutine(Latebeat());
                nextTime += 60 / BPM * 2;
            }
        }
    }

    IEnumerator Earlybeat()
    {
        yield return new WaitForSeconds(calltime - range);
        OnBeat = true;
    }

    IEnumerator beat()
    {
        yield return new WaitForSeconds(calltime);
        Debug.Log("Bam");
        foreach (MusicalObject m in m_objects)
        {
            m.OnBeat();
        }
    }

    IEnumerator Latebeat()
    {
        yield return new WaitForSeconds(calltime + range);
        OnBeat = false;
    }

    public bool IsSynced()
    {
        return true;
    }

    public void Resync()
    {

    }

    public void PauseMusic()
    {
        paused = true;
        source.Pause();
    }

    public void ResumeMusic()
    {
        paused = false;
        source.UnPause();
    }
}
