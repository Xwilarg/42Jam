using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapShop : MonoBehaviour
{
    private GameObject player;
    public GameObject shopPanel;
    private GameObject HeartOfDungeon;

    // Traps
    public GameObject spawner;
    public GameObject holeTrap;
    public GameObject arrowTrap;
    public GameObject goldPile;
    public enum TrapType
    {
        spawner,
        holeTrap,
        arrowTrap,
        gold
    }
    public TrapType type;
    private List<GameObject> trapPlaced;
    public TargetTile tileSelected;
    public RectTransform tileSelectedDisplay;
    public List<GameObject> _trapTypeDisplay;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        HeartOfDungeon = GameObject.Find("Dungeon Heart");
        trapPlaced = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(HeartOfDungeon.transform.position, transform.position) < 5 && !shopPanel.activeInHierarchy)
        {
            ShopOpen();
        }
        else if (Input.GetKeyDown(KeyCode.E) && shopPanel.activeInHierarchy)
        {
            ShopClose();
        }
        if (Input.GetMouseButtonDown(0) && shopPanel.activeInHierarchy)
            PlaceTrap(tileSelected.gameObject.transform.position);
    }

    void ShopOpen()
    {
        shopPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void ShopClose()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void SetSpawner()
    {
        type = TrapType.spawner;
        tileSelectedDisplay.position = new Vector3(tileSelectedDisplay.transform.position.x, _trapTypeDisplay[0].transform.position.y, tileSelectedDisplay.transform.position.z);
    }

    public void SetHole()
    {
        type = TrapType.holeTrap;
        tileSelectedDisplay.position = new Vector3(tileSelectedDisplay.transform.position.x, _trapTypeDisplay[1].transform.position.y, tileSelectedDisplay.transform.position.z);
    }

    public void SetArrows()
    {
        type = TrapType.arrowTrap;
        tileSelectedDisplay.position = new Vector3(tileSelectedDisplay.transform.position.x, _trapTypeDisplay[2].transform.position.y, tileSelectedDisplay.transform.position.z);
    }

    public void SetGoldDisplay()
    {
        type = TrapType.gold;
        tileSelectedDisplay.position = new Vector3(tileSelectedDisplay.transform.position.x, _trapTypeDisplay[3].transform.position.y, tileSelectedDisplay.transform.position.z);
    }

    void PlaceTrap(Vector3 _pos)
    {
        if (tileSelected._isVisible && !IsTaken(_pos))
        {
            GameObject _trap;
            if (type == TrapType.spawner && GetComponent<Character>().GetOr() >= spawner.GetComponent<Spawner>().Cost)
                CreateTrap(spawner, _pos);
            else if (type == TrapType.arrowTrap && GetComponent<Character>().GetOr() >= arrowTrap.GetComponentInChildren<Trap>().Cost)
                CreateTrap(arrowTrap, _pos);
            else if (type == TrapType.holeTrap && GetComponent<Character>().GetOr() >= spawner.GetComponent<Spawner>().Cost)
                CreateTrap(holeTrap, _pos);
            else if (type == TrapType.gold && GetComponent<Character>().GetOr() >= 20)
                CreateTrap(goldPile, _pos);
        }
    }

    void CreateTrap(GameObject trap, Vector3 _pos)
    {
        GameObject _trap = Instantiate(trap, _pos, Quaternion.identity);
        GetComponent<Character>().GainOr(-20);
        GetComponent<Economy>().UpdateGold();
        trapPlaced.Add(_trap);
    }

    bool IsTaken(Vector3 pos)
    {
        for (int i = 0; i < trapPlaced.Count; i++)
        {
            if (trapPlaced[i].transform.position == pos)
                return true;
        }
        return false;
    }

    public bool DeleteTrap(GameObject trap)
    {
        for (int i = 0; i < trapPlaced.Count; i++)
        {
            if (trapPlaced[i] == trap)
            {
                trapPlaced.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

}
