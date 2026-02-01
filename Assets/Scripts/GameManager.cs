using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _gameHUDCanvas;
    [SerializeField] private GameObject _gameOverCanvas;

    [Header("Game References")]
    [SerializeField] private PlayerView _playerScript;
    [SerializeField] private SpawnerView _spawnerScript;
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (_playerScript == null) _playerScript = FindFirstObjectByType<PlayerView>();
        if (_spawnerScript == null) _spawnerScript = FindFirstObjectByType<SpawnerView>();
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
        }
    }

    // States

    private void HandleMainMenu()
    {
        _mainMenuCanvas.SetActive(true);
        _gameHUDCanvas.SetActive(false);
        _gameOverCanvas.SetActive(false);

        SetGameActive(false);
    }

    private void HandlePlaying()
    {
        _mainMenuCanvas.SetActive(false);
        _gameHUDCanvas.SetActive(true);
        _gameOverCanvas.SetActive(false);

        SetGameActive(true);
        
        if(AudioManager.Instance != null) 
            AudioManager.Instance.PlayMenuClick();
    }

    private void HandleGameOver()
    {
        _mainMenuCanvas.SetActive(false);
        _gameHUDCanvas.SetActive(false);
        _gameOverCanvas.SetActive(true);

        SetGameActive(false);
    }

    private void SetGameActive(bool isActive)
    {
        if (_playerScript != null) 
            _playerScript.enabled = isActive;

        if (_spawnerScript != null) 
            _spawnerScript.enabled = isActive;
            
        Time.timeScale = isActive ? 1 : 0;
    }
    
    // UI BUTTONS

    public void OnPlayButton()
    {
        ChangeState(GameState.Playing);
        AudioManager.Instance.PlayMenuClick();
    }

    public void OnRestartButton()
    {
        AudioManager.Instance.PlayMenuClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitButton()
    {
        AudioManager.Instance.PlayMenuClick();
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }
    
    public void TriggerGameOver()
    {
        ChangeState(GameState.GameOver);
    }
}