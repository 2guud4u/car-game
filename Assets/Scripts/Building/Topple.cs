using UnityEngine;

public class Topple : MonoBehaviour
{
    public float toppleForce = 100f;
    public float toppleTorque = 100f;

    public AudioClip crashSound;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("MeleeSoldier"))
        {
            _audioSource.PlayOneShot(crashSound);
            // Debug.Log("Topple");
            Rigidbody rb = GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                Vector3 contactPoint = collision.contacts[0].point;
                Vector3 centerToContact = contactPoint - transform.position;
                rb.AddTorque(Vector3.Cross(centerToContact, Vector3.up) * toppleTorque, ForceMode.Impulse);


                Vector3 toppleDirection = transform.position - collision.transform.position;
                rb.AddForce(toppleDirection.normalized * toppleForce, ForceMode.Impulse);
            }
        }
    }
}
