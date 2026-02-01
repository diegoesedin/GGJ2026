using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Sprite wonSprite, lostSprite;
    [SerializeField] private Image imageResult;

    private void OnEnable()
    {
        if (GameManager.Instance == null)
            return;
        
        imageResult.sprite = GameManager.Instance.HasWon ? wonSprite : lostSprite;
    }

    [Obsolete]
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartBtn()
    {
        GameManager.Instance.OnRestartButton();
    }
}
