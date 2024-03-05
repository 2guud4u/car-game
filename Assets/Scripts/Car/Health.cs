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
    private AudioSource _audioSource;
    public GameObject carSmoke;
    public int smokeThreshold = 30;
    public bool flag = false;

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
        if(currentHealth == maxHealth && !flag)
        {
            flag = true;
            UIManager.Instance.HideHealthPrompt();
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

    void LiveDecrease(int decrease) 
    { 
        currentHealth -= decrease;
        healthBar.SetHealth(currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            Destroy(other.gameObject);
            LiveDecrease(5);
            _audioSource.PlayOneShot(arrowSound); 
        }
        else if (other.CompareTag("Weapon"))
        {
            LiveDecrease(5);
            // other.gameObject.SetActive(false);
            other.gameObject.tag = "Untagged";
            _audioSource.PlayOneShot(swordSound);
        }
        else if (other.CompareTag("Projectile")){
            LiveDecrease(5);
            _audioSource.PlayOneShot(arrowSound);
        }
    }

}