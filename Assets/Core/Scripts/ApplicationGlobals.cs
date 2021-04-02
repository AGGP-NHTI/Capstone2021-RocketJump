using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationGlobals : MonoBehaviour
{
	public static ApplicationGlobals instance;
	public List<GameObject> weaponPrefabList;

	private void Awake()
	{
		if (instance)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
		instance = this;
		AssignGlobalIndices();
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

	public void AssignGlobalIndices()
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
}
