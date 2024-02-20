using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;
    public GameObject portalWarningScreen;
    public Sprite heartSprite;

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
        soulText.text = _soul + "/" + soulCondition;
    }

    void Update()
    {
       int currTime = _levelTime - Mathf.FloorToInt(Time.timeSinceLevelLoad);
       timerText.text = "Time: " + currTime;
       if (currTime <= 0){
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
        // soulText.text = "Score: " + _soul;
        soulText.text = _soul + "/" + soulCondition;
    }

    public void GameOver(string reason)
    {
        gameOverScreen.SetActive(true);
        TextMeshProUGUI overText = GameObject.Find("OverText").GetComponent<TextMeshProUGUI>();
        overText.text = reason;

        // gameOverScreen.getChild
        Time.timeScale = 0;
    }

    public void GameWin()
    {
        gameWinScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void PortalOpened()
    {
        portalWarningScreen.SetActive(true);
        Invoke("HideWarningPortal", 4f);
    }

    public void HideWarningPortal()
    {
        portalWarningScreen.SetActive(false);
    }

    public void EndLevel()
    {
        gameOverScreen.SetActive(true);
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
