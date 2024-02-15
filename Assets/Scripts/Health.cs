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
        else if (other.CompareTag("Enemy") || other.CompareTag("Damage"))
        {
            Destroy(other.gameObject); ;
            GameManager.Instance.LiveDecrease();
        }
    }

}