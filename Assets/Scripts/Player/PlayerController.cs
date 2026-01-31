using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController
{
    private readonly IPlayerView _view;
    private readonly float _speed;
    
    // Input Actions defined in code (no need for external asset file for this simple setup)
    private InputAction _moveAction;

    public PlayerController(IPlayerView view, float speed)
    {
        _view = view;
        _speed = speed;
        InitializeInput();
    }

    private void InitializeInput()
    {
        // Define a 2D Vector composite binding (WASD + Arrow Keys + Gamepad Left Stick)
        _moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
        _moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        _moveAction.Enable();
    }

    // Must be called to clean up inputs when the object is destroyed
    public void Dispose()
    {
        _moveAction.Disable();
        _moveAction.Dispose();
    }

    // Called every frame by the "Owner" (The Monobehaviour)
    public void Tick()
    {
        // 1. Read Input
        Vector2 rawInput = _moveAction.ReadValue<Vector2>();

        // 2. Process Logic (Isometric conversion)
        Vector2 moveDirection = rawInput.normalized; //ConvertToIsometric(rawInput);
        
        // 3. Send commands to View
        bool isMoving = rawInput.sqrMagnitude > 0.01f;
        
        _view.Move(moveDirection * _speed);
        _view.UpdateVisuals(moveDirection, isMoving);
    }

    private Vector2 ConvertToIsometric(Vector2 input)
    {
        if (input == Vector2.zero) return Vector2.zero;

        // Rotate -45 degrees to align with Isometric Grid
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, -45));
        var result = matrix.MultiplyPoint3x4(input);
        
        return result.normalized;
    }
}