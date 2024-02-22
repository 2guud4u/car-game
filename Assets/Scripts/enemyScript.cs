
using UnityEngine;
using UnityEngine.AI;

public class enemyScript : MonoBehaviour
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
    public float timeBeforeFirstAttack;
    public float timeBetweenAttacks;
    public bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // ragdoll
    public Transform RagdollRoot;
    private Rigidbody[] rbs;
    private Rigidbody rb;

    public Rigidbody torsoRb;

    public GameObject enemyObj;

    //player velocity obj
    private Rigidbody playerVelocity;
    public GameObject soulPrefab;
    
    bool firstAttack = true;
    bool soulDropped = false;

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
            if (!playerInAttackRange) {
                if(!playerInSightRange) {
                    firstAttack = true;
                    Patroling();
                }
                else { ChasePlayer(); }
            }
            else if(playerInAttackRange && playerInSightRange) {
                if(firstAttack) {
                    Invoke("ResetAttack", timeBeforeFirstAttack);
                    alreadyAttacked = true;
                    firstAttack = false;
                }
                else { AttackPlayer(); }
            }
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

    public virtual void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            ///Attack code here

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

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
        if(health > 0){
            GameObject enemy = Instantiate(enemyObj, torsoRb.transform.position, Quaternion.identity);
            enemy.GetComponentInChildren<enemyScript>().setHealth(health);
        }
        Destroy(gameObject);
    }
    
    private void DropSoul() {
        Vector3 soulPosition = new(torsoRb.transform.position.x, 1, torsoRb.transform.position.z);
        Instantiate(soulPrefab, soulPosition, Quaternion.identity);
    }

    private void setHealth(float health)
    {
        this.health = health;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Damage")
        {
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            //Debug.Log("Player has entered the enemy's trigger");
            // Debug.Log(playerVelocity.velocity.magnitude);
            if(playerVelocity.velocity.magnitude > 5)
            {
                // Debug.Log("Player has entered the enemy's trigger");
                TakeDamage(5);

                if(health <= 0 && !soulDropped) {
                    soulDropped = true;
                    Invoke("DropSoul", 1f);
                }

                turnOnRagdoll();
                Vector3 awayDirection = transform.position - other.transform.position;

                // Normalize the direction to ensure consistent force magnitude
                awayDirection.Normalize();

                rb.AddForce(awayDirection * 10, ForceMode.Impulse);
                
            }
            
        }
        
    }

}
