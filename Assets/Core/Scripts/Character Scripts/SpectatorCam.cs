using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Spawning;

public class SpectatorCam : Pawn
{
    public float speed = 5f;
    public float MouseSen = 300f;

    public float freeLookSens = 2.5f;
    public Transform playerBody;
    public float XRotate = 0f;
    public GameObject CharSel;
    public GameObject text;
    
    void Awake()
    {
        Debug.Log("awake");
        if (IsLocalPlayer)
        {
            Debug.Log("local");
        }
        else
        {
            Debug.Log("not local");
        }
        /*
        if (SpawnManager.GetLocalPlayerObject() == this.gameObject)
        {
            Debug.Log("here");
        }
        
        Debug.Log(SpawnManager.GetLocalPlayerObject().name);
        */
        if (!IsOwner)
        {
            Debug.Log("not owner");
            GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            Debug.Log("owner");
            Cursor.lockState = CursorLockMode.Locked;
            foreach (SpawnPointManager s in FindObjectsOfType<SpawnPointManager>())
            {
                this.gameObject.transform.position = s.CamSpawn.transform.position;
            }
        }
        
    }

    void Update()
    {
        if (IsOwner)
        {
            float nRotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSens;
            float nRotY = transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * -freeLookSens;
            transform.localEulerAngles = new Vector3(nRotY, nRotX, 0f);

            if (Input.GetKeyDown(KeyCode.Y))
            {
                GetComponent<AudioListener>().enabled = false;
                text.SetActive(false);
                CharSel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                //gameObject.SetActive(false);
            }
        }       
    }

    void FixedUpdate()
    {
        if (IsOwner)
        {
            GetComponent<Camera>().enabled = true;

            if (Input.GetKey(KeyCode.W))
            {
                forward(speed);                
            }
            if (Input.GetKey(KeyCode.S))
            {
                forward(-speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                sideways(-speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                sideways(speed);
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Vertical(-speed);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Vertical(speed);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    forward(speed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    forward(-speed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    sideways(-speed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    sideways(speed);
                }
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Vertical(-speed);
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    Vertical(speed);
                }
            }           
        }
    }

    public void forward(float speed)
    {
        Vector3 location = gameObject.transform.position;

        location += (speed * Time.fixedDeltaTime * transform.forward);
        gameObject.transform.position = location;
    }

    public void sideways(float speed)
    {
        Vector3 location = gameObject.transform.position;

        location += (speed * Time.fixedDeltaTime * transform.right);
        gameObject.transform.position = location;
    }

    public void Vertical(float speed)
    {
        Vector3 location = gameObject.transform.position;

        location += (speed * Time.fixedDeltaTime * transform.up);
        gameObject.transform.position = location;
    }
}
