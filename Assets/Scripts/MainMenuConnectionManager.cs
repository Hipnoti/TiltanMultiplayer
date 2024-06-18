using System;
using System.Collections.Generic;
using System.Net.Mime;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuConnectionManager : MonoBehaviourPunCallbacks
{
    private const string LOBBY_DEFAULT_NAME = "OurCoolLobby";
    private const string ROOM_DEFAULT_NAME = "OurCoolRoom";
    
    [SerializeField] private TextMeshProUGUI debugPhotonText;

    [SerializeField] private TMP_InputField roomNameInputField;

    [SerializeField] private Button[] joinRoomButtons;
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We successfuly connected to Photon");
        base.OnConnectedToMaster();
        ToggleJoinRoomButtonsState(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
      //  cause == 
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 4,
        };
        PhotonNetwork.CreateRoom("MyRoom",roomOptions);
        ToggleJoinRoomButtonsState(false);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created successfully!");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        ToggleJoinRoomButtonsState(true);
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby(new TypedLobby(LOBBY_DEFAULT_NAME, LobbyType.Default));
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log($"We successfully joined the lobby {PhotonNetwork.CurrentLobby}!");
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        ToggleJoinRoomButtonsState(false);
    }

    public void JoinRoomByName()
    {
        PhotonNetwork.JoinRoom(roomNameInputField.text);
        ToggleJoinRoomButtonsState(false);
    }

    public void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(roomNameInputField.text, null, null);
        ToggleJoinRoomButtonsState(false);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We successfully joined the room " + PhotonNetwork.CurrentRoom);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError($"We couldn't join the room because {message} return code is {returnCode}");
        ToggleJoinRoomButtonsState(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        ToggleJoinRoomButtonsState(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        foreach (RoomInfo roomInfo in roomList)
        {
            Debug.Log(roomInfo.Name);
        }
    }

    private void Start()
    {
        ToggleJoinRoomButtonsState(false);
    }

    private void Update()
    {
        debugPhotonText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    private void ToggleJoinRoomButtonsState(bool active)
    {
        foreach (Button joinRoomButton in joinRoomButtons)
        {
            joinRoomButton.interactable = active;
        }
    }
}
