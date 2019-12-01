using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TargetTile : MonoBehaviour
{
    public GameObject[] TilemapList;
    public List<Tilemap> _tm;
    public bool _isVisible = false;
    private GameObject Dungeon;

    // Start is called before the first frame update
    void Start()
    {
        Dungeon = GameObject.Find("Dungeon");
        TileMapSave();
        SetInvisible();
    }

    void TileMapSave()
    {
        TilemapList = GameObject.FindGameObjectsWithTag("Ground");
        for (int i = 0; i < TilemapList.Length; i++)
        {
            _tm.Add(TilemapList[i].GetComponent<Tilemap>());
            _tm[i].CompressBounds();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int pos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        pos.z = 0;
        Vector3 posNormalized = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0);
        if (HasTileInList(pos))
        {
            transform.position = posNormalized;
            SetVisible();
        }
        else
        {
            transform.position = posNormalized;
            SetInvisible();
        }

    }

    bool HasTileInList(Vector3Int pos)
    {
        Debug.Log(pos);
        for (int i = 0; i < _tm.Count; i++)
        {
            if (_tm[i].HasTile(pos - Vector3Int.FloorToInt(_tm[i].transform.position)))
            {
                return true;
            }
        }
        return false;
    }

    void SetVisible()
    {
        _isVisible = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void SetInvisible()
    {
        _isVisible = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

}
