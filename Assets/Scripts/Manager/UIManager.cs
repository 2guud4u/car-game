using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI gameOverText;

    public TextMeshProUGUI scoreBoardText;

    public GameObject scoreBoard;
    public GameObject BoosterWarning;
    public GameObject BoosterPrompt;
    public GameObject GoalWarning;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;
    public GameObject gameStart;
    public GameObject healthPrompt;
    public GameObject enemyPrompt;
    public GameObject haungsPrompt;
    public GameObject soldier;
    public GameObject player;
    public Camera cam;
    public GameObject soul;
    public static UIManager Instance;
    public int tutorialStep = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        timerText.enabled = true;
        soulText.enabled = true;
        speedText.enabled = true;
        soul.SetActive(true);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Tutorial" && Keyboard.current.shiftKey.wasPressedThisFrame){
            turnOffBoosterWarning();
            Invoke("ShowGoalWarning", 2f);
        }
    }

    public void SetTimer(int time)
    {
        timerText.text = "Time Left: " + time;
    }

    public void SetSpeedometer(float speed)
    {
        speedText.text = (int) speed + " MPH";
    }
    
    public void UpdateSoulText(int currentSouls, int soulCondition)
    {
        soulText.text = currentSouls + "/" + soulCondition;
    }

    public void UpdateScoreBoard(int souls, int time, int destructionScore)
    {
        int totalScore = souls*10 + time*5+ destructionScore;
        scoreBoardText.text = "FINAL SCORE" +
        "\nSouls Collected: " + souls.ToString() 
        + "\nTime to spare: "+ time 
        + "\n Destruction Score: " + destructionScore
        + "\n Total Score:" + totalScore;
    }
    public void MakeVisible(string item, bool visibility)
    {
        if(item == "GameOver") {
            gameOverScreen.SetActive(visibility);
        }
        else if(item == "GameWin") {
            gameWinScreen.SetActive(visibility);
        }
        else if (item == "GameStart" && tutorialStep == 0 && gameStart != null)
        {
            // Time.timeScale = 0;
            gameStart.SetActive(visibility);
            StartCoroutine(HideGameStart());
        }
        else if (item == "HealthPrompt" && tutorialStep == 1 && healthPrompt != null)
        {
            healthPrompt.SetActive(visibility);
        }
        else if (item == "EnemyPrompt1" && tutorialStep == 2 && enemyPrompt != null)
        {
            enemyPrompt.SetActive(visibility);
            Invoke("HideEnemyPrompt", 3.5f);
        }
        else if (item == "EnemyPrompt2" && enemyPrompt != null)
        {
            enemyPrompt.SetActive(visibility);
            enemyPrompt.transform.GetChild(0).gameObject.SetActive(false);
            enemyPrompt.transform.GetChild(2).gameObject.SetActive(false);
            enemyPrompt.transform.GetChild(1).gameObject.SetActive(true);
            enemyPrompt.transform.GetChild(3).gameObject.SetActive(true);
            cam.GetComponent<CameraPan>().enabled = true;
            player.GetComponent<PlayerInput>().enabled = true;
            soldier.GetComponent<NavMeshAgent>().enabled = true;
            Invoke("HideEnemyPrompt", 4f);
            tutorialStep--;
        }
        else if(item == "BoosterPrompt" && tutorialStep == 3)
        {
            BoosterPrompt.SetActive(visibility);
        }
        else if (item == "BoosterWarning")
        {
            BoosterWarning.SetActive(visibility);
            Invoke("turnOffBoosterWarning", 2f);
        }
        else if(item == "GoalWarning" && tutorialStep == 4){
            Debug.Log("GoalWarning");
            GoalWarning.SetActive(visibility);
            Invoke("HideGoalWarning", 6f);
            tutorialStep++;
        }
        else if(item == "scoreBoard"){
            scoreBoard.SetActive(visibility);
        }
        else if (item == "haungsMode"){
            haungsPrompt.SetActive(visibility);
        }
    }
    public void turnOffBoosterWarning()
    {
        BoosterWarning.SetActive(false);
    }    
    
    public void turnOffBoosterPrompt()
    {
        BoosterPrompt.SetActive(false);
        if(tutorialStep == 3) { tutorialStep++; }
    }

    public void ShowGoalWarning()
    {
        MakeVisible("GoalWarning", true);
    }

    public void HideGoalWarning()
    {
        GoalWarning.SetActive(false);
    }

    public void SetGameOverReason(string reason)
    {
        gameOverText.text = reason;
    }

    public void HideHealthPrompt()
    {
        healthPrompt.transform.GetChild(0).gameObject.SetActive(false);
        healthPrompt.transform.GetChild(2).gameObject.SetActive(false);
        healthPrompt.transform.GetChild(1).gameObject.SetActive(true);
        healthPrompt.transform.GetChild(3).gameObject.SetActive(true);
        player.GetComponent<PlayerInput>().enabled = false;
        Invoke("Next", 2f);
    }

    public void HideEnemyPrompt()
    {
        enemyPrompt.SetActive(false);
        tutorialStep++;
    }

    private void Next()
    {
        soldier.SetActive(true);    
        healthPrompt.SetActive(false);     
        cam.GetComponent<CameraPan>().enabled = false;
        cam.GetComponent<CameraMover>().enabled = true;
        tutorialStep++;
    }

    IEnumerator HideGameStart()
    {
        yield return new WaitForSecondsRealtime(5f); // Wait for the specified delay
        Time.timeScale = 1;
        gameStart.SetActive(false);

        tutorialStep++;
        MakeVisible("HealthPrompt", true);
    }


}
