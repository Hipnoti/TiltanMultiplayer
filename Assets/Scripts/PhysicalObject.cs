using System;
using Photon.Pun;
using UnityEngine;

public class PhysicalObject : MonoBehaviour
{
    [SerializeField] private Rigidbody thisRB;
    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            thisRB.isKinematic = true;
        }
    }
}
