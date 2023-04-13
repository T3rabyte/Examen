using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    private Lobby lobby;

    [SerializeField]
    private TMP_InputField lobbyName;
    [SerializeField]
    private Toggle isLobbyPrivate;

    async void Start()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            await InitializeUnityService();
        };
        Debug.Log(UnityServices.State);
    }

    private async Task InitializeUnityService() 
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreatLobby() 
    {
        if (lobbyName.text == string.Empty)
            return;
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = isLobbyPrivate.isOn
            };

            lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName.text, 2, createLobbyOptions);
            InvokeRepeating("LobbyHeartbeat", 20f, 20f);

            Debug.Log("lobby: " + lobby.Name + " " + lobby.MaxPlayers);
        } catch (LobbyServiceException ex) 
        {
            Debug.Log(ex);
        }
    }

    async void LobbyHeartbeat() 
    {
        if (lobby == null)
            CancelInvoke("LobbyHeartbeat");
        else
            await LobbyService.Instance.SendHeartbeatPingAsync(lobby.Id);
    }

    public async Task<List<Lobby>> GetLobbyList() 
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized) 
        {
            await InitializeUnityService();
        };
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

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.LobbyCode);
            }

            return queryResponse.Results;
        }
        catch (LobbyServiceException ex) 
        {
            Debug.Log(ex);
            return new List<Lobby>();
        }
    }
}
