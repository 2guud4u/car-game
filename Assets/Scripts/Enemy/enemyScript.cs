
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float totalHealth;
    public float thresholdDamageVelocity;

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
    private Animator animator;

    public GameObject enemyObj;

    //player velocity obj
    private Rigidbody playerVelocity;
    public GameObject soulPrefab;
    

    float health;
    bool firstAttack = true;
    bool soulDropped = false;

    public AudioClip damageSound;
    public AudioSource _audioSource;
    public GameObject particleEffect;

    public float timeToUpdateBehavior; // update current behavior every x seconds

    public int maxHealth = 10;
    [SerializeField] GameObject healthBar;
    protected bool doesMove = true;
    private bool collided = false;

    private void Awake()
    {
        health = totalHealth;
        player = GameObject.Find("Body").transform;
        agent = GetComponent<NavMeshAgent>();
        rbs = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        playerVelocity = GameObject.Find("Player").GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if(healthBar != null) { healthBar.GetComponent<HealthBar>().SetMaxHealth(maxHealth); }
        InvokeRepeating("UpdateBehavior", Random.Range(0f, 1f), timeToUpdateBehavior);
    }

    private void Update()
    {
        if (collided && Vector3.Distance(rb.transform.position, player.transform.position) > 3f)
        {
            agent.enabled = true;
            collided = false;
        }
    }
    private void UpdateBehavior()
    {
        //Check for sight and attack range
        //Debug.Log("Checking for sight and attack range");
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        //run if agent is enabled
        if(agent.enabled && agent.isOnNavMesh){
            if (!playerInAttackRange && doesMove) {
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

        if(animator != null) { Animate(); }
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
        AnimateAttack();
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetActive(true);
        healthBar.GetComponent<HealthBar>().SetHealth((int) health);
        // Debug.Log(health + "/" + totalHealth);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    private void turnOnRagdoll(Vector3 force)
    {
        agent.enabled = false;
        //animator.enabled = false;
        foreach (Rigidbody limbRb in rbs)
        {
            limbRb.isKinematic = false;
            limbRb.AddForce(force*2, ForceMode.Impulse);
        }
        Invoke(nameof(turnOffRagdoll), 4f);
    }
    private void turnOffRagdoll()
    {
        
        if(health > 0){
            GameObject enemy = Instantiate(enemyObj, torsoRb.transform.position + new Vector3(0,1,0), Quaternion.identity);
            enemyScript enemyScript = enemy.GetComponentInChildren<enemyScript>();
            enemyScript.setHealth(health);
            enemyScript.healthBar.GetComponent<HealthBar>().SetHealth((int) health);
            enemyScript.healthBar.SetActive(healthBar.activeSelf);
        } else {
            ParticleManager.Instance.PlayEffect(particleEffect, torsoRb.transform.position, 1f);
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

    public virtual void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Damage" || other.gameObject.tag == "Projectile")
        {
            
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 RbVelocity;
            if(other.gameObject.tag == "Player"){
                RbVelocity = playerVelocity.velocity;
                //_audioSource.PlayOneShot(damageSound);
            }
            else{
                RbVelocity = otherRb.velocity;
            }

            Vector3 awayDirection = transform.position - other.transform.position;
            awayDirection.Normalize();

            if (RbVelocity.magnitude > thresholdDamageVelocity)
            {
                // Debug.Log("Player has entered the enemy's trigger");
                float speedBasedDamage = (thresholdDamageVelocity / 7) + Mathf.Log(playerVelocity.velocity.magnitude - (thresholdDamageVelocity / 2) + 1);

                //Vector3 awayDirection = transform.position - other.transform.position;
                //awayDirection.Normalize();

                turnOnRagdoll(0.2f * awayDirection * RbVelocity.magnitude);
                // Vector3 awayDirection = transform.position - other.transform.position;

                // Normalize the direction to ensure consistent force magnitude
                // awayDirection.Normalize();

                rb.isKinematic = false;
                rb.AddForce(0.2f * awayDirection * RbVelocity.magnitude, ForceMode.Impulse);

                TakeDamage(speedBasedDamage);

                if (health <= 0 && !soulDropped)
                {
                    soulDropped = true;
                    Invoke("DropSoul", 1f);
                }

            }
            else
            {
                agent.enabled = false;
                collided = true;

            }

        }

    }

    private void ResetSoldier()
    {
        agent.enabled = true;
        animator.enabled = true;
        foreach (Rigidbody limbRb in rbs)
        {
            limbRb.isKinematic = true;
        }
    }

    public Rigidbody[] GetRbs() {
        return rbs;
    }
    public Rigidbody GetPlayerRigidbody() {
        return playerVelocity;
    }

    public void Animate()
    {
        animator.SetBool("isWalking", agent.enabled && agent.remainingDistance > 0);
    }

    public void AnimateAttack()
    {
        animator.SetTrigger("isAttacking");
    }
}
