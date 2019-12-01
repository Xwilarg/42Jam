using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapShop : MonoBehaviour
{
    private GameObject player;
    private Character playerC;
    public GameObject shopPanel;
    private GameObject HeartOfDungeon;
    private CameraManager cm;
    private GameObject shopText;

    private const float healTimerRef = .2f;
    private float healTimer;

    // Traps
    public GameObject spawner;
    public string spawnerDescription;
    public GameObject holeTrap;
    public string holeTrapDescription;
    public GameObject arrowTrap;
    public string arrowTrapDescription;
    public GameObject goldPile;
    public string goldPileDescription;
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
    public Text _trapDescription;
    public GameObject UpgradeShop;

    // Start is called before the first frame update
    public void Start()
    {
        cm = GameObject.FindGameObjectWithTag("GameController").GetComponent<CameraManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerC = player.GetComponent<Character>();
        HeartOfDungeon = GameObject.Find("Dungeon Heart");

        //init shop
        _trapTypeDisplay.Add(GameObject.Find("SpawnSelection"));
        _trapTypeDisplay.Add(GameObject.Find("HoleSelection"));
        _trapTypeDisplay.Add(GameObject.Find("ArrowsSelection"));
        _trapTypeDisplay.Add(GameObject.Find("GoldSelection"));
        _trapTypeDisplay[0].GetComponent<Button>().onClick.AddListener(SetSpawner);
        _trapTypeDisplay[1].GetComponent<Button>().onClick.AddListener(SetHole);
        _trapTypeDisplay[2].GetComponent<Button>().onClick.AddListener(SetArrows);
        _trapTypeDisplay[3].GetComponent<Button>().onClick.AddListener(SetGoldDisplay); tileSelectedDisplay = GameObject.Find("Selection").GetComponent<RectTransform>();
        _trapDescription = GameObject.Find("TrapDescription").GetComponent<Text>();
        spawnerDescription = "Invocator Gate\n\nInvoke a gobelin every 5 seconds to kill adventurers\n\nCost: 70 golds";
        holeTrapDescription = "Hole\n\nKill 1 adventurer if we walk on it\n\nCost: 25 gold";
        arrowTrapDescription = "Arrows Wall\n\nSend arrows when an adventurer come in front of them\nCost: 110 golds";
        goldPileDescription = "Gold Pile\n\nPut an amount of gold somewhere, it will allow you to accumulate more gold, you can take it back when needed.\nAdventurers will go take in in priority";
        _trapDescription.text = spawnerDescription;
        tileSelected = GameObject.Find("TargetTile").GetComponent<TargetTile>();
        shopPanel = GameObject.Find("Shop");
        trapPlaced = new List<GameObject>();
        shopText = GameObject.FindGameObjectWithTag("ShopText");
        UpgradeShop = GameObject.Find("UpgradeShop");
        UpgradeShop.SetActive(false);
        ShopClose();
        healTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(HeartOfDungeon.transform.position, transform.position) < 5)
        {
            shopText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && !shopPanel.activeInHierarchy)
            {
                cm.EnableShop();
                ShopOpen();
            }
            else if (Input.GetKeyDown(KeyCode.E) && shopPanel.activeInHierarchy)
            {
                cm.DisableShop();
                ShopClose();
            }
            if (Input.GetMouseButtonDown(0) && shopPanel.activeInHierarchy)
                PlaceTrap(tileSelected.gameObject.transform.position);
        }
        else
        {
            shopText.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(HeartOfDungeon.transform.position, transform.position) < 5)
        {
            healTimer -= Time.deltaTime;
            if (healTimer < 0f)
            {
                playerC.LooseHp(-1);
                healTimer = healTimerRef;
            }
        }
    }

    void ShopOpen()
    {
        shopPanel.SetActive(true);
        tileSelected.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    void ShopClose()
    {
        shopPanel.SetActive(false);
        tileSelected.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void SetSpawner()
    {
        type = TrapType.spawner;
        tileSelectedDisplay.position = new Vector3(tileSelectedDisplay.transform.position.x, _trapTypeDisplay[0].transform.position.y, tileSelectedDisplay.transform.position.z);
        _trapDescription.text = spawnerDescription;
    }

    public void SetHole()
    {
        type = TrapType.holeTrap;
        tileSelectedDisplay.position = new Vector3(tileSelectedDisplay.transform.position.x, _trapTypeDisplay[1].transform.position.y, tileSelectedDisplay.transform.position.z);
        _trapDescription.text = holeTrapDescription;
    }

    public void SetArrows()
    {
        type = TrapType.arrowTrap;
        tileSelectedDisplay.position = new Vector3(tileSelectedDisplay.transform.position.x, _trapTypeDisplay[2].transform.position.y, tileSelectedDisplay.transform.position.z);
        _trapDescription.text = arrowTrapDescription;
    }

    public void SetGoldDisplay()
    {
        type = TrapType.gold;
        tileSelectedDisplay.position = new Vector3(tileSelectedDisplay.transform.position.x, _trapTypeDisplay[3].transform.position.y, tileSelectedDisplay.transform.position.z);
        _trapDescription.text = goldPileDescription;
    }

    void PlaceTrap(Vector3 _pos)
    {
        if (tileSelected._isVisible && !IsTaken(_pos))
        {
            if (type == TrapType.spawner && GetComponent<Character>().GetOr() >= spawner.GetComponent<Spawner>().Cost)
                CreateTrap(spawner, _pos, spawner.GetComponent<Spawner>().Cost);
            else if (type == TrapType.arrowTrap && GetComponent<Character>().GetOr() >= arrowTrap.GetComponentInChildren<Trap>().Cost)
                CreateTrap(arrowTrap, _pos, arrowTrap.GetComponentInChildren<Trap>().Cost);
            else if (type == TrapType.holeTrap && GetComponent<Character>().GetOr() >= holeTrap.GetComponent<Hole>().Cost)
                CreateTrap(holeTrap, _pos, holeTrap.GetComponent<Hole>().Cost);
            else if (type == TrapType.gold && GetComponent<Character>().GetOr() >= 20)
                CreateTrap(goldPile, _pos, goldPile.GetComponent<GoldPile>().Cost);
        }
    }

    void CreateTrap(GameObject trap, Vector3 _pos, int cost)
    {
        GameObject _trap = Instantiate(trap, _pos, Quaternion.identity, gameObject.transform.parent.transform);
        GetComponent<Character>().GainOr(-cost);
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

    public void OnDestroy()
    {
        shopPanel.SetActive(true);
        tileSelected.gameObject.SetActive(true);
        UpgradeShop.SetActive(true);
    }
}
