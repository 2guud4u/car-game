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
                Vector3 directionToPlayer = player.position - transform.position;

            // Create a rotation to look at the player
                Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer)* Quaternion.Euler(0, 90, 0);
                Rigidbody rb = Instantiate(projectile, transform.position + transform.forward * 2 + Vector3.down*3f, rotationToPlayer).GetComponent<Rigidbody>();
                // rb.AddForce(transform.forward * 100f, ForceMode.Impulse);
                // rb.AddForce(transform.up * 8f, ForceMode.Impulse);
                Vector3 directionToAim = (player.position + Vector3.down * 2.5f) - rb.position;

            // Apply force in the direction of the player
                rb.AddForce(directionToAim.normalized * 1200f, ForceMode.Impulse);
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
            
            
        }
}
