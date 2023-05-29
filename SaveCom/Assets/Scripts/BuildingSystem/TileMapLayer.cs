using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingSystem
{
    [RequireComponent(typeof(Tilemap))]
    public class TileMapLayer : MonoBehaviour
    {
        protected Tilemap tileMap { get; private set; }

        protected void Awake()
        {
            tileMap = GetComponent<Tilemap>();
        }
    }
}