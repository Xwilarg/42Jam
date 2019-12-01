using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPile : MonoBehaviour
{
    private int gold;
    private Character player;
    public int Cost;
    public GameObject panel;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        panel = player.GetComponent<TrapShop>().UpgradeShop;
    }

    private void OnMouseDown()
    {
        panel.SetActive(true);
        panel.GetComponentInChildren<UpgradeTrap>().trap = this.gameObject;
        panel.GetComponentInChildren<DeleteTrap>().trap = this.gameObject;
    }

    public void Delete()
    {
        player.GainOr(Cost);
        player.GetComponent<Economy>().UpdateGold();
        player.GetComponent<TrapShop>().DeleteTrap(this.gameObject);
        Destroy(this.gameObject);
    }

    public void SetGold(int value)
    {
        gold = value;
        player.GetComponent<Economy>().UpdateGold();
    }


    public void Upgrade()
    {
        if (player.GetOr() >= Cost)
        {
            player.GainOr(-Cost);
            gold += Cost;
            player.GetComponent<Economy>().UpdateGold();
        }
        else
            Debug.Log("Not enough Gold");
    }
}
