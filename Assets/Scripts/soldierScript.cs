
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class soldierScript : enemyScript
{   
    public override void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            StartCoroutine(ActivateAttack(1.0f));
                      

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        
        
    }
    IEnumerator ActivateAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Rigidbody rb = Instantiate(projectile, transform.position + transform.forward * 2, Quaternion.identity).GetComponent<Rigidbody>();
    }
}
