using UnityEngine;

public interface IPersonView
{
    // The Controller needs to know where the entity is in the world
    Vector2 CurrentPosition { get; }

    // Commands from Controller to View
    void MoveToPosition(Vector2 targetPosition);
    void StopMovement();
    void OnRecruited(Transform leader); // To trigger visual feedback (change color, play sound)
}