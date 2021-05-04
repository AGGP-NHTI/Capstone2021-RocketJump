using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Audio_Manager : NetworkedBehaviour
{
    public static Audio_Manager instance;

    [Header("Prefabs")]
    public GameObject audioObjectPrefab;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }


    public void PlayAudio(AudioClip clip, Vector3 loc)
    {
        //Debug.Log();

        if (clip && loc != null)
        {
            Debug.Log("CLIP: " + clip.name + ", LOC: " + loc);
            InvokeServerRpc(RequestServerSpawnAudio, clip.name , loc);
        }
    }

    [ServerRPC(RequireOwnership = false)]
    void RequestServerSpawnAudio(string clipName, Vector3 loc)
    {
        Debug.Log("SERVER SPAWN AUDIO");
        InvokeClientRpcOnEveryone(SpawnClientAudioObj, clipName, loc);
    }

    [ClientRPC]
    void SpawnClientAudioObj(string clipName, Vector3 loc)
    {
        Debug.Log("CLIENT SPAWN AUDIO");
        GameObject audioObj = Instantiate(audioObjectPrefab, loc, Quaternion.identity);

        StartCoroutine(playAudio(audioObj, clipName));
    }

    IEnumerator playAudio(GameObject audioObj, string clipName)
    {
        yield return new WaitForEndOfFrame();


        if (audioObj.TryGetComponent(out AudioSource source))
        {
            AudioClip clip = ApplicationGlobals.GetAudioClipByName(clipName);
            if (clip)
            {
                source.clip = clip;
                source.Play();

                Destroy(audioObj, clip.length);
            }
        }
    }

    public void PlayAudio(string clipName, Vector3 loc)
    {
        GameObject audioObj = Instantiate(audioObjectPrefab, loc, Quaternion.identity);

        if (audioObj.TryGetComponent(out AudioSource source))
        {
            AudioClip clip = ApplicationGlobals.GetAudioClipByName(clipName);
            if (clip)
            {
                source.clip = clip;
                source.Play();

                Destroy(audioObj, clip.length);
            }
        }
    }


}
