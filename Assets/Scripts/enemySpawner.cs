using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float spawnRate = 5.0f;
    public GameObject enemyPrefab;
    // Update is called once per frame
    //public AudioSource audioSource;
    void Start()
    {
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
       
        Vector3 spawnPos = new Vector3(Random.Range(-40, 79), 1.5f, Random.Range(-280, 400));
        GameObject l = Instantiate(enemyPrefab, spawnPos, Quaternion.identity); 
            

        
    }
}

