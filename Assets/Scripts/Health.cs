using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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
        }
        else if (other.CompareTag("Weapon"))
        {
            GameManager.Instance.LiveDecrease();
            // other.gameObject.SetActive(false);
            other.gameObject.tag = "Untagged";
        }
    }

}