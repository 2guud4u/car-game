using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScene : MonoBehaviour
{
    public GameObject sceneCamera;
    public GameObject player;
    public GameObject text1;
    public GameObject text2;
    public GameObject winScreen;
    public Vector3 initialForce;
    Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Invoke("StartFinalScene", 1f);
    }

    void StartFinalScene()
    {
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Rigidbody>().AddForce(initialForce, ForceMode.Impulse);
        cameraTransform = sceneCamera.GetComponent<Transform>();
        StartCoroutine(ChangeCameraAngle());
    }

    void ChangeText()
    {
        text1.SetActive(false);
        text2.SetActive(true);
        Invoke("ChangeToWin", 9f);
    }

    void ChangeToWin()
    {
        text2.SetActive(false);
        winScreen.SetActive(true);
    }

    IEnumerator ChangeCameraAngle()
    {
        Vector3 c = cameraTransform.rotation.eulerAngles;
        Quaternion initialRotation = Quaternion.Euler(c.x, 245, c.z);
        Quaternion finalRotation = Quaternion.Euler(c.x, 285, c.z);

        for (float i = 0; i < 1; i += 0.004f)
        {
            cameraTransform.rotation = Quaternion.Lerp(initialRotation, finalRotation, i);
            yield return new WaitForSeconds(0.01f);
        }
        sceneCamera.GetComponent<CameraPan>().enabled = true;
        Invoke("ChangeText", 0.2f);
    }

    public void LoadStartMenu()
    {
        print("Restart Game");
        SceneManager.LoadScene("StartMenu");
    }
}
