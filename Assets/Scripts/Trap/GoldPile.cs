﻿using System.Collections;
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

    public void SetGold(int value)
    {
        gold = value;
        player.GetComponent<Economy>().UpdateGold();
    }
}
