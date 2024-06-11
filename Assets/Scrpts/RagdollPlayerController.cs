using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using static UnityTutorial.PlayerControl.PlayerController;

public class RagdollPlayerController : NetworkBehaviour
{
    public Animator animator;
    public float speed;
    public float jumpForce;
    public CamControl cameraController;

    [SerializeField] private Transform spawnedObjectPrefab;

    private Transform spawnedObjectTransform;
    private bool _hasAnimator;

    public Rigidbody hips;
    public bool isGrounded;
    [SerializeField] private LimbHealth[] limbHealth; // LimbHealth bileþeni için referans


    void Start()
    {
        hips = GetComponent<Rigidbody>();
        limbHealth = GetComponentsInChildren<LimbHealth>(); // LimbHealth bileþenini al
    }

    private void FixedUpdate()
    {

        Move();

        if (!IsLocalPlayer) { return; }

        // Karakterin caný 0 deðilse hareket kontrollerini gerçekleþtir
        if (checkLimbs())
        {
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetBool("isWalking", true);
                hips.AddForce(hips.transform.forward * speed);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            if (Input.GetKey(KeyCode.A))
            {
                hips.AddForce(-hips.transform.right * speed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("isWalking", true);
                hips.AddForce(-hips.transform.forward * speed);
            }
            else if (!Input.GetKey(KeyCode.W))
            {
                animator.SetBool("isWalking", false);
            }

            if (Input.GetKey(KeyCode.D))
            {
                hips.AddForce(hips.transform.right * speed);
            }

            if (Input.GetAxis("Jump") > 0)
            {
                if (isGrounded)
                {
                    hips.AddForce(new Vector3(0, jumpForce, 0));
                    isGrounded = false;
                }
            }
        }
        else
        {
            // Karakterin caný 0 ise hareket etme
            animator.SetBool("isWalking", false);
        }
    }

    bool checkLimbs()
    {
        foreach (LimbHealth lh in limbHealth)
        {
            if (lh.health <= 0)
                return false;
        }
        return true;
    }

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
            new MyCustomData
            {
                _int = 56,
                _bool = true,
            }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {

            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + ";  " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
            base.OnNetworkSpawn();
            if (IsLocalPlayer) { return; }
            cameraController.GetComponent<Camera>().enabled = false;
            //cameraController.enabled = false;
        };

    }

    private void Move()
    {
        if (!IsLocalPlayer) { return; }



        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

            //TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } });
            /*
            randomNumber.Value = new MyCustomData {
                _int = 10,
                _bool = false,
                message = "All your base are belong to us! -- What are you looking for ESOKEÞ?"
            };
            */
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
            Destroy(spawnedObjectTransform.gameObject);
        }

        if (!_hasAnimator) return;
    }

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRpc " + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("TestClientRpc");
    }
}
    

    
