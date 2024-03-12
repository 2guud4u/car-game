
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class horseScript : enemyScript
{   
    float timeSwordIsDrawn = 0.7f;

    public override void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
           
            DrawSword();

            alreadyAttacked = true;
            
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        
        
    }

    public void DrawSword()
    {
        projectile.tag = "Weapon";
        // projectile.SetActive(true);
        Invoke(nameof(HideSword), timeSwordIsDrawn);
    }

    public void HideSword()
    {
        projectile.tag = "Untagged";
        // projectile.SetActive(false);
    }

    // void OnDestroy() {
    //     Destroy(projectile);
    // }
}
