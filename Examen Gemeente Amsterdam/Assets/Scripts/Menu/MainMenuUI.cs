using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject lobbyViewContent;
    
    [SerializeField]
    private GameObject lobbyListObj;

    private TMP_Text errorText;
    [SerializeField]
    private GameObject errorMessageObj;

    [SerializeField]
    private List<GameObject> panelList;

    private List<GameObject> lobbyListItems = new();
    public  List<GameObject> roomListItems = new();

    private LobbyManager lobbyManager;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
        lobbyManager = GetComponent<LobbyManager>();
        errorText = errorMessageObj.GetComponentInChildren<TMP_Text>();
    }

    private async void InstantiateLobbyList() 
    {
        List<Lobby> lobbyList = await GetComponent<LobbyManager>().GetLobbyList();
        foreach (Lobby lobby in lobbyList) 
        {
            GameObject lobbyUIObj = Instantiate(lobbyListObj, lobbyViewContent.transform);
            lobbyUIObj.transform.Find("Lobby Name").GetComponent<TMP_Text>().text = lobby.Name;
            lobbyUIObj.transform.Find("Btn_Join").GetComponent<Button>().onClick.AddListener(delegate { lobbyManager.JoinLobbyById(lobby.Id); });
            lobbyListItems.Add(lobbyUIObj);
        }
    }

    public void DestroyItemsOnList(List<List<GameObject>> lists) 
    {
        foreach (List<GameObject>list in lists)
        {
            if (list != null)
            {
                foreach (GameObject listObj in list)
                {
                    Destroy(listObj);
                }
            }
            list.Clear();
        }
    }

    

    public void ShowPanel(string panelName) 
    {
        DestroyItemsOnList(new List<List<GameObject>> { lobbyListItems, roomListItems });
        if (panelName == lobbyPanel.name)
            InstantiateLobbyList();

        if (panelName == "Quit Game Panel")
        {
            panelList.Where(obj => obj.name == "Quit Game Panel").SingleOrDefault().SetActive(true);
            return;
        }

        foreach (GameObject panel in panelList)
        {
            if (panelName == "Create Lobby Panel" && panel.name == "Lobby Panel")
                continue;

            else if (panel.name != panelName)
                panel.SetActive(false);

            else
                panel.SetActive(true);
        }
    }

    public void ShowError(string message) 
    {
        errorText.text = message;
        errorMessageObj.SetActive(true);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
