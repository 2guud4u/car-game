
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class soldierScript : enemyScript
{   
    [SerializeField] float timeSwordIsDrawn;

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
            Invoke(nameof(HideSword), timeSwordIsDrawn);
        }
        
        
    }

    public void DrawSword()
    {
        projectile.tag = "Weapon";
        // projectile.SetActive(true);
        AnimateAttack();
    }

    public void HideSword()
    {
        projectile.tag = "Untagged";
        // projectile.SetActive(false);
    }

    void OnDestroy() {
        Destroy(projectile);
    }
}
