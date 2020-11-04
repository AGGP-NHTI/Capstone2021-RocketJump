using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class fireScript : MonoBehaviour
{
    public GameObject projectile;
    public float force = 10;
    public GameObject spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("FIRE");
            Instantiate(projectile, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}
