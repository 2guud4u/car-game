using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverScreen;
    public Sprite heartSprite;

    public static GameManager Instance;
    public int soulCondition = 1;
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
        soulText.text = _soul + "/" + soulCondition;
    }

    void Update()
    {
       int currTime = _levelTime - Mathf.FloorToInt(Time.timeSinceLevelLoad);
       timerText.text = "Time: " + currTime;
       if (currTime <= 0){
        GameOver();
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
            GameOver();
        }

    }

    public void ScoreUpdate()
    {
        _soul++;
        // soulText.text = "Score: " + _soul;
        soulText.text = _soul + "/" + soulCondition;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void EndLevel()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(Application.loadedLevel);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }


}
