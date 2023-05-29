using BuildingSystem.Models;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingSystem
{
    public class CollisionLayer : TileMapLayer
    {
        [SerializeField] 
        private TileBase collisionTileBase;

        public void SetCollisions(Buildable buildable, bool value)
        {
            var tile = value ? collisionTileBase : null;
            buildable.IterateCollisionSpace(tileCoords => tileMap.SetTile(tileCoords, tile));
        }
    }
}
