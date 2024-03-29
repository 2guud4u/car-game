using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float FireRate=10.0f;

    private float nextFire = 0.0f;

    public GameObject projectile;

    public float powerforward = 1000.0f;

    public float powerup = 1.0f;

    private GameObject ball;
    
    public AudioSource audioSource;
    void Start()
    {
        FireRate = Random.Range(5.0f, 50.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextFire){
            Fire();
            nextFire = Time.time + FireRate;
        }
    }

    void Fire()
    {
        FireRate = Random.Range(10.0f, 30.0f);
        audioSource.Play();
        if(ball != null){
            Destroy(ball);
        }
        //Attack code here

        GameObject obj = Instantiate(projectile, transform.position + transform.forward * 2 + Vector3.up*3f, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward*-1 * powerforward, ForceMode.Impulse);
        rb.AddForce(transform.up * powerup, ForceMode.Impulse);
        ball = obj;
        
        //End of attack code
    }
}
