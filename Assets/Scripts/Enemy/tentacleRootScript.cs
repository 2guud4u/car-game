using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentacleRootScript : MonoBehaviour
{
    int rotationSpeed;
    Quaternion originalRotation;
    Quaternion startRotation;
    Quaternion endRotation;
    float rotationProgress = -1;

    int attackTime;

    float timeFromLastAttack;
    void StartRotating(float xPosition){

        // Here we cache the starting and target rotations
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(xPosition, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // This starts the rotation, but you can use a boolean flag if it's clearer for you
        rotationProgress = 0;
        rotationSpeed = 3;
    }

    void Start()
    {
        timeFromLastAttack = 0;
        attackTime = Random.Range(2, 10);
        originalRotation = transform.rotation;

    }
    // Update is called once per frame
    void Update()
    {
        timeFromLastAttack += Time.deltaTime;
        if(timeFromLastAttack >= attackTime){
            StartRotating(90);
            timeFromLastAttack = 0;
            attackTime = Random.Range(0, 10);
            Invoke("ResetRotation", 2);
        }
        if (rotationProgress < 1 && rotationProgress >= 0){
        rotationProgress += Time.deltaTime * rotationSpeed;

        // Here we assign the interpolated rotation to transform.rotation
        // It will range from startRotation (rotationProgress == 0) to endRotation (rotationProgress >= 1)
        transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
    }
    }

    void ResetRotation(){
        startRotation = transform.rotation;
        endRotation = originalRotation;

        // This starts the rotation, but you can use a boolean flag if it's clearer for you
        rotationProgress = 0;
        rotationSpeed = 1;
    }
}
