using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNear : MonoBehaviour
{
    [SerializeField] string objTag;
    [SerializeField] float radius;
    [SerializeField] float speed;

    GameObject objectToFollow;

    void Awake()
    {
        objectToFollow = GameObject.Find(objTag);
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, objectToFollow.transform.position) < radius) {
            transform.position = Vector3.MoveTowards(transform.position, objectToFollow.transform.position, speed * Time.deltaTime);
            transform.up = objectToFollow.transform.position - transform.position;
        }
    }
}
