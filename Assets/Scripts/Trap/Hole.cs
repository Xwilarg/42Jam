using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public int Cost = 1;
    private Character player;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hero")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnMouseDown()
    {
        panel.SetActive(true);
    }


    public void Delete()
    {
        player.GainOr(Cost);
        player.GetComponent<Economy>().UpdateGold();
        player.GetComponent<TrapShop>().DeleteTrap(this.gameObject);
        Destroy(this.gameObject);
    }
}
