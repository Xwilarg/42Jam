using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private int health = 100;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) {
            // TODO: death animation
            Destroy(this);
            return true;
        }
        return false;
    }
}
