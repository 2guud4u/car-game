using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndPortal : MonoBehaviour
{
    private Vector3 portalSize = new Vector3(10f, 20f, 1f);
    private bool _open = false;
    public Material skyboxMaterial;
    public Color skyColor;
    public Color portalSky;
    private AudioSource _audioSource;
    public AudioClip thunderCrack;
    //public LineRenderer lightning;
    //public Transform startPoint;
    //public Transform targetObject;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void Start()
    {
        skyboxMaterial = RenderSettings.skybox;
        if (skyboxMaterial != null)
        {
            if (skyboxMaterial.shader.name == "Skybox/Procedural")
            {
                //skyColor = skyboxMaterial.GetColor("_SkyTint");
            }
            else
            {
                Debug.LogWarning("The default Skybox material is not procedural.");
            }
        }
        else
        {
            Debug.LogWarning("No Skybox material assigned.");
        }
        //lightning.enabled = false;
        //StartCoroutine(strikeLightning());
    }
    
    void Update()
    {
        if (GameManager.Instance._soul >= GameManager.Instance.soulCondition && !_open){
            transform.localScale = portalSize;
            _open = true;
            GameManager.Instance.PortalOpened();
            changeSky();
        }
        if (GameManager.Instance._soul < GameManager.Instance.soulCondition){
            transform.localScale = new Vector3(0f, 0f, 0f);
            _open = false;
            changeSky();
        }
        //growth
        if(_open){
            transform.localScale = portalSize+(new Vector3(1f, 1f, 1f)*(GameManager.Instance._soul/ GameManager.Instance.soulCondition));
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (_open && collision.gameObject.tag == "Player"){
            GameManager.Instance.EndLevel();
        }
    }

    void changeSky()
    {
        if (_open){
            RenderSettings.skybox = skyboxMaterial;
            skyboxMaterial.SetColor("_SkyTint", portalSky);
        } else {
            RenderSettings.skybox = skyboxMaterial;
            skyboxMaterial.SetColor("_SkyTint", skyColor);
        }
    }

    /* IEnumerator strikeLightning()
    {
        while (true) {
            if (_open){
                Debug.Log("Lightning Strike");
                _audioSource.PlayOneShot(thunderCrack);
                lightning.SetPosition(0, startPoint.position);
                lightning.SetPosition(1, targetObject.position);
                lightning.enabled = true;
                yield return new WaitForSeconds(2f); 
                lightning.enabled = false;
                yield return new WaitForSeconds(8f); 
            }
        }
    } */
}
