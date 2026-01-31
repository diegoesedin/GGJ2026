using UnityEngine;

public class SpawnerView : MonoBehaviour, ISpawnerView
{
    [Header("Configuration")]
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private SpawnerSettings _settings;

    private SpawnerController _controller;

    private void Awake()
    {
        _controller = new SpawnerController(this, _settings);
    }

    private void Update()
    {
        _controller.Tick(Time.deltaTime);
    }

    #region ISpawnerView Implementation

    // Simple way to track population: Count children transform
    // Note: When an NPC is recruited, you might want to un-parent it so the spawner spawns a new one.
    public int CurrentPopulation => transform.childCount;

    public bool IsPositionFree(Vector2 position, float checkRadius, LayerMask obstacleLayers)
    {
        // Check if there is any collider in the given radius
        Collider2D hit = Physics2D.OverlapCircle(position, checkRadius, obstacleLayers);
        
        // If hit is null, the space is free
        return hit == null;
    }

    public void SpawnEntity(Vector2 position)
    {
        if (_prefabToSpawn == null) return;

        // Instantiate as a child of this spawner to track population automatically
        Instantiate(_prefabToSpawn, position, Quaternion.identity, this.transform);
    }

    public Vector2 GetSpawnAreaSize() => _settings.SpawnAreaSize;
    
    public Vector2 GetCenterPosition() => transform.position;

    #endregion

    #region Debugging

    private void OnDrawGizmos()
    {
        // Visualize the spawn area in the editor
        Gizmos.color = new Color(0, 1, 0, 0.3f); // Semi-transparent green
        Gizmos.DrawCube(transform.position, new Vector3(_settings.SpawnAreaSize.x, _settings.SpawnAreaSize.y, 1));
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(_settings.SpawnAreaSize.x, _settings.SpawnAreaSize.y, 1));
    }

    #endregion
}