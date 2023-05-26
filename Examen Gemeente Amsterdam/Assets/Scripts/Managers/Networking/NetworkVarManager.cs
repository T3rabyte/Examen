using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkVarManager : NetworkBehaviour
{
    public bool gameFinished = false;
    
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    //execute on server 
    [ServerRpc(RequireOwnership = false)]
    public void ShowScreenServerRpc()
    {
        //The server then tell every Clients to execute the "ShowScreenClientRpc" Function
        ShowScreenClientRpc();
    }

    [ClientRpc]
    void ShowScreenClientRpc()
    {
        ShowScreen();
    }

    void ShowScreen()
    {
        if (gameFinished == true)
            gameManager.Win();
        else
            gameManager.Lose();
    }
}
