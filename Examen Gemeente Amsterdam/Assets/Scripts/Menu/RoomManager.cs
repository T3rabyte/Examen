using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject roomPlayerObj;
    [SerializeField]
    private GameObject roomCodeObj;

    public List<GameObject> roomContentList;

    private MainMenuUI mainMenuUI;
    private LobbyManager lobbyManager;
    [SerializeField]
    private RelayManager relayManager;

    private void Start()
    {
        mainMenuUI = GetComponent<MainMenuUI>();
        lobbyManager = GetComponent<LobbyManager>();
    }


    public void InstantiateRoomItems(Lobby lobby)
    {
        foreach (Player player in lobby.Players)
        {
            if (mainMenuUI.roomListItems.Contains(mainMenuUI.roomListItems.Where(obj => obj.GetComponent<PlayerMenuInfo>().playerInstanceId == player.Id).SingleOrDefault()))
                continue;

            GameObject playerUIObj = Instantiate(roomPlayerObj, roomContentList.Where(obj => obj.name == player.Data["playerRole"].Value).SingleOrDefault().transform);
            playerUIObj.transform.Find("Player Name").GetComponentInChildren<TMP_Text>().text = player.Data["playerName"].Value;
            playerUIObj.GetComponent<PlayerMenuInfo>().playerInstanceId = player.Id;
            playerUIObj.GetComponent<PlayerMenuInfo>().playerInstanceRole = player.Data["playerRole"].Value;

            if (player.Id == lobby.HostId)
                playerUIObj.transform.Find("Host Image").gameObject.SetActive(true);

            if (AuthenticationService.Instance.PlayerId == lobby.HostId && player.Id != lobby.HostId)
            {
                playerUIObj.transform.Find("Btn_KickPlayer").gameObject.SetActive(true);
                playerUIObj.transform.Find("Btn_KickPlayer").GetComponent<Button>().onClick.AddListener(delegate { lobbyManager.RemoveFromLobby(lobby.Id, playerUIObj); });
            }

            roomContentList.Where(obj => obj.name == playerUIObj.GetComponent<PlayerMenuInfo>().playerInstanceRole).SingleOrDefault().transform.parent.parent.GetComponent<Button>().interactable = false;
            mainMenuUI.roomListItems.Add(playerUIObj);
        }
        if (lobby.IsPrivate)
        {
            roomCodeObj.transform.Find("Room Code").GetComponentInChildren<TMP_Text>().text = lobby.LobbyCode;
            roomCodeObj.SetActive(true);
        }
        if(AuthenticationService.Instance.PlayerId == lobby.HostId)
            lobbyManager.roomStartBtn.gameObject.SetActive(true);
    }

    public async void UpdatePlayerRole(string newRole) 
    {
        try
        {
            await LobbyService.Instance.UpdatePlayerAsync(lobbyManager.joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    { "playerRole", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, newRole)}
                }
            });
            GameObject playerUIObj = mainMenuUI.roomListItems.Where(obj => obj.GetComponent<PlayerMenuInfo>().playerInstanceId == AuthenticationService.Instance.PlayerId).SingleOrDefault();
            playerUIObj.transform.SetParent(roomContentList.Where(obj => obj.name == newRole).SingleOrDefault().transform);
            roomContentList.Where(obj => obj.name == playerUIObj.GetComponent<PlayerMenuInfo>().playerInstanceRole).SingleOrDefault().GetComponentInParent<Button>().interactable = true;
            playerUIObj.GetComponent<PlayerMenuInfo>().playerInstanceRole = newRole;
            roomContentList.Where(obj => obj.name == newRole).SingleOrDefault().GetComponentInParent<Button>().interactable = false;
        }
        catch(LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public void UpdateRolesInLobby(Lobby joinedLobby) 
    {
        List<string> playerIds = new();
        foreach (Player player in joinedLobby.Players)
            playerIds.Add(player.Id);

        for (int i = mainMenuUI.roomListItems.Count -1; i >= 0; i--)
            if (!playerIds.Contains(mainMenuUI.roomListItems[i].GetComponent<PlayerMenuInfo>().playerInstanceId))
            {
                GameObject playerUIObj = mainMenuUI.roomListItems[i];
                mainMenuUI.roomListItems.Remove(playerUIObj);
                Destroy(playerUIObj);
            }

        foreach (Player player in joinedLobby.Players) 
        {
            if (!mainMenuUI.roomListItems.Contains(mainMenuUI.roomListItems.Where(obj => obj.GetComponent<PlayerMenuInfo>().playerInstanceId == player.Id).SingleOrDefault()))
                InstantiateRoomItems(joinedLobby);
            GameObject playerUIObj = mainMenuUI.roomListItems.Where(obj => obj.GetComponent<PlayerMenuInfo>().playerInstanceId == player.Id).SingleOrDefault();
            playerUIObj.transform.SetParent(roomContentList.Where(obj => obj.name == player.Data["playerRole"].Value).SingleOrDefault().transform);
        }
    }

    public async void StartGame()
    {
        if (AuthenticationService.Instance.PlayerId == lobbyManager.joinedLobby.HostId)
            try
            {
                Debug.Log("start game");

                string relayCode = await relayManager.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(lobbyManager.joinedLobby.Id, new UpdateLobbyOptions {
                    Data = new Dictionary<string, DataObject> {
                        { "RelayKey", new DataObject(DataObject.VisibilityOptions.Member, relayCode)}
                    }
                });

                lobbyManager.joinedLobby = lobby;
                SceneManager.LoadScene("SampleScene");
            }
            catch (LobbyServiceException ex) 
            {
                Debug.Log(ex);
            }
    }
}
