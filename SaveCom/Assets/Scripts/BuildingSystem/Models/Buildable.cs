using System;
using Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace BuildingSystem.Models
{
    [Serializable]
    public class Buildable
    {
        [field: SerializeField] public Tilemap parenTilemap { get; private set; }
        
        [field: SerializeField] public BuildableItem buildableType { get; private set; }
        
        [field: SerializeField] public GameObject gameObject { get; private set; }
        
        [field: SerializeField] public Vector3Int coordinates { get; private set; }

        public Buildable(BuildableItem type, Vector3Int coords, Tilemap tilemap, GameObject gameObject = null)
        {
            parenTilemap = tilemap;
            buildableType = type;
            coordinates = coords;
            this.gameObject = gameObject;
        }

        public void Destroy()
        {
            if (gameObject != null)
            {
                Object.Destroy(gameObject);
            }
            parenTilemap.SetTile(coordinates, null);
        }

        public void IterateCollisionSpace(RectIntExtensions.RectAction action)
        {
            buildableType.CollisionSpace.Iterate(coordinates, action);
        }
        
        public bool IterateCollisionSpace(RectIntExtensions.RectActionBool action)
        {
            return buildableType.CollisionSpace.Iterate(coordinates, action);
        }
    }
}
