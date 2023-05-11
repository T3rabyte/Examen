using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : MonoBehaviour
{
    private new Camera camera;
    public GameObject player;
    public GameObject crosshair;
    public bool cameraFocusedToObj;
    public bool isPlayingAudio;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
                if (hit.transform.tag == "CameraInput")
                    SetCameraPosition(hit);
                else if (hit.transform.tag == "AudioInput" && !isPlayingAudio)
                    SelectAudioClip(hit.transform.gameObject);
        }
        if (Input.GetMouseButtonDown(1) && cameraFocusedToObj) 
        {
            gameObject.transform.SetParent(player.transform);
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
            player.GetComponent<PlayerController>().canRotate = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crosshair.SetActive(true);
            cameraFocusedToObj = false;
        }
    }

    void SetCameraPosition(RaycastHit hit) 
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
        cameraFocusedToObj = true;
    }

    void SelectAudioClip(GameObject audioSourceObj) 
    {
        List<AudioClip> audioClipList = audioSourceObj.GetComponent<AudioInfo>().audioClips;
        AudioSource audioSource = audioSourceObj.GetComponent<AudioSource>();
        audioSource.clip = audioClipList[Random.Range(0, audioClipList.Count)];
        isPlayingAudio = true;
        StartCoroutine(PlayAudioClip(audioSource));
    }

    IEnumerator PlayAudioClip(AudioSource audioSource)
    {
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        isPlayingAudio = false;
    }
}
