using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerlook : MonoBehaviour
{
    public float sensitivity = 100f;
    public Transform playerobject;
    float xrotate = 0f;
    public bool paused=false;
  
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            float Xaxis = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float Yaxis = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            xrotate -= Yaxis;
            xrotate = Mathf.Clamp(xrotate, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xrotate, 0f, 0f);
            playerobject.Rotate(Vector3.up * Xaxis);
        }
    }
}
