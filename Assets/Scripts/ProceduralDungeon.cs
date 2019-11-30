using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralDungeon : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(9, 5);
    public uint numberOfRooms;
    private Vector2Int roomSize = new Vector2Int(10, 10);

    public uint maxStartRoom = 3;

    private struct RoomDescriptor
    {
        public GameObject instance;
    };

    private RoomDescriptor[,] rooms = null;

    public List<GameObject> roomPrefabs;
    public GameObject corridorHorizontalPrefab;
    public GameObject corridorVerticalPrefab;
    public Tile wallTileUp;
    public Tile wallTileRight;
    public Tile wallTileDown;
    public Tile wallTileLeft;

    private uint gapBetweenRooms = 2;

    void Start()
    {
        Generate();
    }
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.G)) {
            Generate();
        }
#endif
    }

    public void Generate()
    {
        DestroyDungeon();

        rooms = new RoomDescriptor[size.y, size.x];
        
        Vector2Int startRoomPos = new Vector2Int(size.x - 1, (size.y - 1) / 2);
        rooms[startRoomPos.y, startRoomPos.x].instance = FindRoomPrefab(Room.Type.Boss);
        rooms[startRoomPos.y, startRoomPos.x - 1].instance = FindRoomPrefab(Room.Type.Normal);

        uint maxIter = 0;
        uint numberOfGeneratedRooms = 2;
        while (numberOfGeneratedRooms < numberOfRooms) {
            Vector2Int randomRoomPos = new Vector2Int(Random.Range(0, size.x - 2), Random.Range(0, size.y));

            if (rooms[randomRoomPos.y, randomRoomPos.x].instance == null) {
                NeighboursDescriptor neighbours = GetNeighbours(randomRoomPos);
                if (neighbours.nb == 1 || (neighbours.nb == 2 && Random.Range(1, 10) <= 3)) {
                    rooms[randomRoomPos.y, randomRoomPos.x].instance = FindRoomPrefab(Room.Type.Normal);
                    numberOfGeneratedRooms++;
                }
            }

            if (maxIter >= 1000000) {
                Debug.LogError("Too much iterations, can't generate map!");
                break;
            }
            maxIter++;
        }

        /* Add special rooms */
        // Get all single neighbor rooms
        List<Vector2Int> singleNeighborRooms = new List<Vector2Int>();
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                if (rooms[i, j].instance != null && rooms[i, j].instance.GetComponent<Room>().type == Room.Type.Normal)
                {
                    NeighboursDescriptor neighbours = GetNeighbours(new Vector2Int(j, i));

                    if (neighbours.nb == 1)
                    {
                        singleNeighborRooms.Add(new Vector2Int(j, i));
                    }
                }
            }
        }

        // Add start rooms
        if (singleNeighborRooms.Count == 0) {
            Generate();
        }

        int generatedStartRoom = 0;
        while (singleNeighborRooms.Count > 0 && generatedStartRoom < maxStartRoom) {
            int farthestRoom = GetFarthestRoomFromList(singleNeighborRooms, startRoomPos);
            rooms[singleNeighborRooms[farthestRoom].y, singleNeighborRooms[farthestRoom].x].instance = FindRoomPrefab(Room.Type.Start);
            singleNeighborRooms.RemoveAt(farthestRoom);
            generatedStartRoom++;
        }

        InstantiateRooms();

        LinkNodes();
    }

    private GameObject FindRoomPrefab(Room.Type type)
    {
        foreach (GameObject roomPrefab in roomPrefabs)
        {
            if (roomPrefab.GetComponent<Room>().type == type) {
                return roomPrefab;
            }
        }
        return null;
    }

    private void InstantiateRooms()
    {
        for (int i = 0; i < rooms.GetLength(0); i++) {
            for (int j = 0; j < rooms.GetLength(1); j++)
            {
                if (rooms[i, j].instance != null) {
                    rooms[i, j].instance = Instantiate(rooms[i, j].instance, new Vector3(j * (roomSize.x + gapBetweenRooms), i * (roomSize.y + gapBetweenRooms), 0), Quaternion.identity, transform);

                    // Add corridors and walls
                    NeighboursDescriptor neighbours = GetNeighbours(new Vector2Int(j, i));

                    Tilemap wallTilemap = null;
                    foreach (Transform child in rooms[i, j].instance.transform) {
                        if (child.name == "Wall") {
                            wallTilemap = child.gameObject.GetComponent<Tilemap>();
                        }
                    }
                    if (wallTilemap == null) {
                        Debug.LogError("Can't find wallTilemap!");
                    }
                    wallTilemap.CompressBounds();

                    if (neighbours.up == false) {
                        for (int k = 0; k < 4; k++) {
                            wallTilemap.SetTile(new Vector3Int(3 + k, -1, 0), wallTileUp);
                        }
                    }
                    if (neighbours.right == false) {
                        for (int k = 0; k < 4; k++) {
                            wallTilemap.SetTile(new Vector3Int(9, -4 - k, 0), wallTileRight);
                        }
                    }
                    else {
                        Instantiate(corridorHorizontalPrefab, new Vector3(j * (roomSize.x + gapBetweenRooms) + roomSize.x - 1, i * (roomSize.y + gapBetweenRooms) - roomSize.y / 2 + 2, 0), Quaternion.identity, rooms[i, j].instance.transform);
                    }
                    if (neighbours.down == false) {
                        for (int k = 0; k < 4; k++) {
                            wallTilemap.SetTile(new Vector3Int(3 + k, -10, 0), wallTileDown);
                        }
                    }
                    else {
                        Instantiate(corridorVerticalPrefab, new Vector3(j * (roomSize.x + gapBetweenRooms) + roomSize.x / 2 - 2, i * (roomSize.y + gapBetweenRooms) - roomSize.y + 1, 0), Quaternion.identity, rooms[i, j].instance.transform);
                    }
                    if (neighbours.left == false) {
                        for (int k = 0; k < 4; k++) {
                            wallTilemap.SetTile(new Vector3Int(0, -4 - k, 0), wallTileLeft);
                        }
                    }
                }
            }
        }
    }

    private void LinkNodes()
    {
        for (int i = 0; i < rooms.GetLength(0); i++)
        {
            for (int j = 0; j < rooms.GetLength(1); j++)
            {
                if (rooms[i, j].instance != null)
                {
                    NeighboursDescriptor neighbours = GetNeighbours(new Vector2Int(j, i));
                    Node node = rooms[i, j].instance.GetComponentInChildren<Node>();
                    node.nodes = new Node[neighbours.nb];

                    int index = 0;
                    if (neighbours.up)
                    {
                        node.nodes[index++] = rooms[i + 1, j].instance.GetComponentInChildren<Node>();
                    }
                    if (neighbours.right)
                    {
                        node.nodes[index++] = rooms[i, j + 1].instance.GetComponentInChildren<Node>();
                    }
                    if (neighbours.down)
                    {
                        node.nodes[index++] = rooms[i - 1, j].instance.GetComponentInChildren<Node>();
                    }
                    if (neighbours.left)
                    {
                        node.nodes[index++] = rooms[i, j - 1].instance.GetComponentInChildren<Node>();
                    }
                }
            }
        }
    }

    private void DestroyDungeon()
    {
        if (rooms != null) {
            for (int i = 0; i < rooms.GetLength(0); i++)
            {
                for (int j = 0; j < rooms.GetLength(1); j++)
                {
                    if (rooms[i, j].instance != null)
                    {
                       Destroy(rooms[i, j].instance);
                    }
                }
            }
            rooms = null;
        }
    }

    private struct NeighboursDescriptor
    {
        public uint nb;
        public bool up;
        public bool right;
        public bool down;
        public bool left;
    };

    private NeighboursDescriptor GetNeighbours(Vector2Int index)
    {
        NeighboursDescriptor ret;
        ret.nb = 0;
        ret.up = false;
        ret.right = false;
        ret.down = false;
        ret.left = false;

        if (index.y + 1 < size.y && rooms[index.y + 1, index.x].instance != null)
        {
            ret.up = true;
            ret.nb++;
        }
        if (index.x + 1 < size.x && rooms[index.y, index.x + 1].instance != null)
        {
            ret.right = true;
            ret.nb++;
        }
        if (index.y - 1 >= 0 && rooms[index.y - 1, index.x].instance != null)
        {
            ret.down = true;
            ret.nb++;
        }
        if (index.x - 1 >= 0 && rooms[index.y, index.x - 1].instance != null)
        {
            ret.left = true;
            ret.nb++;
        }

        return ret;
    }

    int GetFarthestRoomFromList(List<Vector2Int> singleNeighborRooms, Vector2Int room)
    {
        int farthestRoomIndex = 0;
        int farthestRoomValue = Mathf.Abs(room.x - singleNeighborRooms[0].x) + Mathf.Abs(room.y - singleNeighborRooms[0].y); // Get distance of first

        for (int i = 1; i < singleNeighborRooms.Count; i++)
        {
            int value = Mathf.Abs(room.x - singleNeighborRooms[i].x) + Mathf.Abs(room.y - singleNeighborRooms[i].y);
            if (value > farthestRoomValue)
            {
                farthestRoomValue = value;
                farthestRoomIndex = i;
            }
        }

        return farthestRoomIndex;
    }
}
