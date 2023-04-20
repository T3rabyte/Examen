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

    private bool Isplaying = false;


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

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Phone" && Isplaying == false)
            {
                Transform objectHit = hit.transform;
                Transform camPos = objectHit.Find("Phone Position");
                
                int index = Random.Range(0, tip.Length);
                shootClip = tip[index];
                audioSource.clip = shootClip;
                audioSource.Play();
                Isplaying=true;
                StartCoroutine (WaitForAudio());
            }
        }

        
    }

    IEnumerator WaitForAudio()
    {
        audioSource.Play ();
        yield return new WaitWhile (()=> audioSource.isPlaying);
        Isplaying = false;
    
    }
}
