using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [HideInInspector]
    public Lobby joinedLobby;

    private MainMenuUI mainMenuUI;
    private RoomManager roomManager;

    private UnityAction leaveFromLobbyAction;

    [SerializeField]
    private TMP_InputField playerNameInput;

    [SerializeField]
    private TMP_InputField quickJoinLobbyName;

    [SerializeField]
    private TMP_InputField lobbyCreateName;
    [SerializeField]
    private Toggle isLobbyPrivate;

    [SerializeField]
    private Button roomBackBtn;
    [SerializeField]
    private Button roomStartBtn;

    private readonly List<string> randomPlayerNames = new() { "Ben", "Jan", "Henk", "Sjaak", "Harry"};

    private void Start()
    {
        mainMenuUI = GetComponent<MainMenuUI>();
        roomManager = GetComponent<RoomManager>();
    }

    private async Task InitializeUnityService() 
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public void SetRandomPlayerName() 
    {
        playerNameInput.text = randomPlayerNames[Random.Range(0, randomPlayerNames.Count)];
    }

    public async void CreateLobby() 
    {
        if (lobbyCreateName.text == string.Empty)
            return;
        if (playerNameInput.text == string.Empty)
            SetRandomPlayerName();
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = isLobbyPrivate.isOn,
                Player = NewPlayer()
            };
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyCreateName.text, 2, createLobbyOptions);
            InvokeRepeating("LobbyHeartbeat", 20f, 20f);
            InvokeRepeating("PollForLobbyUpdates", 1f, 1.5f);

            mainMenuUI.ShowPanel("Room Panel");
            roomManager.InstantiateRoomItems(joinedLobby);
            SetLobbyleaveButton();
        } catch (LobbyServiceException ex) 
        {
            Debug.Log(ex);
        }
    }

    private Player NewPlayer(string playerRole = "Neutral") 
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "playerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerNameInput.text)},
                { "playerRole", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerRole)}
            }
        };
    }

    public async void JoinLobbyById(string lobbyId) 
    {
        if (playerNameInput.text == string.Empty)
            SetRandomPlayerName();
        try
        {
            JoinLobbyByIdOptions option = new JoinLobbyByIdOptions()
            {
                Player = NewPlayer()
            };
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, option);
            InvokeRepeating("PollForLobbyUpdates", 1f, 1.5f);
            mainMenuUI.ShowPanel("Room Panel");
            roomManager.InstantiateRoomItems(joinedLobby);
            SetLobbyleaveButton();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void SetLobbyleaveButton() 
    {
        leaveFromLobbyAction = () => { LeaveFromLobby(joinedLobby.Id); };
        roomBackBtn.onClick.AddListener(leaveFromLobbyAction);
    }

    public async void JoinLobbyByCode() 
    {
        if (quickJoinLobbyName.text == string.Empty)
            return;
        if (playerNameInput.text == string.Empty)
            SetRandomPlayerName();
        try
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions() 
            {
                Player = NewPlayer()
            };
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(quickJoinLobbyName.text, options);
            InvokeRepeating("PollForLobbyUpdates", 1f, 1.5f);
            SetLobbyleaveButton();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    async void LobbyHeartbeat() 
    {
        try
        {
            if (joinedLobby == null)
                CancelInvoke("LobbyHeartbeat");
            else
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException ex) 
        {
            Debug.Log(ex);
        }
    }

    async void PollForLobbyUpdates() 
    {
        try
        {
            if (joinedLobby == null)
                ResetRoom();
            else
            {
                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                if (!joinedLobby.Players.Contains(joinedLobby.Players.Where(player => player.Id == AuthenticationService.Instance.PlayerId).SingleOrDefault()))
                {
                    ResetRoom();
                }
                else
                    roomManager.UpdateRolesInLobby(joinedLobby);
            }
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
            ResetRoom();
        }
    }

    public void ResetRoom() 
    {
        CancelInvoke();
        mainMenuUI.DestroyItemsOnList(new List<List<GameObject>> { mainMenuUI.roomListItems });
        roomBackBtn.onClick.RemoveListener(leaveFromLobbyAction);
        mainMenuUI.ShowPanel("Lobby Panel");
        joinedLobby = null;

        foreach (GameObject contentObj in roomManager.roomContentList) 
        {
            Debug.Log("1");
            contentObj.transform.parent.parent.GetComponent<Button>().interactable = true;
        }
    }

    public async Task<List<Lobby>> GetLobbyList() 
    {
        if (UnityServices.State != ServicesInitializationState.Initialized) 
        {
            await InitializeUnityService();
        }
        Debug.Log(UnityServices.State);
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.EQ)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            return queryResponse.Results;
        }
        catch (LobbyServiceException ex) 
        {
            Debug.Log(ex);
            return new List<Lobby>();
        }
    }

    public async void LeaveFromLobby(string lobbyId)
    {
        try
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            if (playerId == joinedLobby.HostId)
                await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
            else
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
            ResetRoom();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void RemoveFromLobby(string lobbyId, GameObject player) 
    {
        try
        {
            string playerId = player.GetComponent<PlayerMenuInfo>().playerInstanceId;
            await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
            mainMenuUI.roomListItems.Remove(player);
            Destroy(player);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
