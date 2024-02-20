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
                
                        

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
            
            
        }
}
