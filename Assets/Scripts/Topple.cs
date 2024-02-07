using UnityEngine;

public class Topple : MonoBehaviour
{
    public float toppleForce = 100f;
    public float toppleTorque = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Topple");
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
