using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class MultiplayerGameManager : MonoBehaviour
{
    private const string PlayerPrefabName = "Prefabs\\Player Prefab";

    [Header("Spawn Points")]
    [SerializeField] private bool randomizeSpawnPoint;

    [SerializeField] private Transform masterClientSpawnPoint;
    [SerializeField] private Transform peasantClientSpawnPoint;
    
    private void Start()
    {
        // if(randomizeSpawnPoint)
        // Transform targetSpawnPoint = PhotonNetwork.IsMasterClient ? masterClientSpawnPoint : peasantClientSpawnPoint;
        
    //PhotonNetwork.Instantiate(PlayerPrefabName, targetSpawnPoint.position, targetSpawnPoint.rotation);    PhotonNetwork.Instantiate(PlayerPrefabName, targetSpawnPoint.position, targetSpawnPoint.rotation);
        PhotonNetwork.Instantiate(PlayerPrefabName, Vector3.zero, Quaternion.identity);
    }
}
