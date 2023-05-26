using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleManager : NetworkBehaviour
{
    [SerializeField] private GameObject NetworkVarManagerObj;
    public string role;

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

    void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game") 
        {
            GameObject player = GameObject.Find("Player");
            player.transform.position = GameObject.Find(role).transform.position;
            GameObject spawnedObj = Instantiate(NetworkVarManagerObj);
        }
    }
}
