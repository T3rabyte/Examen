using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleManager : MonoBehaviour
{
    public string role;
    // Start is called before the first frame update
    void Start()
    {
        role = "Neutral";
        SceneManager.sceneLoaded += OnGameSceneLoaded;
        for (int i = 0; i < Object.FindObjectsOfType<DontDestroyOnLoad>().Length; i++) 
        {
            if (Object.FindObjectsOfType<DontDestroyOnLoad>()[i] != this) 
            {
                if (Object.FindObjectsOfType<DontDestroyOnLoad>()[i].name == gameObject.name)
                {
                    SceneManager.sceneLoaded -= OnGameSceneLoaded;
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game") 
        {
            GameObject player = GameObject.Find("Player");
            player.transform.position = GameObject.Find(role).transform.position;
        }
    }
}
