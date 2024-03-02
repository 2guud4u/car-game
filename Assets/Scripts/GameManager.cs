using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics.Tracing;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public int soulCondition = 10;
    public int _levelTime;

    public int _soul = 0;
    public string nextLevel;

    Rigidbody playerRigidbody;

    public AudioClip winSound;
    public AudioClip loseSound;
    private AudioSource _audioSource;

    public AudioSource engine;
    public AudioSource drift;
    public GameObject damagedCar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody>();
        Time.timeScale = 1;
        UIManager.Instance.UpdateSoulText(_soul, soulCondition);
    }

    void Update()
    {
        int currTime = _levelTime - Mathf.FloorToInt(Time.timeSinceLevelLoad);
        UIManager.Instance.SetTimer(currTime);
        UIManager.Instance.SetSpeedometer(playerRigidbody.velocity.magnitude);
        if(currTime <= 0){
            GameOver("Time's up!");
        } else if (Health.Instance.currentHealth <= 50) {
            ParticleManager.Instance.PlayEffect(damagedCar, player.transform.position, 1f);
        } else if (Health.Instance.currentHealth <= 0)
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
        // this isn't working for some reason
        //MusicManager.Instance.StopAudio();
        //_audioSource.PlayOneShot(winSound);
    }

    public void GameWin()
    {
        UIManager.Instance.MakeVisible("GameWin", true);
        Time.timeScale = 0;
        MusicManager.Instance.StopAudio();
        StopAudio();
        _audioSource.PlayOneShot(winSound);
    }

    public void PortalOpened()
    {
        UIManager.Instance.MakeVisible("WarningPortal", true);
    }

    public void EndLevel()
    {
        if(nextLevel == "GameWin")
        {
            GameWin();
        }
        else
        {
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void StopAudio()
    {
        if (engine != null){
            engine.Stop();
        } 
        if (drift != null){
            drift.Stop();
        }
    }


}
