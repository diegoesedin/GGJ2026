using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PersonView : MonoBehaviour, IPersonView, IMaskHolder
{
    [Header("Configuration")]
    [SerializeField] private PersonSettings _settings;
    [SerializeField] private Transform _target;
    public bool IsLeader;
    
    [Header("Visuals")]
    public SpriteRenderer SpriteRenderer;
    [SerializeField] private Animator _animator;

    public PersonController Controller;
    private Rigidbody2D _rb;
    public MaskType CurrentMaskType;
    public bool IsMaskless;
    public int FollowerNumber;

    #region Unity Lifecycle

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        // Initialize Controller
        Controller = new PersonController(this, _settings, FindAnyObjectByType<PlayerInteraction>(), _target, IsLeader);
        
        // Physics Setup for Smooth Movement
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        Controller.Tick();
    }

    private void FixedUpdate()
    {
        Controller.FixedTick();
    }

    // Debugging Tool to see the detection range
    private void OnDrawGizmosSelected()
    {
        if (_settings != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _settings.InteractRange);
        }
    }



    #endregion

    #region IPersonView Implementation

    public Vector2 CurrentPosition => _rb.position;

    public MaskType MaskType => CurrentMaskType;

    public void MoveToPosition(Vector2 targetPosition, Vector2 currentVelocity)
    {
        // MovePosition is better for kinematic/smooth movement than adding force
        _rb.MovePosition(targetPosition);
        
        if (_animator != null)
        {
            //_animator.SetBool("IsMoving", isMoving);
            // If you have Blend Trees:
            _animator.SetFloat("InputX", currentVelocity.normalized.x);
            _animator.SetFloat("InputY", currentVelocity.normalized.y);
        }
    }

    public void StopMovement()
    {
        _rb.linearVelocity = Vector2.zero;
    }

    public void OnRecruited(Transform leader)
    {
        // Visual Feedback
        if (SpriteRenderer != null)
        {
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 0.5f);
        }
        
        transform.SetParent(leader);
        
        AudioManager.Instance.PlayRecruitSound();
    }

    #endregion
}