using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public GameObject soldier;
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
    enemySpawner[] spawners;
    [SerializeField] GameObject[] powerups;
    public bool _haungs;

    public int timeLeft;
    public int destructionScore = 0;
    public GameObject mainCamera;
    public GameObject portalCamera;
    bool spawnersEnabled = false;

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
        UIManager.Instance.MakeVisible("GameStart", true);
        UIManager.Instance.MakeVisible("haungsMode", false);
        spawners = GetComponents<enemySpawner>();
        foreach(enemySpawner spawner in spawners)
        {
            spawner.enabled = false;
        }
        _haungs = false;
    }

    void Update()
    {
        int currTime = _levelTime - Mathf.FloorToInt(Time.timeSinceLevelLoad);
        timeLeft = currTime;
        UIManager.Instance.SetTimer(currTime);
        UIManager.Instance.SetSpeedometer(playerRigidbody.velocity.magnitude);
        if(currTime <= 0){
            GameOver("Time's up!");
        }else if (Health.Instance.currentHealth <= 0)
        {
            GameOver("You died!");
        }

        if(!spawnersEnabled && soldier == null)
        {
            foreach (enemySpawner spawner in spawners)
            {
                spawner.enabled = true;
            }
            spawnersEnabled = true;
        }

        if(_soul == 1 && SceneManager.GetActiveScene().name == "Tutorial" && UIManager.Instance.tutorialStep == 3)
        {
            UIManager.Instance.MakeVisible("BoosterPrompt", true);
        }

        if (Keyboard.current.hKey.isPressed && !_haungs){
            _haungs = true;
            UIManager.Instance.MakeVisible("haungsMode", true);
        }

        if (_haungs){
            // add time 
            if (Keyboard.current.tKey.isPressed){
                AddTime(10);
            }
            // add health
            if (Keyboard.current.hKey.isPressed){
                Health.Instance.LiveIncrease(10);
            }
            // add soul
            if (Keyboard.current.yKey.isPressed){
                ScoreUpdate();
            }
        }
    }

    public void addDestructionScore(int score)
    {
        destructionScore += score;
    }
    public void ScoreUpdate()
    {
        _soul++;
        UIManager.Instance.UpdateSoulText(_soul, soulCondition);
    }

    public void AddTime(int increment)
    {
        _levelTime += increment;
    }

    public void DecreaseSoul()
    {
        _soul--;
        UIManager.Instance.UpdateSoulText(_soul, soulCondition);
    }
    
    public void GameOver(string reason)
    {
        if (_haungs){
            UIManager.Instance.MakeVisible("haungsMode", false);
        }
        UIManager.Instance.MakeVisible("GameOver", true);
        UIManager.Instance.SetGameOverReason(reason);
        Time.timeScale = 0;
        // this isn't working for some reason
        //MusicManager.Instance.StopAudio();
        //_audioSource.PlayOneShot(winSound);
        UIManager.Instance.UpdateScoreBoard(_soul, timeLeft, destructionScore);

    }

    public void GameWin()
    {
        if (_haungs){
            UIManager.Instance.MakeVisible("haungsMode", false);
        }
        UIManager.Instance.MakeVisible("GameWin", true);
        Time.timeScale = 0;
        MusicManager.Instance.StopAudio();
        StopAudio();
        _audioSource.PlayOneShot(winSound);
        UIManager.Instance.UpdateScoreBoard(_soul, timeLeft, destructionScore);
    }

    public void PortalOpened()
    {
        // UIManager.Instance.MakeVisible("WarningPortal", true);
        if(portalCamera != null)
        {
            SwitchCamera();
            Invoke("SwitchCamera", 2f);
        }
    }

    public void EndLevel()
    {
        if (_haungs){
            UIManager.Instance.MakeVisible("haungsMode", false);
        }
        UIManager.Instance.UpdateScoreBoard(_soul, timeLeft, destructionScore);

        UIManager.Instance.MakeVisible("scoreBoard", true);

        UIManager.Instance.MakeVisible("GameWin", true);
        StopAudio();
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void StopAudio()
    {
        if (engine != null){
            engine.Stop();
        } 
        if (drift != null){
            drift.Stop();
        }
        MusicManager.Instance.StopAudio();
    }

    public (GameObject, float)[] GetPowers()
    {
        (GameObject, float) healthPower = (powerups[0], 0.05f);
        (GameObject, float) timePower = (powerups[1], 0.05f);
        (GameObject, float)[] powerUps = {healthPower, timePower};
        return powerUps;
    }
    
    void SwitchCamera()
    {
        mainCamera.SetActive(!mainCamera.activeSelf);
        portalCamera.SetActive(!portalCamera.activeSelf);
    }

}
