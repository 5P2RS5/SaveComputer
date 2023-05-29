using UnityEngine;
using BuildingSystem.Models;
using UnityEngine.Tilemaps;


namespace BuildingSystem
{
    public class CursorLayer : TileMapLayer
    {
        [SerializeField] private SpriteRenderer cursorSpriter;

        public void ShowCursor(Vector3 worldCoords)
        {
            Debug.Log("월드 좌표: " + worldCoords);
            var coords = tileMap.WorldToCell(worldCoords);
            Debug.Log("변환 좌표: " + coords);
            cursorSpriter.transform.position = tileMap.CellToWorld(coords) + Vector3.one * 0.5f;
        }
    }
}
