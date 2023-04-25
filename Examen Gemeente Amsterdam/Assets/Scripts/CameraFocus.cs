using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public Camera camera;
    public GameObject player;
    public GameObject crosshair;
    public bool inObjectPosition;


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Monitor")
            {
                Transform objectHit = hit.transform;
                Transform camPos = objectHit.Find("Cam Position");
                gameObject.transform.SetParent(camPos);
                gameObject.transform.localPosition = new Vector3(0, 0, 0);
                player.GetComponent<PlayerController>().canRotate = false;
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                crosshair.SetActive(false);
                inObjectPosition = true;
            }
        }
        if (Input.GetMouseButtonDown(1) && inObjectPosition) 
        {
            gameObject.transform.SetParent(player.transform);
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
            player.GetComponent<PlayerController>().canRotate = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crosshair.SetActive(true);
            inObjectPosition = false;
        }
    }
}
