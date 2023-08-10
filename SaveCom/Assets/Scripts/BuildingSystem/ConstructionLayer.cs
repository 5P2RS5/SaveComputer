using System;
using System.Collections.Generic;
using BuildingSystem.Models;
using Extensions;
using Unity.Mathematics;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingSystem
{
    public class ConstructionLayer : TileMapLayer
    {
        public GameObject parentObj;
        
        private Dictionary<Vector3Int, Buildable> buildables = new();
        
        [SerializeField] 
        private CollisionLayer collisionLayer;

        private BuildableItem tower;

        public void Build(Vector3 worldCoords, BuildableItem item) // Cell단위로 타일을 설치하기 위함
        {
            GameObject itemObject = null;
            Vector3Int coords = tileMap.WorldToCell(worldCoords); // WorldToCell : 월드 좌표의 실수형을 정수형으로 반환
            
            item.coords = coords;
            
            if (item.tile != null)
            {
                var tileChangeData =
                    new TileChangeData(item.coords, item.tile, Color.white, Matrix4x4.Translate(item.tileOffset));
                tileMap.SetTile(tileChangeData, false);
            }

            if (item.gameObject != null)
            {
                // itemObject = Instantiate(item.gameObject,
                //     tileMap.CellToWorld(coords) + Vector3.one * 0.5f,
                //     Quaternion.identity, transform);
                
                itemObject = Instantiate(item.gameObject,
                    tileMap.CellToWorld(coords) + Vector3.one * 0.5f,
                    Quaternion.identity, parentObj.transform);
                itemObject.gameObject.GetComponent<Tower>().TowerInit(item, coords);
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

        public void Destroy(Vector3 worldCoords) // 건물 삭제
        {
            var coords = tileMap.WorldToCell(worldCoords);
            if (!buildables.ContainsKey(coords))
            {
                return;
            }
            
            var buildable = buildables[coords];
            if (buildable.buildableType.name == "Server")
                GameSystem.instance.serverCnt--;
            if (buildable.buildableType.useCustomCollisionSpace)
            {
                collisionLayer.SetCollisions(buildable, false);
                UnRegisterBuildableCollisionSpace(buildable);
            }
            buildables.Remove(coords);
            buildable.Destroy();
        }

        public Buildable TowerInfo(Vector3 coords)
        {
            var convertCoords = tileMap.WorldToCell(coords);
            var towers = (buildables.ContainsKey(convertCoords)) ? buildables[convertCoords] : null;
            return towers;
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
