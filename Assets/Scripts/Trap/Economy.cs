using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Economy : MonoBehaviour
{
    private Character player;
    public Text goldDisplay;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Character>();
        player.GainOr(100);
        goldDisplay.text = "gold : " + player.GetOr();
    }

    public void UpdateGold()
    {
        goldDisplay.text = "gold : " + player.GetOr();
    }
}
