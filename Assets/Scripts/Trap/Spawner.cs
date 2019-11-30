using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Spawner : MonoBehaviour
{
    public GameObject gobelin;
    public float rate;
    public int maxLevel;
    public int Level;
    public List<Sprite> upgradeSprite;
    public int Cost;
    private Character player;
    public GameObject panel;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        //StartCoroutine(cooldown());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            Upgrade();
        
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(rate);
        Spawn();
    }

    void Spawn()
    {
        Instantiate(gobelin, transform);
        StartCoroutine(cooldown());
    }

    public void Upgrade()
    {
        if (player.GetOr() >= Cost)
        {
            GetComponent<SpriteRenderer>().sprite = upgradeSprite[Level];
        }
        else
            Debug.Log("Not enough Gold");
    }

    public void Delete()
    {
        player.GainOr(Cost * Level);
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        panel.SetActive(true);
    }
}
