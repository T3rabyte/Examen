using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject lobbyView;
    [SerializeField]
    private GameObject lobbyViewContent;
    [SerializeField]
    private GameObject lobbyListObj;
    [SerializeField]
    private GameObject lobbyCreatePanel;

    private List<GameObject> lobbyListItems = new List<GameObject>();

    public async void ShowLobbyList() 
    {
        List<Lobby> lobbyList = await lobbyPanel.GetComponent<LobbyManager>().GetLobbyList();
        foreach (Lobby lobby in lobbyList) 
        {
            GameObject lobbyObj = Instantiate(lobbyListObj) as GameObject;
            lobbyObj.transform.Find("Lobby Name").GetComponentInChildren<TMP_Text>().text = lobby.Name;
            lobbyObj.transform.SetParent(lobbyViewContent.transform);
            lobbyObj.transform.localPosition = new Vector3(0, 0, 0);
            lobbyListItems.Add(lobbyObj);
        }
    }

    public void DestroyLobbyListItems() 
    {
        if (lobbyListItems != null)
        {
            foreach (GameObject listObj in lobbyListItems)
            {
                Destroy(listObj);
            }
            lobbyListItems = new List<GameObject>();
        }
    }

    public void ShowLobbyPanel()
    {
        ShowLobbyList();
        mainMenu.SetActive(true);
        lobbyPanel.SetActive(false);
    }

    public void HideLobbyPanel() 
    {
        DestroyLobbyListItems();
        mainMenu.SetActive(true);
        lobbyPanel.SetActive(false);
    }

    public void ShowLobbyCreate() 
    {
        DestroyLobbyListItems();
        lobbyView.SetActive(false);
        lobbyCreatePanel.SetActive(true);
    }

    public void HideLobbyCreate()
    {
        ShowLobbyList();
        lobbyCreatePanel.SetActive(false);
        lobbyView.SetActive(true);
    }
}
