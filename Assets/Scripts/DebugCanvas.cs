using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI isMasterClientText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
#if UNITY_EDITOR
    private void Update()
    {
        isMasterClientText.text = "Is Master Client :" + PhotonNetwork.IsMasterClient.ToString();
    }
#endif

}
