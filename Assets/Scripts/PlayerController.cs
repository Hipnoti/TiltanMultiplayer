using System;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float speed = 10;
    private static readonly int XMovement = Animator.StringToHash("XMovement");
    private static readonly int ZMovement = Animator.StringToHash("ZMovement");

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        Vector3 movementVector = new Vector3();
        if (Input.GetKey(KeyCode.W))
            movementVector.z = 1;
        if (Input.GetKey(KeyCode.S))
            movementVector.z = -1;
        if (Input.GetKey(KeyCode.D))
            movementVector.x = 1;
        if (Input.GetKey(KeyCode.A))
            movementVector.x = -1;

        transform.Translate(movementVector * (Time.deltaTime * speed));

        playerAnimator.SetFloat(XMovement, movementVector.x);
        playerAnimator.SetFloat(ZMovement, movementVector.z);
    }
}