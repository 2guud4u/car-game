using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI gameOverText;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;
    public GameObject portalWarningScreen;
    public GameObject gameStart;
    public GameObject healthPrompt;
    public GameObject enemyPrompt;
    public GameObject soldier;
    public GameObject player;
    public Camera cam;
    public GameObject soul;
    public static UIManager Instance;

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

    public void MakeVisible(string item, bool visibility)
    {
        if(item == "GameOver") {
            gameOverScreen.SetActive(visibility);
        }
        else if(item == "GameWin") {
            gameWinScreen.SetActive(visibility);
        }
        else if(item == "WarningPortal") {
            portalWarningScreen.SetActive(visibility);
            Invoke("HideWarningPortal", 4f);
        }
        else if (item == "GameStart")
        {
            Time.timeScale = 0;
            gameStart.SetActive(visibility);
            StartCoroutine(HideGameStart());
        }
        else if (item == "HealthPrompt")
        {
            healthPrompt.SetActive(visibility);
            healthPrompt.transform.GetChild(2).gameObject.SetActive(false);   
        }
        else if (item == "EnemyPrompt")
        {
            enemyPrompt.SetActive(visibility);
            cam.GetComponent<CameraPan>().enabled = true;
            player.GetComponent<PlayerInput>().enabled = true;
            soldier.GetComponent<NavMeshAgent>().enabled = true;
            Invoke("HideEnemyPrompt", 4f);
        }

    }

    public void SetGameOverReason(string reason)
    {
        gameOverText.text = reason;
    }

    public void HideWarningPortal()
    {
        portalWarningScreen.SetActive(false);
    }

    public void HideHealthPrompt()
    {
        healthPrompt.transform.GetChild(1).gameObject.SetActive(false);
        healthPrompt.transform.GetChild(2).gameObject.SetActive(true);
        player.GetComponent<PlayerInput>().enabled = false;
        Invoke("Next", 1f);
    }

    public void HideEnemyPrompt()
    {
        enemyPrompt.SetActive(false);
    }

    private void Next()
    {
        soldier.SetActive(true);    
        healthPrompt.SetActive(false);     
        cam.GetComponent<CameraPan>().enabled = false;
        cam.GetComponent<CameraMover>().enabled = true;
    }

    IEnumerator HideGameStart()
    {
        yield return new WaitForSecondsRealtime(2f); // Wait for the specified delay
        Time.timeScale = 1;
        gameStart.SetActive(false);

        MakeVisible("HealthPrompt", true);
    }


}
