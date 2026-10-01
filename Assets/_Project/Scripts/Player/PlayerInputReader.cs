using UnityEngine;
using UnityEngine.InputSystem;

namespace Thaka.Platformer.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        InputAction moveAction;
        InputAction jumpAction;

        public float Move => moveAction.ReadValue<float>();
        public bool JumpPressed => jumpAction.WasPressedThisFrame();
        public bool JumpHeld => jumpAction.IsPressed();

        void Awake()
        {
            moveAction = new InputAction("Move", InputActionType.Value);
            moveAction.AddCompositeBinding("1DAxis")
                .With("Negative", "<Keyboard>/a")
                .With("Positive", "<Keyboard>/d");
            moveAction.AddCompositeBinding("1DAxis")
                .With("Negative", "<Keyboard>/leftArrow")
                .With("Positive", "<Keyboard>/rightArrow");
            moveAction.AddBinding("<Gamepad>/leftStick/x");
            moveAction.AddBinding("<Gamepad>/dpad/x");

            jumpAction = new InputAction("Jump", InputActionType.Button);
            jumpAction.AddBinding("<Keyboard>/space");
            jumpAction.AddBinding("<Keyboard>/w");
            jumpAction.AddBinding("<Keyboard>/upArrow");
            jumpAction.AddBinding("<Gamepad>/buttonSouth");
        }

        void OnEnable()
        {
            moveAction.Enable();
            jumpAction.Enable();
        }

        void OnDisable()
        {
            moveAction.Disable();
            jumpAction.Disable();
        }

        void OnDestroy()
        {
            moveAction.Dispose();
            jumpAction.Dispose();
        }
    }
}
