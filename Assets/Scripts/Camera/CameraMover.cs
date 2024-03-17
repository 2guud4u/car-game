using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMover : MonoBehaviour
{
    public Transform end;
    private Vector3 startPosition;
    private Vector3 position;
    private Vector3 endPosition;
    private Quaternion startRotation;
    private Quaternion endRotation;
    public float speed = 1.0f;
    public bool stage = false;

    private float startTime;
    private float journeyLength;
    public static CameraMover Instance;

    private float waitTime = 0f;
    private bool isWaiting = false;
    private bool hasTraveled = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Record start time
        startTime = Time.time;
        startPosition = transform.position;
        position = transform.position;
        endPosition = end.position;
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(10, 0, 0); // Set the end rotation to 0
        // Calculate the journey length
        journeyLength = Vector3.Distance(startPosition, endPosition);
    }

    private void Update()
    {
        if (isWaiting)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0f)
            {
                isWaiting = false;
                // Swap the start and end positions and rotations
                Vector3 tempPos = startPosition;
                Quaternion tempRot = startRotation;
                startPosition = endPosition;
                startRotation = endRotation;
                endPosition = tempPos;
                endRotation = tempRot;

                // Reset the start time and journey length
                startTime = Time.time;
                journeyLength = Vector3.Distance(startPosition, endPosition);
            }
        }
        else
        {
            // Calculate the fraction of the journey completed
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position and rotation as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, fractionOfJourney);

            // If the camera has reached the end position
            if (transform.position == endPosition)
            {
                UIManager.Instance.MakeVisible("EnemyPrompt1", true);

                isWaiting = true;
                hasTraveled = true;
                waitTime = 3.5f;
            }
        }

        if (transform.position == position && hasTraveled)
        {
            UIManager.Instance.MakeVisible("EnemyPrompt2", true);
            this.enabled = false;
        }
    }
}
