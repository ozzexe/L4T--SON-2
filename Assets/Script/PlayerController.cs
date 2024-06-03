using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityTutorial.Manager;


namespace UnityTutorial.PlayerControl
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private Transform spawnedObjectPrefab;

        private Transform spawnedObjectTransform;


        public CameraController cameraController;
        [SerializeField] private float AnimBlendSpeed = 8.9f;
        [SerializeField] float rotationSpeed = 500f;
        [SerializeField] private float jumpPower;
        Quaternion targetRotation;
        private Rigidbody _playerRigidbody;
        private InputManager _inputManager;
        private Animator _animator;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;        
        

        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;
        private Vector2 _currentVelocity;       


        private void Start()
        {            
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidbody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();


            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
        }                    

        

        private void FixedUpdate()
        {
            Move();

            
           
           

            if (_inputManager.Move.x != 0 || _inputManager.Move.y != 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, cameraController.PlanarRotation,
               rotationSpeed * Time.deltaTime);

            }

           
        }

        private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
            new MyCustomData {
                _int = 56,
                _bool = true,
            }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



        public struct MyCustomData : INetworkSerializable {
            public int _int;
            public bool _bool;
            public FixedString128Bytes message;

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter { 
            
                serializer.SerializeValue(ref _int);
                serializer.SerializeValue(ref _bool);
                serializer.SerializeValue(ref message);
            }
        }


        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
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

           

            if (Input.GetKeyDown(KeyCode.T)) {                
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

            if (Input.GetKeyUp(KeyCode.Y)) {
                spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
                Destroy(spawnedObjectTransform.gameObject);            
            }

            if (!_hasAnimator) return;

            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if (_inputManager.Move == Vector2.zero) targetSpeed = 0.1f;

            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

            var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
            var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.z;

            _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
            
                
            
        }

        [ServerRpc]
        private void TestServerRpc(ServerRpcParams serverRpcParams) {
            Debug.Log("TestServerRpc " + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId);
        }

        [ClientRpc]
        private void TestClientRpc(ClientRpcParams clientRpcParams) {
            Debug.Log("TestClientRpc");
        }

    }
}

