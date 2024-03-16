using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balistaScript : enemyScript
{
    // Start is called before the first frame update


    // Update is called once per frame
    public override void AttackPlayer()
        {
            //Make sure enemy doesn't move
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                Debug.Log("Attacking player");
                Vector3 directionToPlayer = player.position - transform.position;

            // Create a rotation to look at the player
                Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer)* Quaternion.Euler(0, 90, 0);
                Rigidbody rb = Instantiate(projectile, transform.position + transform.forward * 2 + Vector3.down*3f, rotationToPlayer).GetComponent<Rigidbody>();
                // rb.AddForce(transform.forward * 100f, ForceMode.Impulse);
                // rb.AddForce(transform.up * 8f, ForceMode.Impulse);
                Vector3 aimRandomness = new Vector3(Random.Range(-1f, -1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                Vector3 directionToAim = (aimRandomness +player.position + Vector3.down * 1.5f) - rb.position;

            // Apply force in the direction of the player
                rb.AddForce(directionToAim.normalized * 900f, ForceMode.Impulse);
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
            
            
        }

    public override void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Damage")
        {
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            if(GetPlayerRigidbody().velocity.magnitude > 5)
            {
                // Debug.Log("Player has entered the enemy's trigger");
                
                
                Vector3 awayDirection = transform.position - other.transform.position;

                // Normalize the direction to ensure consistent force magnitude
                awayDirection.Normalize();

                _audioSource.PlayOneShot(damageSound);
                
                BreakApart(awayDirection, GetPlayerRigidbody().velocity.magnitude);
            }
        }
    }

    public void BreakApart(Vector3 awayDirection, float force){
        agent.enabled = false;
        foreach (Rigidbody rb in GetRbs())
        {
            rb.isKinematic = false;
            rb.AddForce(awayDirection * force, ForceMode.Impulse);
        }
        
    }
}
