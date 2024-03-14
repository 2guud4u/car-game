using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float min;
    public float max;
    public float distance = 100;
    void Start()
    {
               
        min=transform.position.x;
        max=transform.position.x+distance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =new Vector3(Mathf.PingPong(Time.time*2,max-min)+min, transform.position.y, transform.position.z);
    }
}
