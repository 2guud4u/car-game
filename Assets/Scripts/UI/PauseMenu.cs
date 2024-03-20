using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;

    bool _isPaused = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        _isPaused = true;
        MusicManager.Instance._audioSource.Pause();
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
    }

    public void Resume()
    {
        _isPaused = false;
        MusicManager.Instance._audioSource.Play();
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }

    public void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void LoadStartMenu()
    {
        //if (Health.Instance != null)
        //{
        //    Destroy(Health.Instance.gameObject);
        //}

        //if (GameManager.Instance != null)
        //{
        //    Destroy(GameManager.Instance.gameObject);
        //}

        //Time.timeScale = 1f;
        //SceneManager.LoadScene("Scenes/Start");
    }
}
