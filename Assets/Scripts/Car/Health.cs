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
        }
    }

    void LiveIncrease()
    {
        currentHealth += 10;
        healthBar.SetHealth(currentHealth);
    }

    void LiveDecrease() 
    { 
        currentHealth -= 10;
        healthBar.SetHealth(currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Health"))
        {
            Destroy(other.gameObject);
            LiveIncrease();
            _audioSource.PlayOneShot(heartSound);
        }
        else if (other.CompareTag("Damage"))
        {
            Destroy(other.gameObject);
            LiveDecrease();
            _audioSource.PlayOneShot(arrowSound); 
        }
        else if (other.CompareTag("Weapon"))
        {
             LiveDecrease();
            // other.gameObject.SetActive(false);
            other.gameObject.tag = "Untagged";
            _audioSource.PlayOneShot(swordSound);
        }
        else if (other.CompareTag("BallistaBolt")){
            LiveDecrease();
            _audioSource.PlayOneShot(arrowSound);
        }
    }

}