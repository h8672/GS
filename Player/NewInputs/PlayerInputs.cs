using UnityEngine;
//using UnityEngine.Experimental.Input;

namespace GS.Player
{
    public class PlayerInputs : MonoBehaviour
    {
        private static PlayerInputs _instance;

        public static PlayerInputs Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            if ( _instance != null && _instance != this )
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /*/public InputMaster controls;

        public Transform Target { get; private set; }
        public bool InMenu { get; private set; }
        public bool Mounted { get; private set; }
        public bool Focused { get; private set; }
        public bool Moving { get; private set; }
        public bool Turning { get; private set; }
        public bool Strafing { get; private set; }
        public bool Jumping { get; private set; }
        public bool forward, turnRight, strafeRight;

        // TODO Add Velocity, TurnVelocity, Speed, TurnSpeed for carlike speed handling.
        // TODO Add Force so you can drift with the car!

        private void Menu(InputAction.CallbackContext obj)
        {
            InMenu = !InMenu;

        }

        void Forward(InputAction.CallbackContext obj)
        {
            forward = ((obj.ReadValue<float>() >= 0) ? true : false);
            Moving = !Moving;
        }
        void Strafe(InputAction.CallbackContext obj)
        {
            strafeRight = ((obj.ReadValue<float>() >= 0) ? true : false);
            Strafing = !Strafing;
        }
        void Turn(InputAction.CallbackContext obj)
        {
            turnRight = ((obj.ReadValue<float>() >= 0) ? true : false);
            Turning = !Turning;
        }
        void Jump(InputAction.CallbackContext obj)
        {
            Jumping = !Jumping;
        }

        void Focus(InputAction.CallbackContext obj)
        {
            Focused = !Focused;
        }

        void TestFire() { Debug.Log("Fire test!!!"); }

		void LateUpdate() { if( Jumping ) Jumping = false; }

        void OnEnable()
        {
            //Keyboard.current[Key.Space];
            //controls.Enable();
            //controls.Camera.Enable();
            Debug.Log("controls enabled");
            controls.Actions.MenuToggle.performed += Menu;
            controls.Camera.Focus.performed += Focus;

            controls.Character.Enable();
            controls.Character.Turn.performed += Turn;
            controls.Character.Strafe.performed += Strafe;
            controls.Character.Forward.performed += Forward;
            controls.Character.Jump.performed += Jump;
        }
        void OnDisable()
        {
            //controls.Disable();
            controls.Actions.MenuToggle.performed -= Menu;
            controls.Camera.Focus.performed -= Focus;

            controls.Character.Disable();
            controls.Character.Turn.performed -= Turn;
            controls.Character.Strafe.performed -= Strafe;
            controls.Character.Forward.performed -= Forward;
            controls.Character.Jump.performed -= Jump;
        }// */
    }
}
