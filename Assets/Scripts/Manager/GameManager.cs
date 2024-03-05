using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private bool hasCalled = false;

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
        spawners = GetComponents<enemySpawner>();
        foreach(enemySpawner spawner in spawners)
        {
            spawner.enabled = false;
        }
    }

    void Update()
    {
        int currTime = _levelTime - Mathf.FloorToInt(Time.timeSinceLevelLoad);
        UIManager.Instance.SetTimer(currTime);
        UIManager.Instance.SetSpeedometer(playerRigidbody.velocity.magnitude);
        if(currTime <= 0){
            GameOver("Time's up!");
        }else if (Health.Instance.currentHealth <= 0)
        {
            GameOver("You died!");
        }

        if(soldier == null)
        {
            foreach (enemySpawner spawner in spawners)
            {
                spawner.enabled = true;
            }
        }

        if(_soul == 1 && SceneManager.GetActiveScene().name == "Tutorial" && !hasCalled)
        {
            UIManager.Instance.MakeVisible("BoosterWarning", true);
            hasCalled = true;
        }
    }

    public void ScoreUpdate()
    {
        _soul++;
        UIManager.Instance.UpdateSoulText(_soul, soulCondition);
    }

    public void DecreaseSoul()
    {
        _soul--;
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
        UIManager.Instance.MakeVisible("GameWin", true);
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
    }


}
