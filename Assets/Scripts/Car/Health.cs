using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    public static Health Instance;

    public AudioClip swordSound;
    public AudioClip arrowSound;
    public AudioClip heartSound;
    public AudioClip sparksSound;
    private AudioSource _audioSource;
    public GameObject sparks;
    public GameObject carSmoke;
    public int smokeThreshold = 30;
    public bool flag = false;
    bool isTakingWaterDamage;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth >= maxHealth)
        {
            if (!flag)
            {
                flag = true;
                UIManager.Instance.HideHealthPrompt();
            }
            currentHealth = maxHealth;

        } else if (currentHealth <= smokeThreshold){
            ParticleManager.Instance.PlayEffect(carSmoke, transform.position, 1f);
        }
    }

    public void LiveIncrease(int increase)
    {
        currentHealth += increase;
        healthBar.SetHealth(currentHealth);
        _audioSource.PlayOneShot(heartSound);
    }

    public void LiveDecrease(int decrease) 
    { 
        currentHealth -= decrease;
        healthBar.SetHealth(currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            Destroy(other.gameObject);
            LiveDecrease(10);
            _audioSource.PlayOneShot(arrowSound); 
        }
        else if (other.CompareTag("Weapon"))
        {
            LiveDecrease(10);
            // other.gameObject.SetActive(false);
            other.gameObject.tag = "Untagged";
            _audioSource.PlayOneShot(swordSound);
        }
        else if (other.CompareTag("Projectile"))
        {
            LiveDecrease(20);
            _audioSource.PlayOneShot(arrowSound);
        }
        else if (other.CompareTag("Water"))
        {
            StartCoroutine(TakeDamage());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Water"))
        {
            isTakingWaterDamage = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            if(!isTakingWaterDamage) { StartCoroutine(TakeDamage()); }
        }
    }

    private IEnumerator TakeDamage()
    {
        isTakingWaterDamage = true;

        yield return new WaitForSeconds(0.25f);
        LiveDecrease(5);
        Instantiate(sparks, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        _audioSource.PlayOneShot(sparksSound);

        isTakingWaterDamage = false;
    }

}