using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{

    // [SerializeField] float lifeTime = 4f;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     Destroy(gameObject, lifeTime);
    // }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
