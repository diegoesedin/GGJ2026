using UnityEngine;

public class PersonController
{
    private readonly IPersonView _view;
    private readonly PersonSettings _settings;

    // State
    private bool _isRecruited = false;
    private Transform _targetToFollow;
    private Vector2 _currentVelocity; // Helper for SmoothDamp

    // Constructor injection
    public PersonController(IPersonView view, PersonSettings settings)
    {
        _view = view;
        _settings = settings;
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
        if (_isRecruited && _targetToFollow != null)
        {
            PerformFollowLogic();
        }
    }

    private void ScanForPlayer()
    {
        // Check surrounding area for the player
        Collider2D hit = Physics2D.OverlapCircle(_view.CurrentPosition, _settings.DetectRange, _settings.PlayerLayer);

        if (hit != null)
        {
            Recruit(hit.transform);
        }
    }

    private void Recruit(Transform leader)
    {
        _isRecruited = true;
        _targetToFollow = leader;
        _view.OnRecruited();
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
}

// Simple data class to keep the constructor clean
[System.Serializable]
public class PersonSettings
{
    public float DetectRange = 2.0f;
    public float FollowSpeed = 5.0f;
    public float SmoothTime = 0.2f;
    public float StoppingDistance = 0.8f;
    public LayerMask PlayerLayer;
}