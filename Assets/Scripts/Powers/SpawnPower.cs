using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPower : MonoBehaviour
{
    GameObject currentPower;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("PowerUpdate", Random.Range(0, 5f), 30f);
        Transform child = GetComponentInChildren<Transform>();
        if(child != null)
        {
            currentPower = child.gameObject;
        }
    }

    void PowerUpdate()
    {
        if(meshRenderer != null && meshRenderer.isVisible) { return; }
        
        if(currentPower != null) { Destroy(currentPower); }

        (GameObject, float)[] powerUps = GameManager.Instance.GetPowers();
        (GameObject, float) powerUp = powerUps[Random.Range(0, powerUps.Length)];   

        GameObject power = powerUp.Item1;
        float spawnChance = powerUp.Item2 * powerUps.Length;
        
        if(Random.Range(0f, 1f) < spawnChance)
        {
            currentPower = Instantiate(power, transform);
            meshRenderer = currentPower.GetComponent<MeshRenderer>();
        }
    }
}
