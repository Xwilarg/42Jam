using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapShop : MonoBehaviour
{
    private GameObject player;
    public GameObject shopPanel;

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
        trapPlaced = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(player.transform.position, transform.position) < 5 && !shopPanel.activeInHierarchy)
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
        Debug.Log("oui");
        if (tileSelected._isVisible && !IsTaken(_pos))
        {
            Debug.Log("bah oui connard");
            GameObject _trap;
            if (type == TrapType.spawner && player.GetComponent<Character>().GetOr() >= spawner.GetComponent<Spawner>().Cost)
            {
                _trap = Instantiate(spawner, _pos, Quaternion.identity);
                player.GetComponent<Character>().GainOr(-spawner.GetComponent<Spawner>().Cost);
                trapPlaced.Add(_trap);
            }
            else if (type == TrapType.arrowTrap && player.GetComponent<Character>().GetOr() >= arrowTrap.GetComponentInChildren<Trap>().Cost)
            {
                _trap = Instantiate(arrowTrap, _pos, Quaternion.identity);
                player.GetComponent<Character>().GainOr(-arrowTrap.GetComponentInChildren<Trap>().Cost);
                trapPlaced.Add(_trap);
            }
            else if (type == TrapType.holeTrap && player.GetComponent<Character>().GetOr() >= spawner.GetComponent<Spawner>().Cost)
            {
                _trap = Instantiate(holeTrap, _pos, Quaternion.identity);
                player.GetComponent<Character>().GainOr(-spawner.GetComponent<Spawner>().Cost);
                trapPlaced.Add(_trap);
            }
            else if (type == TrapType.gold && player.GetComponent<Character>().GetOr() >= 20)
            {
                _trap = Instantiate(goldPile, _pos, Quaternion.identity);
                player.GetComponent<Character>().GainOr(-20);
                trapPlaced.Add(_trap);
            }
        }
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

}
