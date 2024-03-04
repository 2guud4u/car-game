using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioClip collisionSound;
    public float volume;
    [SerializeField] string[] collisionTags;

    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if(collision.relativeVelocity.magnitude > 10 && collisionTags.Any(tag => other.CompareTag(tag)))
        {
            audioSource.PlayOneShot(collisionSound, volume);
        }
    }
}
