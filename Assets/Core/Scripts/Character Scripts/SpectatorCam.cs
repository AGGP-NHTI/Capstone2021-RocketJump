using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCam : Pawn
{
    Camera cam;
    public float speed = 5f;
    public float MouseSen = 300f;

    public float freeLookSens = 2.5f;
    public Transform playerBody;
    public float XRotate = 0f;
    public GameObject CharSel;
    
    void Awake()
    {
        if (!IsLocalPlayer)
        {
            GetComponent<Camera>().enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            float nRotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSens;
            float nRotY = transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * -freeLookSens;
            transform.localEulerAngles = new Vector3(nRotY, nRotX, 0f);
        }       
    }

    void FixedUpdate()
    {
        if (IsLocalPlayer)
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
            if (Input.GetKeyDown(KeyCode.Y))
            {
                GetComponent<AudioListener>().enabled = false;
                CharSel.SetActive(true);
                //gameObject.SetActive(false);
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
