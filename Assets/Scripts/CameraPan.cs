using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {

	public Transform carTransform;
	[Range(1, 50)]
	public float panSpeed;
	[Range(1, 50)]
	public float followSpeed;

	Vector3 offset;
	public Vector3 lookOffset;

	void Start(){
		offset = carTransform.position - transform.position;
	}

	void FixedUpdate()
	{
        var newRotation = Quaternion.LookRotation(carTransform.position - transform.position + lookOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, panSpeed * Time.deltaTime);

        // Move
        Vector3 newPosition = carTransform.position - carTransform.forward * offset.z - carTransform.up * offset.y;
        transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed * Time.deltaTime);

	}

}
