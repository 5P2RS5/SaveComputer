using System;
using System.Collections.Generic;
using BuildingSystem.Models;
using Extensions;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingSystem
{
    public class ConstructionLayer : TileMapLayer
    {
        private Dictionary<Vector3Int, Buildable> buildables = new();

        [SerializeField] 
        private CollisionLayer collisionLayer;

        public void Build(Vector3 worldCoords, BuildableItem item) // Cell단위로 타일을 설치하기 위함
        {
            GameObject itemObject = null;
            Vector3Int coords = tileMap.WorldToCell(worldCoords); // WorldToCell : 월드 좌표의 실수형을 정수형으로 반환
            if (item.tile != null)
            {
                var tileChangeData =
                    new TileChangeData(coords, item.tile, Color.white, Matrix4x4.Translate(item.tileOffset));
                tileMap.SetTile(tileChangeData, false);
            }

            if (item.gameObject != null)
            {
                itemObject = Instantiate(item.gameObject,
                    tileMap.CellToWorld(coords) + Vector3.one * 0.5f,
                    Quaternion.identity);
                // itemObject = Instantiate(item.gameObject,
                //     tileMap.CellToWorld(coords) + tileMap.cellSize / 2 + item.tileOffset,
                //     Quaternion.identity);
            }

            var buildable = new Buildable(item, coords, tileMap, itemObject);

            if (item.useCustomCollisionSpace)
            {
                collisionLayer.SetCollisions(buildable, true);
                RegisterBuildableCollisionSpace(buildable);
            }
            else
            {
                buildables.Add(coords, buildable);
            }
        }

        public void Destroy(Vector3 worldCoords)
        {
            var coords = tileMap.WorldToCell(worldCoords);
            if (!buildables.ContainsKey(coords))
            {
                return;
            }

            var buildable = buildables[coords];
            if (buildable.buildableType.useCustomCollisionSpace)
            {
                collisionLayer.SetCollisions(buildable, false);
                UnRegisterBuildableCollisionSpace(buildable);
            }
            buildables.Remove(coords);
            buildable.Destroy();
        }
        
        public bool IsEmpty(Vector3 worldCoords, RectInt collisionSpace = default)
        {
            Vector3Int coords = tileMap.WorldToCell(worldCoords);
            if (!collisionSpace.Equals(default))
            {
                return !IsRectOccupied(coords, collisionSpace);
            }
            return !buildables.ContainsKey(coords) && tileMap.GetTile(coords) == null; 
            // ContainsKey: Dictionary에 coords 키가 존재하는지 리턴
            // 비어있어야 설치가 가능하므로 false를 리턴했을때 함수가 true를 리턴하도록 해야함.
        }

        private void RegisterBuildableCollisionSpace(Buildable buildable)
        {
            buildable.IterateCollisionSpace(tileCoords => buildables.Add(tileCoords, buildable));
        }

        private void UnRegisterBuildableCollisionSpace(Buildable buildable)
        {
            buildable.IterateCollisionSpace(tileCoords =>
            {
                buildables.Remove(tileCoords);
            });
        }
        
        private bool IsRectOccupied(Vector3Int coords, RectInt rect)
        {
            return rect.Iterate(coords, tileCoords => buildables.ContainsKey(tileCoords));
        }
    }
}
