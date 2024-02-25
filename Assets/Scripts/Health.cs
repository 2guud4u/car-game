using UnityEngine;

public class Health : MonoBehaviour
{

    public AudioClip swordSound;
    public AudioClip arrowSound;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Health"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.LiveIncrease();
        }
        else if (other.CompareTag("Damage"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.LiveDecrease();
            _audioSource.PlayOneShot(swordSound);
        }
        else if (other.CompareTag("Weapon"))
        {
            GameManager.Instance.LiveDecrease();
            // other.gameObject.SetActive(false);
            other.gameObject.tag = "Untagged";
            _audioSource.PlayOneShot(swordSound);
        }
        else if (other.CompareTag("BallistaBolt")){
            GameManager.Instance.LiveDecrease();
            _audioSource.PlayOneShot(arrowSound);
        }
    }

}