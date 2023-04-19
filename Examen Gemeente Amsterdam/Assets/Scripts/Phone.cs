using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
     public Camera camera;
    public GameObject player;
    public GameObject crosshair;
    public bool inObjectPosition;

    private AudioListener audioListener;

    public AudioSource audioSource;
    public AudioClip[] tip;
    private AudioClip shootClip;


    void Start()
    {
        audioListener = gameObject.GetComponent<AudioListener>();
    }
 


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                Transform camPos = objectHit.Find("y");
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
            
            int index = Random.Range(0, tip.Length);
            shootClip = tip[index];
            audioSource.clip = shootClip;
            audioSource.Play();

            /*gameObject.transform.SetParent(player.transform);
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
            player.GetComponent<PlayerController>().canRotate = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crosshair.SetActive(true);
            inObjectPosition = false;*/
        }

        
    }
}
