using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralDungeon : MonoBehaviour
{
    public Vector2Int size;
    public uint numberOfRooms;

    private struct RoomDescriptor
    {
        public GameObject instance;
    };

    private RoomDescriptor[,] rooms = null;

    public List<GameObject> roomPrefabs;
    public GameObject corridorHorizontalPrefab;
    public GameObject corridorVerticalPrefab;

    private uint gapBetweenRooms = 2;

    void Start()
    {
        generate();
    }

    void Update()
    {
        if (Input.GetKeyDown("g")) {
            generate();
        }
    }

    public void generate()
    {
        destroyDungeon();

        rooms = new RoomDescriptor[size.y, size.x];
        
        Vector2Int startRoomPos = new Vector2Int(0, (size.y - 1) / 2);
        rooms[startRoomPos.y, startRoomPos.x].instance = findRoomPrefab(Room.Type.Start);
        rooms[startRoomPos.y, startRoomPos.x + 1].instance = findRoomPrefab(Room.Type.Normal);

        uint maxIter = 0;
        uint numberOfGeneratedRooms = 2;
        while (numberOfGeneratedRooms < numberOfRooms) {
            Vector2Int randomRoomPos = new Vector2Int(Random.Range(1, size.x - 1), Random.Range(0, size.y));

            if (rooms[randomRoomPos.y, randomRoomPos.x].instance == null) {
                NeighboursDescriptor neighbours = getNeighbours(randomRoomPos);
                if (neighbours.nb == 1) {
                    rooms[randomRoomPos.y, randomRoomPos.x].instance = findRoomPrefab(Room.Type.Normal);
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
                    NeighboursDescriptor neighbours = getNeighbours(new Vector2Int(j, i));

                    if (neighbours.nb == 1)
                    {
                        singleNeighborRooms.Add(new Vector2Int(j, i));
                    }
                }
            }
        }

        // Add boss room
        int farthestRoomIndex = getFarthestRoomFromList(singleNeighborRooms, startRoomPos);
        rooms[singleNeighborRooms[farthestRoomIndex].y, singleNeighborRooms[farthestRoomIndex].x].instance = findRoomPrefab(Room.Type.Boss);

        instantiateRooms();
    }

    private GameObject findRoomPrefab(Room.Type type)
    {
        foreach (GameObject roomPrefab in roomPrefabs)
        {
            if (roomPrefab.GetComponent<Room>().type == type) {
                return roomPrefab;
            }
        }
        return null;
    }

    private void instantiateRooms()
    {
        for (int i = 0; i < rooms.GetLength(0); i++) {
            for (int j = 0; j < rooms.GetLength(1); j++)
            {
                if (rooms[i, j].instance != null) {
                    Vector2Int size = roomPrefabs[0].GetComponent<Room>().size; // TODO: change this line
                    rooms[i, j].instance = Instantiate(rooms[i, j].instance, new Vector3(j * (size.x + gapBetweenRooms), i * (size.y + gapBetweenRooms), 0), Quaternion.identity, transform);
                
                    // Add corridors
                    NeighboursDescriptor neighbours = getNeighbours(new Vector2Int(j, i));
                    if (neighbours.right == true) {
                        Instantiate(corridorHorizontalPrefab, new Vector3(j * (size.x + gapBetweenRooms) + size.x, i * (size.y + gapBetweenRooms) - size.y / 2 + 2, 0), Quaternion.identity, rooms[i, j].instance.transform);
                    }
                    if (neighbours.down == true) {
                        Instantiate(corridorVerticalPrefab, new Vector3(j * (size.x + gapBetweenRooms) + size.x / 2 - 2, i * (size.y + gapBetweenRooms) - size.y, 0), Quaternion.identity, rooms[i, j].instance.transform);
                    }
                }
            }
        }
    }

    private void destroyDungeon()
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

    private NeighboursDescriptor getNeighbours(Vector2Int index)
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

    int getFarthestRoomFromList(List<Vector2Int> singleNeighborRooms, Vector2Int room)
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
