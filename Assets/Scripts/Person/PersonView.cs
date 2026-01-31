using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PersonView : MonoBehaviour, IPersonView
{
    [Header("Configuration")]
    [SerializeField] private PersonSettings _settings;
    
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private PersonController _controller;
    private Rigidbody2D _rb;

    #region Unity Lifecycle

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        // Initialize Controller
        _controller = new PersonController(this, _settings);
        
        // Physics Setup for Smooth Movement
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        _controller.Tick();
    }

    private void FixedUpdate()
    {
        _controller.FixedTick();
    }

    // Debugging Tool to see the detection range
    private void OnDrawGizmosSelected()
    {
        if (_settings != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _settings.DetectRange);
        }
    }

    #endregion

    #region IPersonView Implementation

    public Vector2 CurrentPosition => _rb.position;

    public void MoveToPosition(Vector2 targetPosition)
    {
        // MovePosition is better for kinematic/smooth movement than adding force
        _rb.MovePosition(targetPosition);
    }

    public void StopMovement()
    {
        _rb.linearVelocity = Vector2.zero;
    }

    public void OnRecruited()
    {
        // Visual Feedback
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = Color.green; // Or play a particle effect
        }
    }

    #endregion
}