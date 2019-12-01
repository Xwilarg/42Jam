using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public int damage = 10;
    [HideInInspector]
    public GameObject caller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hero") {
            collision.GetComponent<Character>().LooseHp(damage);
        }
        if (collision.tag == "Hero" || collision.tag == "Wall") {
            Destroy(this.gameObject);
        }
    }
}
