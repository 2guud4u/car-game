using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurstOnCollide : MonoBehaviour
{
    public AudioClip collisionSound;
    public GameObject particles;
    public float volume;
    string[] collisionTags = {"Player", "MeleeSoldier"};
    int burstThreshold = 25;

    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if(gameObject == null) { return; }
        if(collision.relativeVelocity.magnitude > burstThreshold && collisionTags.Any(tag => other.CompareTag(tag)))
        {
            audioSource.PlayOneShot(collisionSound, volume);
            Rigidbody[] rbs = transform.parent.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                rb.isKinematic = false;
                rb.AddForce(collision.relativeVelocity * 20, ForceMode.Impulse);
            }
            Instantiate(particles, collision.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject other = collision.gameObject;
        Rigidbody collisionBody = collision.attachedRigidbody;
        if(collisionBody == null || gameObject == null) { return; }
        if(collisionBody.velocity.magnitude > burstThreshold && collisionTags.Any(tag => other.CompareTag(tag)))
        {
            audioSource.PlayOneShot(collisionSound, volume);
            Instantiate(particles, collision.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Rigidbody[] rbs = transform.parent.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                rb.isKinematic = false;
                rb.AddForce(collisionBody.velocity * 200, ForceMode.Impulse);
            }
        }
    }
}
