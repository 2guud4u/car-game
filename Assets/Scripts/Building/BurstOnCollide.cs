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
    int burstThreshold = 18;

    GameObject audioSource;
    void Awake()
    {
        // audioSource = GetComponent<AudioSource>();
        audioSource = GameObject.Find("CollideAudio");
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if(gameObject == null) { return; }
        if(collision.relativeVelocity.magnitude > burstThreshold && other.tag == "MeleeSoldier")
        {
            audioSource.GetComponent<AudioSource>().PlayOneShot(collisionSound, volume);
            audioSource.transform.position = collision.transform.position;

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
        if(collisionBody.velocity.magnitude > burstThreshold && other.tag == "Player")
        {
            audioSource.GetComponent<AudioSource>().PlayOneShot(collisionSound, volume);
            audioSource.transform.position = collision.transform.position;

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
