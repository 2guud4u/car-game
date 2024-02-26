using UnityEngine;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    public static Health Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {

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
        }
        else if (other.CompareTag("Damage"))
        {
            Destroy(other.gameObject);
            LiveDecrease();
        }
        else if (other.CompareTag("Weapon"))
        {
             LiveDecrease();
            // other.gameObject.SetActive(false);
            other.gameObject.tag = "Untagged";
        }
        else if (other.CompareTag("BallistaBolt")){
            LiveDecrease();
            
        }
    }

}