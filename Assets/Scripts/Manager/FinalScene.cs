using System.Collections;
using UnityEngine;

public class FinalScene : MonoBehaviour
{
    public GameObject sceneCamera;
    public GameObject player;
    public GameObject text1;
    public GameObject text2;
    public Vector3 initialForce;
    Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<Rigidbody>().AddForce(initialForce, ForceMode.Impulse);
        cameraTransform = sceneCamera.GetComponent<Transform>();
        StartCoroutine(ChangeCameraAngle());
    }

    // Update is called once per frame
    void Update()
    {
        // cameraTransform.LookAt(player.transform);
    }

    void ChangeText()
    {
        text1.SetActive(false);
        text2.SetActive(true);
    }

    IEnumerator ChangeCameraAngle()
    {
        Vector3 c = cameraTransform.rotation.eulerAngles;
        Quaternion initialRotation = Quaternion.Euler(c.x, 245, c.z);
        Quaternion finalRotation = Quaternion.Euler(c.x, 285, c.z);

        for (float i = 0; i < 1; i += 0.004f)
        {
            cameraTransform.rotation = Quaternion.Lerp(initialRotation, finalRotation, i);;
            yield return new WaitForSeconds(0.01f);
        }
        sceneCamera.GetComponent<CameraPan>().enabled = true;
        Invoke("ChangeText", 0.2f);
    }
}
