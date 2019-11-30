using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum CanHaveNeighbour
    {
        Yes,
        Maybe,
        No
    };

    public enum Type
    {
        Normal,
        Start,
        Boss
    };

    public Type type = Type.Normal;

    public CanHaveNeighbour up = CanHaveNeighbour.Maybe;
    public CanHaveNeighbour right = CanHaveNeighbour.Maybe;
    public CanHaveNeighbour down = CanHaveNeighbour.Maybe;
    public CanHaveNeighbour left = CanHaveNeighbour.Maybe;


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
