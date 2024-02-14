using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI overText;
    public Sprite heartSprite;
    public Material unblock;

    public Button button;

    public static GameManager Instance;
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
    }

    void Update()
    {
       timerText.text = "Time: " + (_levelTime - Mathf.FloorToInt(Time.timeSinceLevelLoad));
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
        soulText.text = "" + _soul;
    }

    public void GameOver()
    {
        overText.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

}
