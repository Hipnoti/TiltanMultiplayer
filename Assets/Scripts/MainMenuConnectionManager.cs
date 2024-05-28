using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class MainMenuConnectionManager : MonoBehaviourPunCallbacks
{
    private const string LOBBY_DEFAULT_NAME = "OurCoolLobby";
    [SerializeField] private TextMeshProUGUI debugPhotonText;

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We successfuly connected to Photon");
        base.OnConnectedToMaster();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("MyRoom");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created successfully!");
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
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We successfully joined the room " + PhotonNetwork.CurrentRoom);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        foreach (RoomInfo roomInfo in roomList)
        {
            Debug.Log(roomInfo.Name);
        }
    }

    private void Update()
    {
        debugPhotonText.text = PhotonNetwork.NetworkClientState.ToString();
    }
}
