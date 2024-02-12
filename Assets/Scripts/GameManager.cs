using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI overText;
    public Sprite heartSprite;
    public GameObject heartUI;
    public Material unblock;

    public Button button;

    public static GameManager Instance;

    int _live = 3;
    int _soul = 0;

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
       // timerText.text = "Time: " + Mathf.FloorToInt(Time.timeSinceLevelLoad);
    }

    public void LiveIncrease()
    {
        if(_live < 10)
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
                Vector3 newPosition = new Vector3(lastHeart.transform.position.x + 0.7f, lastHeart.transform.position.y, lastHeart.transform.position.z);
                GameObject newHeart = new GameObject("heart");
                SpriteRenderer renderer = newHeart.AddComponent<SpriteRenderer>();
                renderer.sprite = heartSprite;
                newHeart.transform.position = newPosition;
                newHeart.transform.localScale = lastHeart.transform.localScale;
                newHeart.transform.SetParent(heartUI.transform);
                newHeart.GetComponent<Renderer>().material = unblock;
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
        soulText.text = "Score: " + _soul;
    }

    void GameOver()
    {
        overText.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

}
