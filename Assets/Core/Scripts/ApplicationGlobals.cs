using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationGlobals : MonoBehaviour
{
	public static ApplicationGlobals instance;
	public List<GameObject> weaponPrefabList;
	public List<AudioClip> audioClips;

	private void Awake()
	{
		if (instance)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
		instance = this;
		AssignWeaponGlobalIndices();
	}

	public static GameObject GetWeaponPrefab(int weaponIndex)
	{
		if (weaponIndex < 0 || weaponIndex >= instance.weaponPrefabList.Count)
		{
			Debug.LogError("Weapon index out of bounds");
			return null;
		}
		return instance.weaponPrefabList[weaponIndex];
	}

	public static int GetWeaponIndex(GameObject weaponPrefab)
	{
		int index = instance.weaponPrefabList.IndexOf(weaponPrefab);
		if (index != -1)
		{
			return index;
		}
		else
		{
			Debug.LogError($"Could not find the index of weapon {weaponPrefab.name}. Has it been added to ApplicationGlobals?");
			return -1;
		}
	}

	public void AssignWeaponGlobalIndices()
	{
		int i = 0;
		foreach(GameObject g in weaponPrefabList)
		{
			if (g.TryGetComponent(out Weapon w))
			{
				w.globalIndex = i;
			}
			else
			{
				Debug.LogError("Non-weapon found in global weapons list");
			}
			i++;
		}
	}

	public static AudioClip GetAudioClipByName(string name)
	{
		foreach (AudioClip clip in instance.audioClips)
		{
			if(clip.name == name)
            {
				return clip;
            }
		}

		return null;
	}

	//public static int GetAudioClipIndex(AudioClip clip)
	//{
	//	int index = instance.audioClips.IndexOf(clip);
	//	if (index != -1)
	//	{
	//		return index;
	//	}
	//	else
	//	{
	//		Debug.LogError($"Could not find the index of weapon {weaponPrefab.name}. Has it been added to ApplicationGlobals?");
	//		return -1;
	//	}
	//}

	//public void AssignAudioClipGlobalIndices()
	//{
	//	int i = 0;
	//	foreach (AudioClip c in weaponPrefabList)
	//	{
	//		if (c.TryGetComponent(out  w))
	//		{
	//			w.globalIndex = i;
	//		}
	//		else
	//		{
	//			Debug.LogError("Non-weapon found in global weapons list");
	//		}
	//		i++;
	//	}
	//}

	//public static GameObject GetAudioClip(int AudioIndex)
	//{
	//	if (AudioIndex < 0 || AudioIndex >= instance.weaponPrefabList.Count)
	//	{
	//		Debug.LogError("Weapon index out of bounds");
	//		return null;
	//	}
	//	return instance.weaponPrefabList[AudioIndex];
	//}
}
