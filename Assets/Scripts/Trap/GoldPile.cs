using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPile : MonoBehaviour
{
    private int gold;

    public int GiveGold()
    {
        int tmp = gold;
        gold = 0;
        return tmp;
    }

    public void SetGold(int value)
    {
        gold = value;
    }
}
