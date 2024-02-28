using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource _audioSource;
    bool _isPlayingLowVolume = false;
    float _startingVolume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _startingVolume = _audioSource.volume;
        if (_audioSource.isPlaying == false){
            _audioSource.Play();
        }
        if (Instance == null){
            Instance = this;
        }
    }

    private void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame && _isPlayingLowVolume){
            _audioSource.volume = 0.1f;
            _isPlayingLowVolume = true;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3);
        _isPlayingLowVolume = false;
        _audioSource.volume = _startingVolume;
    }

    public void StopAudio()
    {
        _audioSource.Stop();
    }
}
