using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipScript : MonoBehaviour
{
   public float speed = 2f; // Adjust this value to change the movement speed
    public float distance = 5f; // Adjust this value to change the movement distance

    private Vector3 startPosition;
    private Vector3 movementDirection;
    private bool isMovingForward = true;

    private void Start()
    {
        startPosition = transform.position;
        movementDirection = transform.forward*-1;
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        while (true)
        {
            transform.position += movementDirection * speed * Time.deltaTime;
            Debug.Log(isMovingForward);
            if (isMovingForward)
            {
                if (Vector3.Distance(transform.position, startPosition) >= distance)
                {
                    isMovingForward = false;
                    movementDirection = -movementDirection;
                }
            }
            else
            {
                Debug.Log(Vector3.Distance(transform.position, startPosition));
                if (Vector3.Distance(transform.position, startPosition) <= 5f)
                {
                    isMovingForward = true;
                    movementDirection = -movementDirection;
                }
            }

            yield return null;
        }
    }
}
