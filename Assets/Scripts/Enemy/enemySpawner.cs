using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class enemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float spawnRate = 5.0f;
    [SerializeField] float spawnRange;
    public int maxEnemiesNearby;
    public LayerMask spawnLayer;
    public LayerMask enemyLayer;
    public GameObject enemyPrefab;
    GameObject player;
    // Update is called once per frame
    //public AudioSource audioSource;
    void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(SpawnWithDelayCoroutine());
    }
    IEnumerator SpawnWithDelayCoroutine(){
        while(true){
            yield return new WaitForSeconds(spawnRate);
            spawnEnemy();
            //audioSource.Play();
        }
    }
    private void spawnEnemy(){
        Collider[] nearSpawnPoints = Physics.OverlapSphere(player.transform.position, spawnRange, spawnLayer, QueryTriggerInteraction.Collide);
        Collider[] nearEnemies = Physics.OverlapSphere(player.transform.position, spawnRange, enemyLayer, QueryTriggerInteraction.Collide);
        int nearEnemiesCount = nearEnemies.Where(c => c.gameObject.transform.parent == null).ToArray().Length;

        int random = Mathf.Min(nearSpawnPoints.Length, Random.Range(0, nearSpawnPoints.Length));
        if(nearEnemiesCount < maxEnemiesNearby && nearSpawnPoints.Length > 0)
        {
            GameObject l = Instantiate(enemyPrefab, nearSpawnPoints[random].gameObject.transform.position, Quaternion.identity);
        }

    }
}

