using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Heart")
        {
            Destroy(other.gameObject);
            gameObject.GetComponent<Health>().LiveIncrease(10);
        }
        if(other.gameObject.tag == "Time")
        {
            Destroy(other.gameObject);
            GameManager.Instance.AddTime(15);
        }
        // other ideas: more base speed, free boost, higher damage, more time, etc.
    }
}
