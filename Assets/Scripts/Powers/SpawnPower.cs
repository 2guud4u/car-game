using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPower : MonoBehaviour
{
    [SerializeField] GameObject[] powerups;
    [SerializeField] int minEffect;
    [SerializeField] int maxEffect;
    [SerializeField] float spawnChance;

    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0f, 1f) < spawnChance)
        {
            GameObject randomPower = powerups[Random.Range(0, powerups.Length)];
            Instantiate(randomPower, transform);
        }
    }
}
