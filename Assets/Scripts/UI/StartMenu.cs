using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject startSceen;
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject tutorialPrompt;
    [SerializeField] GameObject story;

    bool _isPaused = false;

    public void StartGame()
    {
        tutorialPrompt.SetActive(true);
    }

    public void SelectLevel()
    {
        levelSelect.SetActive(true);
        startSceen.SetActive(false);
    }

    public void GoTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void GoLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void GoLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void GoBack()
    {
        levelSelect.SetActive(false);
        story.SetActive(false);
        startSceen.SetActive(true);
    }

    public void GoStory()
    {
        startSceen.SetActive(false);
        story.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

}
