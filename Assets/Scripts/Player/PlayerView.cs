using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerView : MonoBehaviour, IPlayerView
{
    public Transform Transform => transform;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Visual Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    private Rigidbody2D _rb;
    private PlayerController _controller;
    private Vector2 _currentVelocity;
    private PlayerInteraction _playerInteraction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInteraction = GetComponent<PlayerInteraction>();

        // Dependency Injection: We inject "this" (the view) into the controller
        _controller = new PlayerController(this, _moveSpeed);

        _controller.OnLeftClick.AddListener(_playerInteraction.ChangeMask);
    }

    private void OnDestroy()
    {
        // Crucial: Clean up the pure C# class and its Input Actions
        _controller?.Dispose();
    }

    private void Update()
    {
        // Delegate logic to the pure C# controller
        _controller.Tick();
    }

    private void FixedUpdate()
    {
        // Apply physics
        _rb.linearVelocity = _currentVelocity;
    }

    #region IPlayerView Implementation

    public void Move(Vector2 velocity)
    {
        _currentVelocity = velocity;
    }

    public void UpdateVisuals(Vector2 direction, bool isMoving)
    {
        if (_animator != null)
        {
            //_animator.SetBool("IsMoving", isMoving);
            // If you have Blend Trees:
            _animator.SetFloat("InputX", direction.x);
            _animator.SetFloat("InputY", direction.y);
        }
    }

    #endregion
}