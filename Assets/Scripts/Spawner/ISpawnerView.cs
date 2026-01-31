using UnityEngine;

public interface ISpawnerView
{
    // The current number of active entities spawned by this spawner
    int CurrentPopulation { get; }
    
    // Checks if a specific position is clear of obstacles (walls, water, etc.)
    bool IsPositionFree(Vector2 position, float checkRadius, LayerMask obstacleLayers);
    
    // Creates the entity and returns it (or simply executes the instantiation)
    void SpawnEntity(Vector2 position);
    
    // Helper to get the area boundaries for random generation
    Vector2 GetSpawnAreaSize();
    Vector2 GetCenterPosition();
}