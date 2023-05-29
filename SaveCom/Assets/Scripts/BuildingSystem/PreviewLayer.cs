using BuildingSystem.Models;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingSystem
{
    public class PreviewLayer : TileMapLayer
    {
        [SerializeField] private SpriteRenderer previewSpriter;

        public void ShowPreview(BuildableItem item, Vector3 worldCoords, bool isValid)
        {
            // isValid: 색으로 설치 가능 여부 확인
            
            // ex)
            // worldCoords: 7.2f, 13.2f
            // coords: 7, 13;
            // previewSpriter.transform.position: 7, 13
            
            var coords = tileMap.WorldToCell(worldCoords);
            previewSpriter.enabled = true;
           // previewSpriter.transform.position = tileMap.CellToWorld(coords) + tileMap.cellSize / 2 + item.tileOffset; 
            previewSpriter.transform.position = tileMap.CellToWorld(coords) + Vector3.one * 0.5f;
            previewSpriter.sprite = item.previewSprite;
            previewSpriter.color = isValid ? new Color(0.3f, 1f, 0.3f, 1f) : new Color(1, 0, 0, 1f);
        }
        
        public void ClearPreview()
        {
            previewSpriter.enabled = false;
        }
    }
}
