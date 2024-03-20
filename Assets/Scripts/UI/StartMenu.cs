using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject startSceen;
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject tutorialPrompt;
    [SerializeField] GameObject story;

    bool _isActive = false;

    private void Update()
    {
        if (_isActive)
        {
            if(Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                GoBack();

            }
        }
    }

    public void StartGame()
    {
        tutorialPrompt.SetActive(true);
        _isActive = true;
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button btn in buttons)
        {
            if(btn.name != "Yes" && btn.name != "No")
            {
                
                btn.interactable = false;
            }
            
        }
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
        tutorialPrompt.SetActive(false);
        levelSelect.SetActive(false);
        story.SetActive(false);
        startSceen.SetActive(true);
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            btn.interactable = true;
        }
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
