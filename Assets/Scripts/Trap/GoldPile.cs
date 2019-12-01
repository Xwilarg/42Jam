using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPile : MonoBehaviour
{
    private int gold;
    private Character player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    public int GiveGold()
    {
        int tmp = gold;
        gold = 0;
        player.GetComponent<Economy>().UpdateGold();
        player.GetComponent<TrapShop>().DeleteTrap(this.gameObject);
        return tmp;
    }

    public void SetGold(int value)
    {
        gold = value;
        player.GetComponent<Economy>().UpdateGold();
    }
}
