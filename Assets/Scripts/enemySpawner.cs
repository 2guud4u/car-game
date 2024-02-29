using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float spawnRate = 5.0f;
    public GameObject enemyPrefab;
    GameObject[] spawnPoints;
    // Update is called once per frame
    //public AudioSource audioSource;
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
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
        // spawning left
       
        //Vector3 spawnPos = new Vector3(Random.Range(-40, 79), 1.5f, Random.Range(-280, 400));
        //GameObject l = Instantiate(enemyPrefab, spawnPos, Quaternion.identity); 
        int random = Random.Range(0, spawnPoints.Length);
        GameObject l = Instantiate(enemyPrefab, spawnPoints[random].transform.Find("SpawnPoint").transform.position, Quaternion.identity);

    }
}

