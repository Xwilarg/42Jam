﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int size;

    public enum Type
    {
        Normal,
        Start,
        Boss
    };

    public Type type = Type.Normal;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
