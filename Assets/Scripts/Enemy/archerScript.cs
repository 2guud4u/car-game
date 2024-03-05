
using UnityEngine;
using UnityEngine.AI;

public class archerScript : enemyScript
{
    public override void AttackPlayer()
    {
        //Make sure enemy doesn't move
        if(agent.enabled) { agent.SetDestination(transform.position); }

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            AnimateAttack();
            ShootArrow();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        //turnOnRagdoll();
        
    }

    void ShootArrow()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer)* Quaternion.Euler(90, 0, 0);

        Vector3 attackRandomness = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        ///Attack code here
        Rigidbody rb = Instantiate(projectile, transform.position + transform.forward * 2 + Vector3.up*3f, rotationToPlayer).GetComponent<Rigidbody>();
        rb.AddForce(attackRandomness+transform.forward * 32f, ForceMode.Impulse);
        rb.AddForce(attackRandomness+transform.up * 8f, ForceMode.Impulse);
        ///End of attack code
    }
}
