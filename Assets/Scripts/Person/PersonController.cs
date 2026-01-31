using UnityEngine;

public class PersonController
{
    private readonly IPersonView _view;
    private readonly PersonSettings _settings;

    // State
    private bool _isRecruited = false;
    private Transform _targetToFollow;
    private Vector2 _currentVelocity; // Helper for SmoothDamp
    private PlayerInteraction _playerInteraction;
    private PersonView _personView;
    private float _moveTimer;
    private Vector2 _center;
    private Vector2 _randomPoint;
    private bool _stayInPlace;
    private bool _isLeader;
    private LayerMask _recruitLayer;


    // Constructor injection
    public PersonController(IPersonView view, PersonSettings settings, PlayerInteraction playerInteraction, Transform leader, bool isLeader)
    {
        _view = view;
        _personView = (PersonView)view;
        _settings = settings;
        _playerInteraction = playerInteraction;
        _center = _view.CurrentPosition;
        _targetToFollow = leader;
        _isLeader = isLeader;
    }

    // Logic usually called in Update
    public void Tick()
    {
        if (!_isRecruited)
        {
            ScanForPlayer();
        }
    }

    // Physics logic usually called in FixedUpdate
    public void FixedTick()
    {
        if (_targetToFollow != null)
        {
            PerformFollowLogic();
        }
        else Patrol();
    }

    private void ScanForPlayer()
    {
        // Check surrounding area for the player
        Collider2D hit;
        if (!_isLeader && _targetToFollow == null ) hit = Physics2D.OverlapCircle(_view.CurrentPosition, _settings.DetectRange, _settings.PlayerLayer | _settings.LeaderLayer);
        else hit = Physics2D.OverlapCircle(_view.CurrentPosition, _settings.DetectRange, _settings.PlayerLayer );

        if (hit != null)
        {
            AttemptRecruit(hit.transform);
        }
    }

    private void AttemptRecruit(Transform leader)
    {
        _targetToFollow = leader;
        _view.OnRecruited(leader);

        if (leader.GetComponent<PlayerInteraction>() != null)
        {
            _isRecruited = true;
            _playerInteraction.CalculateEncounter(_personView);

        }
        else
        {
            _personView.CurrentMaskType = leader.GetComponent<IMaskHolder>().MaskType;
            _personView.SpriteRenderer.color = MaskColor.GetMaskColor(_personView.CurrentMaskType);
        }
    }

    private void PerformFollowLogic()
    {
        Vector2 targetPos = _targetToFollow.position;
        Vector2 myPos = _view.CurrentPosition;
        float distance = Vector2.Distance(myPos, targetPos);

        // Only move if outside the stopping distance to avoid jitter/crowding
        if (distance > _settings.StoppingDistance)
        {
            // Calculate the next smooth position
            Vector2 nextPos = Vector2.SmoothDamp(
                myPos,
                targetPos,
                ref _currentVelocity,
                _settings.SmoothTime,
                _settings.FollowSpeed
            );

            _view.MoveToPosition(nextPos);
        }
        else
        {
            _view.StopMovement();
        }
    }

    private void Patrol()
    {
        if (_moveTimer <= 0f)
        {
            _moveTimer = _settings.PatrolPeriod * Random.Range(0f, 1f);
            _stayInPlace = !_stayInPlace;
            _randomPoint = _center + Random.insideUnitCircle * _settings.PatrolRadius;
        }
        else
        {
            _moveTimer -= Time.fixedDeltaTime;
        }
        // Simple random patrol logic within a radius
        if (_stayInPlace) return;
        Vector2 nextPos = Vector2.SmoothDamp(
            _view.CurrentPosition,
            _randomPoint,
            ref _currentVelocity,
            _settings.SmoothTime,
            _settings.FollowSpeed / 2 // Slower speed for patrolling
        );
        _view.MoveToPosition(nextPos);

    }
}

// Simple data class to keep the constructor clean
[System.Serializable]
public class PersonSettings
{
    public float DetectRange = 2.0f;
    public float FollowSpeed = 5.0f;
    public float SmoothTime = 0.2f;
    public float StoppingDistance = 0.8f;
    public float PatrolRadius = 6.0f;
    public float PatrolPeriod = 1.5f;
    public LayerMask PlayerLayer;
    public LayerMask LeaderLayer;
}