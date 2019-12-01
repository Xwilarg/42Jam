using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    private Character player;


    // Upgrade
    public GameObject panel;
    public int Cost;
    public int maxLevel;
    public int Level;

    //Effect
    public float timeSlow = 2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        panel = player.GetComponent<TrapShop>().UpgradeShop;
    }


    public void Upgrade()
    {
        if (player.GetOr() >= Cost)
        {
            player.GainOr(-Cost);
            Level += 1;
            timeSlow += 1;
            player.GetComponent<Economy>().UpdateGold();
        }
        else
            Debug.Log("Not enough Gold");
    }

    public void Delete()
    {
        player.GainOr(Cost);
        player.GetComponent<Economy>().UpdateGold();
        player.GetComponent<TrapShop>().DeleteTrap(transform.parent.gameObject);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        panel.SetActive(true);
        panel.GetComponentInChildren<UpgradeTrap>().trap = this.gameObject;
        panel.GetComponentInChildren<DeleteTrap>().trap = this.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hero")
            collision.gameObject.GetComponent<HeroController>().MudSlow(timeSlow);
    }
}
