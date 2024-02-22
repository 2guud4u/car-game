using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int soulCondition = 10;
    public int _levelTime = 100;

    int _live = 3;
    public int _soul = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        Time.timeScale = 1;
        UIManager.Instance.UpdateSoulText(_soul, soulCondition);
    }

    void Update()
    {
        int currTime = _levelTime - Mathf.FloorToInt(Time.timeSinceLevelLoad);
        UIManager.Instance.SetTimer(currTime);
        if(currTime <= 0){
            GameOver("Time's up!");
        }
    }

    public void LiveIncrease()
    {
        if(_live < 5)
        {
            GameObject lastHeart = null;
            float bigX = float.NegativeInfinity;

            foreach (GameObject heart in GameObject.FindGameObjectsWithTag("Heart"))
            {
                if (heart.transform.position.x > bigX)
                {
                    bigX = heart.transform.position.x;
                    lastHeart = heart;
                }
            }

            if (lastHeart != null)
            {
                Instantiate(lastHeart, new Vector3(lastHeart.transform.position.x + 102.45f, lastHeart.transform.position.y, lastHeart.transform.position.z),
                    lastHeart.transform.rotation, lastHeart.transform.parent);
                _live++;
            }
        }

    }

    public void LiveDecrease()
    {
        _live--;

        GameObject lastHeart = null;
        float bigX = float.NegativeInfinity;

        foreach (GameObject heart in GameObject.FindGameObjectsWithTag("Heart"))
        {
            if (heart.transform.position.x > bigX)
            {
                bigX = heart.transform.position.x;
                lastHeart = heart;
            }
        }

        Destroy(lastHeart);

        if (_live <= 0)
        {
            GameOver("You died!");
        }

    }

    public void ScoreUpdate()
    {
        _soul++;
        UIManager.Instance.UpdateSoulText(_soul, soulCondition);
    }

    public void GameOver(string reason)
    {
        UIManager.Instance.MakeVisible("GameOver", true);
        UIManager.Instance.SetGameOverReason(reason);
        Time.timeScale = 0;
    }

    public void GameWin()
    {
        UIManager.Instance.MakeVisible("GameWin", true);
        Time.timeScale = 0;
    }

    public void PortalOpened()
    {
        UIManager.Instance.MakeVisible("WarningPortal", true);
    }

    public void EndLevel()
    {
        UIManager.Instance.MakeVisible("GameWin", true);
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }


}
