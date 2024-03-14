using UnityEngine;

public class bulletScript : MonoBehaviour 
{
	public float speed = 0.001f;
    public float maxOffset;
    Vector3 direction;

    void Start()
    {
        Vector3 targetPos = GameObject.Find("Body").transform.position;
        Vector3 offsetPos = new(targetPos.x + Random.Range(-maxOffset, maxOffset),
                                targetPos.y + Random.Range(-maxOffset, maxOffset),
                                targetPos.z + Random.Range(-maxOffset, maxOffset));
        direction = (offsetPos - transform.position).normalized;
        // direction = Vector3.MoveTowards(transform.position, target.position, step)
    }

    void Update()
    {
        transform.position = transform.position + direction * speed * Time.deltaTime;
    }
}