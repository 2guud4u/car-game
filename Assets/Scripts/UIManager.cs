using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI gameOverText;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;
    public GameObject portalWarningScreen;
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
    }

    public void SetGameOverReason(string reason)
    {
        gameOverText.text = reason;
    }

    public void HideWarningPortal()
    {
        portalWarningScreen.SetActive(false);
    }
}
