using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace UnityTutorial.Manager
{
    public class InputManager : NetworkBehaviour 
    {
        [SerializeField] private PlayerInput PlayerInput;

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Run { get; private set; }
        public bool Jump { get; private set; }
        public bool LeftGrab { get; private set; }
        public bool Toss { get; private set; }
        public bool RightGrab { get; private set; }
        public bool LeftPunch { get; private set; }
        public bool RightPunch { get; private set; }
        private PlayerControlls _controls;

        private InputActionMap _currentMap;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _runAction;
        private InputAction _jumpAction;        
        private InputAction _tossAction;       
        private InputAction _leftGrab;
        private InputAction _rightGrab;
        private InputAction _leftPunch;
        private InputAction _rightPunch;

        private void Awake()
        {
            _controls = new PlayerControlls();
            _currentMap = new InputActionMap();
            _moveAction = _controls.Player.Move;
            _lookAction = _controls.Player.Look;
            _runAction = _controls.Player.Run;
            _jumpAction = _controls.Player.Jump;
            _leftGrab = _controls.Player.LeftGrab;
            _rightGrab = _controls.Player.RightGrab;
            _tossAction = _controls.Player.Toss;
            _leftPunch = _controls.Player.LeftPunch;
            _rightPunch = _controls.Player.RightPunch;
            

            


            _moveAction.performed += onMove;
            _lookAction.performed += onLook;
            _runAction.performed += onRun;
            _jumpAction.performed += onJump;
            _leftGrab.performed += onLeftGrab;
            _tossAction.performed += onToss;
            _rightGrab.performed += onRightGrab;
            _leftPunch.performed += onLeftPunch;
            _rightPunch.performed += onRightPunch;

            _moveAction.canceled += onMove;
            _lookAction.canceled += onLook;
            _jumpAction.canceled += onJump;
            _runAction.canceled += onRun;
            _rightGrab.canceled += onRightGrab;
            _tossAction.canceled += onToss;
            _leftGrab.canceled += onLeftGrab;
            _leftPunch.canceled += onLeftPunch;
            _rightPunch.canceled += onRightPunch;
            
            
        }

        private void _jumpAction_canceled(InputAction.CallbackContext obj)
        {
            throw new System.NotImplementedException();
        }
        private void onLeftPunch(InputAction.CallbackContext context) 
        {
            LeftPunch = context.ReadValueAsButton();
        }
        private void onRightPunch(InputAction.CallbackContext context)
        {
            RightPunch = context.ReadValueAsButton();
        }
        private void onMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }
        private void onLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
            
        }
        private void onRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }
        private void onJump(InputAction.CallbackContext context)
        {
            Jump = context.ReadValueAsButton();
        }
        private void onLeftGrab(InputAction.CallbackContext context)
        { 
            LeftGrab = context.ReadValueAsButton();
        }
        private void onToss(InputAction.CallbackContext context)
        {
            Toss = context.ReadValueAsButton();
        }
        private void onRightGrab(InputAction.CallbackContext context)
        {
            RightGrab = context.ReadValueAsButton();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();
        }

        private void Onisable()
        {
            _controls.Player.Disable();
        }


    }
}