using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EditorController : MonoBehaviour
{
    private float speed = 4;
    private float sensitivity = 1;
    private Vector2 camRot;
    private Camera cam;
    private float rayDist = 64;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        // Controls
        if (Input.GetKey(KeyCode.W)) transform.position += transform.forward * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.A)) transform.position -= transform.right * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.S)) transform.position -= transform.forward * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.D)) transform.position += transform.right * Time.deltaTime * speed;

        if (Input.GetKeyDown(KeyCode.LeftShift)) speed *= 2;
        if (Input.GetKeyUp(KeyCode.LeftShift))   speed *= 0.5f;
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            camRot.x += Input.GetAxis("Mouse X") * sensitivity;
            camRot.y -= Input.GetAxis("Mouse Y") * sensitivity;
            camRot.x = Mathf.Repeat(camRot.x, 360);
            cam.transform.rotation = Quaternion.Euler(camRot.y, camRot.x, 0);
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Debug.DrawLine(hit.point, hit.point + (hit.normal * 3), Color.green, 1000);
                Debug.DrawLine(hit.point, transform.position, Color.blue, 100);
                Debug.Log(hit.transform.name);
            }
        }
    }
}
