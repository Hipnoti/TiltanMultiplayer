using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{    
    private const string ProjectilePrefabName = "Prefabs\\Projectile";
    private const string ProjectileTag = "Projectile";
    private const string RecievedamageRPC = "RecieveDamage";
    private static readonly int XMovement = Animator.StringToHash("XMovement");
    private static readonly int ZMovement = Animator.StringToHash("ZMovement");

    [SerializeField] private int HP = 100;
    
    [Header("Projectile")]
    [SerializeField] private Transform projectileSpawnTransform;
    
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float speed = 10;
    [SerializeField] private ParticleSystem hitParticle;

    [SerializeField] public Material[] projectileColors;
    
    [SerializeField] private int ourSerializedParameter;
    public void PlayHitEffect()
    {
        hitParticle.Play();
    }
    
    private Camera cachedCamera;

    private Vector3 raycastPos;
    private Vector3 movementVector = new Vector3();
    
    private void Start()
    {
        cachedCamera = Camera.main;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Ray ray = cachedCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // hit.point contains the world position where the ray hit.
                raycastPos = hit.point;
            }

            movementVector = new Vector3();
            if (Input.GetKey(KeyCode.W))
                movementVector.z = 1;
            if (Input.GetKey(KeyCode.S))
                movementVector.z = -1;
            if (Input.GetKey(KeyCode.D))
                movementVector.x = 1;
            if (Input.GetKey(KeyCode.A))
                movementVector.x = -1;

            if (Input.GetKeyDown(KeyCode.Mouse0))
                Shoot();

            Vector3 directionToFace = raycastPos - gameObject.transform.position;
            Quaternion lookAtRotation = Quaternion.LookRotation(directionToFace);
            Vector3 eulerRotation = lookAtRotation.eulerAngles;
            eulerRotation.x = 0;
            eulerRotation.z = 0;
            transform.eulerAngles = eulerRotation;
            transform.Translate(movementVector * (Time.deltaTime * speed));
        }

        playerAnimator.SetFloat(XMovement, movementVector.x);
        playerAnimator.SetFloat(ZMovement, movementVector.z);
    }

    private void Shoot()
    {
        int randomMaterialIndex = Random.Range(0, projectileColors.Length);
        GameObject projectile = PhotonNetwork.Instantiate(ProjectilePrefabName,
            projectileSpawnTransform.position, projectileSpawnTransform.rotation, 0,
            new object []{ randomMaterialIndex});
        
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawSphere(raycastPos, 2);
    // }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ProjectileTag))
        {
            Projectile otherProjectile = other.GetComponent<Projectile>();
            
            if (otherProjectile.photonView.Owner.ActorNumber == photonView.Owner.ActorNumber)
                return;

            PlayHitEffect();
            if (otherProjectile.photonView.IsMine)
            {
                //run login that affect other players! only the projectile owner should do that
                StartCoroutine(DestroyDelay(5f, otherProjectile.gameObject));
                photonView.RPC(RecievedamageRPC, RpcTarget.All, 10);
            }
            
            otherProjectile.visualPanel.SetActive(false);
            //add bool for projectile hit
        }
    }

    IEnumerator DestroyDelay(float delay, GameObject otherObject)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(otherObject);
    }

    [PunRPC]
    private void RecieveDamage(int damageAmount)
    {
        HP -= damageAmount;
        Debug.Log("Hp left is " + HP);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            float somea, someb;
            somea = movementVector.x;
            someb = movementVector.z;
            stream.SendNext(somea);
            stream.SendNext(someb);
        }
        else
        {
            if (stream.IsReading)
            {
                movementVector.x = (float)stream.ReceiveNext();
                movementVector.z = (float)stream.ReceiveNext();
            }
        }
    }
}