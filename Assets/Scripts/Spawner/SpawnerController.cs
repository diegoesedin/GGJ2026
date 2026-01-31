using UnityEngine;

public class SpawnerController
{
    private readonly ISpawnerView _view;
    private readonly SpawnerSettings _settings;

    private float _spawnTimer;

    public SpawnerController(ISpawnerView view, SpawnerSettings settings)
    {
        _view = view;
        _settings = settings;
        _spawnTimer = 0f;
    }

    public void Tick(float deltaTime)
    {
        // 1. Check Population Limit
        if (_view.CurrentPopulation >= _settings.MaxPopulation)
        {
            return; // Do nothing if full
        }

        // 2. Update Timer
        _spawnTimer += deltaTime;

        if (_spawnTimer >= _settings.SpawnInterval)
        {
            _spawnTimer = 0f;
            AttemptSpawn();
        }
    }

    private void AttemptSpawn()
    {
        // Try multiple times to find a valid spot in this frame
        // (prevents waiting another full interval just because one random point was bad)
        for (int i = 0; i < _settings.SpawnRetriesPerTick; i++)
        {
            Vector2 candidatePos = GetRandomPosition();

            if (_view.IsPositionFree(candidatePos, _settings.CollisionCheckRadius, _settings.ObstacleLayers))
            {
                _view.SpawnEntity(candidatePos);
                return; // Success! Stop trying
            }
        }
        
        // If we reach here, we failed to find a spot this time. We'll try again next interval.
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 center = _view.GetCenterPosition();
        Vector2 size = _view.GetSpawnAreaSize();

        float randomX = Random.Range(-size.x / 2, size.x / 2);
        float randomY = Random.Range(-size.y / 2, size.y / 2);

        return center + new Vector2(randomX, randomY);
    }
}

[System.Serializable]
public class SpawnerSettings
{
    [Header("Timing")]
    public float SpawnInterval = 2.0f;     // Seconds between attempts
    public int MaxPopulation = 10;         // Max active NPCs

    [Header("Placement")]
    public Vector2 SpawnAreaSize = new Vector2(10, 10);
    public int SpawnRetriesPerTick = 5;    // How many times to try per frame if position is invalid
    
    [Header("Collision Validation")]
    public float CollisionCheckRadius = 0.5f; // Size of the NPC base
    public LayerMask ObstacleLayers;       // Walls, Water, etc.
}