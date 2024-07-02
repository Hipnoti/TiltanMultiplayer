using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{
    [SerializeField] private float speed = 20;

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
    }
}
