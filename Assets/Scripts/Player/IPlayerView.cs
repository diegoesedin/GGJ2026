using UnityEngine;

public interface IPlayerView
{
    Transform Transform { get; }
    // The Controller doesn't care about Rigidbodies, it just sends velocity.
    void Move(Vector2 velocity);
    
    // The Controller tells the view to update its state (visuals).
    void UpdateVisuals(Vector2 direction, bool isMoving);
}