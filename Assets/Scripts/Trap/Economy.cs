using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy : MonoBehaviour
{
    public int maxGold;
    private List<GameObject> goldPile;
    public GameObject goldPrefab;
    private Character player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        player.GainOr(100);
        Debug.Log(player.GetOr());
        goldPile = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            DropGold();
    }

    void DropGold()
    {
        if (player.GetOr() >= 20)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            GameObject newGoldPile = Instantiate(goldPrefab, pos, Quaternion.identity);
            goldPile.Add(newGoldPile);
            newGoldPile.GetComponent<GoldPile>().SetGold(20);
            player.GainOr(-20);
        }
    }
}
