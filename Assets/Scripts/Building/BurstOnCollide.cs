using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BurstOnCollide : MonoBehaviour
{
    public AudioClip collisionSound;
    public GameObject particles;
    public float volume;
    public bool hasChildren;
    string[] collisionTags = {"Player", "MeleeSoldier"};
    int burstThreshold = 20;
    float tiltThreshold = 50;

    GameObject audioSource;
    void Awake()
    {
        audioSource = GameObject.Find("CollideAudio");
    }

    private void StartFallChecks()
    {
        InvokeRepeating("CheckDidFall", Random.Range(0f, 1f), 1f);
    }

    private void CheckDidFall()
    {
        if(Mathf.Abs(transform.rotation.eulerAngles.z) > tiltThreshold || Mathf.Abs(transform.rotation.eulerAngles.x) > tiltThreshold)
        {
            DestroyObject();
        }
    }

    private void DestroyObject() {
        GameManager.Instance.addDestructionScore(5);
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
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

            if(hasChildren) { addForceToChildren(collision.relativeVelocity * 100); }

            DestroyObject();
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
            
            if(hasChildren) { addForceToChildren(collisionBody.velocity * 100); }

            DestroyObject();
        }
    }

    private void addForceToChildren(Vector3 force)
    {
        Transform[] children = transform.parent.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(force, ForceMode.Impulse);
                
                BurstOnCollide boc = child.GetComponent<BurstOnCollide>();
                boc.StartFallChecks();
            }
        }
    }
}
