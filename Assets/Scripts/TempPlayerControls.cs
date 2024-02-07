using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotation;
    PlayerInput _playerInput;
    Rigidbody _rb;
    void Start()
    {
    }
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
    }

    void FixedUpdate()
    {
        var speedScaled = speed * Time.deltaTime;
        var turnSpeedScaled = speedScaled * rotation;

        // movement controls
        if (Keyboard.current.aKey.isPressed){
            transform.Rotate(Vector3.down * turnSpeedScaled);
        } else if (Keyboard.current.dKey.isPressed){
            transform.Rotate(Vector3.up * turnSpeedScaled);
        }

        // rotation controls
        if (Keyboard.current.wKey.isPressed){
            transform.position += transform.forward * speedScaled;
            transform.Translate(Vector3.forward * speedScaled);
        } else if (Keyboard.current.sKey.isPressed){
            transform.position += -transform.forward * speedScaled;
            transform.Translate(-Vector3.forward * speedScaled);
        }
    }
    

}
