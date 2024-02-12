
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // ragdoll
    public Transform RagdollRoot;
    private Rigidbody[] rbs;
    public Rigidbody rb;

    public Rigidbody torsoRb;

    public GameObject enemyObj;

    //player velocity obj
    public Rigidbody playerVelocity;

    private void Awake()
    {
        player = GameObject.Find("Body").transform;
        agent = GetComponent<NavMeshAgent>();
        rbs = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        playerVelocity = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Check for sight and attack range
        //Debug.Log("Checking for sight and attack range");
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        //run if agent is enabled
        if(agent.enabled ){
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
        

    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        //Debug.Log("Chasing player");
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position + transform.forward * 2, Quaternion.identity).GetComponent<Rigidbody>();
            // rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            // rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        //turnOnRagdoll();
        
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    private void turnOnRagdoll()
    {
        agent.enabled = false;
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }
        Invoke(nameof(turnOffRagdoll), 8f);
    }
    private void turnOffRagdoll()
    {

        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
        }
        agent.enabled = true;
        Vector3 bodyLoc = rbs[0].transform.position;
        

        GameObject enemy = Instantiate(enemyObj, torsoRb.transform.position, Quaternion.identity);
        
        Destroy(gameObject);
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            //Debug.Log("Player has entered the enemy's trigger");
            Debug.Log(playerVelocity.velocity.magnitude);
            if(playerVelocity.velocity.magnitude > 5)
            {
                Debug.Log("Player has entered the enemy's trigger");
                turnOnRagdoll();
                Vector3 awayDirection = transform.position - other.transform.position;

                // Normalize the direction to ensure consistent force magnitude
                awayDirection.Normalize();
                // foreach (Rigidbody rb in rbs)
                // {
                //     rb.AddForce(awayDirection * 50, ForceMode.Impulse);
                // }
                //rb.AddForce(awayDirection * 50, ForceMode.Impulse);
                
            }
            
        }
    }

}
