using System;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{   
    private const string ProjectilePrefabName = "Prefabs\\Projectile";
    private static readonly int XMovement = Animator.StringToHash("XMovement");
    private static readonly int ZMovement = Animator.StringToHash("ZMovement");
    
    [Header("Projectile")]
    [SerializeField] private Transform projectileSpawnTransform;
    
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float speed = 10;

    private Camera cachedCamera;

    private Vector3 raycastPos;
    
    private void Start()
    {
        cachedCamera = Camera.main;
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        Ray ray = cachedCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // hit.point contains the world position where the ray hit.
            raycastPos = hit.point;
        }
        
        Vector3 movementVector = new Vector3();
        if (Input.GetKey(KeyCode.W))
            movementVector.z = 1;
        if (Input.GetKey(KeyCode.S))
            movementVector.z = -1;
        if (Input.GetKey(KeyCode.D))
            movementVector.x = 1;
        if (Input.GetKey(KeyCode.A))
            movementVector.x = -1;

        if(Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();
        
        Vector3 directionToFace = raycastPos - gameObject.transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(directionToFace);
        Vector3 eulerRotation = lookAtRotation.eulerAngles;
        eulerRotation.x = 0;
        eulerRotation.z = 0;
        transform.eulerAngles = eulerRotation;
        transform.Translate(movementVector * (Time.deltaTime * speed));
        
        
        playerAnimator.SetFloat(XMovement, movementVector.x);
        playerAnimator.SetFloat(ZMovement, movementVector.z);
    }

    private void Shoot()
    {
        GameObject projectile = PhotonNetwork.Instantiate(ProjectilePrefabName,
            projectileSpawnTransform.position, projectileSpawnTransform.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(raycastPos, 2);
    }
}