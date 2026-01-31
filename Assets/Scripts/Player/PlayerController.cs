using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController
{
    private const float STOP_DISTANCE = 0.1f;
    
    private readonly IPlayerView _view;
    private readonly float _speed;
    
    // Input Actions defined in code (no need for external asset file for this simple setup)
    private InputAction _clickAction;
    
    private Camera _mainCamera;
    public UnityEvent OnLeftClick = new();

    public PlayerController(IPlayerView view, float speed)
    {
        _view = view;
        _speed = speed;
        _mainCamera = Camera.main;
        InitializeInput();
    }

    private void InitializeInput()
    {
        // Click for switching mask
        _clickAction = new InputAction("SwitchMask", binding: "<Mouse>/leftButton");
        _clickAction.Enable();
        _clickAction.performed += ctx => LeftMouseClicked();
    }

    private void LeftMouseClicked()
    {
        OnLeftClick?.Invoke();
    }

    // Must be called to clean up inputs when the object is destroyed
    public void Dispose()
    {
        _clickAction?.Disable();
        _clickAction?.Dispose();
    }

    public void Tick()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;

        Vector2 currentPos = _view.Transform.position;
        Vector2 targetPos = mouseWorldPosition;
        
        float distance = Vector2.Distance(currentPos, targetPos);
        Vector2 direction = (targetPos - currentPos).normalized;

        bool shouldMove = distance > STOP_DISTANCE;
        if (shouldMove)
        {
            _view.Move(direction * _speed);
        }
        else
        {
            _view.Move(Vector2.zero);
        }
        _view.UpdateVisuals(direction, shouldMove);
    }
    /*
    [Obsolete("Using mouse")]
    private void MoveWithInput()
    {
        // 1. Read Input
        Vector2 rawInput = _moveAction.ReadValue<Vector2>();

        // 2. Process Logic (Isometric conversion)
        Vector2 moveDirection = rawInput.normalized;
        
        // 3. Send commands to View
        bool isMoving = rawInput.sqrMagnitude > 0.01f;
        
        _view.Move(moveDirection * _speed);
        _view.UpdateVisuals(moveDirection, isMoving);
    }

    [Obsolete("Using top-down")]
    private Vector2 ConvertToIsometric(Vector2 input)
    {
        if (input == Vector2.zero) return Vector2.zero;

        // Rotate -45 degrees to align with Isometric Grid
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, -45));
        var result = matrix.MultiplyPoint3x4(input);
        
        return result.normalized;
    }*/
}