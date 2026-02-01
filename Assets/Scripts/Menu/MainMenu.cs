using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Obsolete]
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void StartBtn()
    {
        GameManager.Instance.OnPlayButton();
    }

    public void ExitBtn()
    {
        GameManager.Instance.OnExitButton();
    }
}
