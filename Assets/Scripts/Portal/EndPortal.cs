using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndPortal : MonoBehaviour
{
    private Vector3 portalSize = new Vector3(10f, 20f, 1f);
    private bool _open;
    private bool lightningActive;
    public Material skyboxMaterial;
    public Color skyColor;
    private Color currentColor;
    public GameObject lightning;
    private LineRenderer lineRenderer;
    public Color portalSky;
    private AudioSource _audioSource;
    public AudioClip thunderCrack;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void Start()
    {
        lightningActive = false;
        _open = false;
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
        lineRenderer = lightning.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
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
            if (!lightningActive){
                StartCoroutine(Strike());
            }
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

    IEnumerator Strike()
    {
        _audioSource.PlayOneShot(thunderCrack);
        lightningActive = true;
        lineRenderer.enabled = true;
        
        yield return new WaitForSeconds(2f); 
        
        lineRenderer.enabled = false;

        yield return new WaitForSeconds(2f); 

        lightningActive = false;
    }
}
