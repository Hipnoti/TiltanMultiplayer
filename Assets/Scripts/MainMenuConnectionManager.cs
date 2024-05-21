using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class MainMenuConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI debugPhotonText;
    
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We successfuly connected to Photon");
        base.OnConnectedToMaster();
    }

    private void Update()
    {
        debugPhotonText.text = PhotonNetwork.NetworkClientState.ToString();
    }
}
