using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TargetTile : MonoBehaviour
{
    public Tilemap _tm;
    public bool _isVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        _tm.CompressBounds();
        SetInvisible();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int pos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        pos.z = 0;
        Vector3 posNormalized = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0);
        if (_tm.HasTile(pos))
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
