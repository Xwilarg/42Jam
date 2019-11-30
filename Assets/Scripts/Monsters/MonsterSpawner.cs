using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterToSpawn;
    public uint numberToSpawn;
    
    void Start()
    {
        for (int i = 0; i < numberToSpawn; i++) {
            Instantiate(monsterToSpawn, transform.position, Quaternion.identity, transform);
        }
    }

    void Update()
    {
        
    }
}
