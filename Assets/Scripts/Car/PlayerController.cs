using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip collectSoul;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -15)
        {
            GameManager.Instance.GameOver("You fell!");
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "Soul")
        {
            Destroy(collision.gameObject.transform.parent.gameObject);
            collision.gameObject.transform.parent.gameObject.SetActive(false);
            GameManager.Instance.ScoreUpdate();
            _audioSource.PlayOneShot(collectSoul);
        }
    }
}
