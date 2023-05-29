using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingSystem.Models
{
    [CreateAssetMenu(menuName = "Building/New Buildable Item", fileName = "New Buildable Item")] // ScriptableObject이기 떄문에 사용
    public class BuildableItem : ScriptableObject
    {
        [field: SerializeField] // 지을 건물 이름
        public string name { get; private set; }
        
        [field: SerializeField] // 기본 타일
        public TileBase tile { get; private set; }
        
        [field: SerializeField]
        public Vector3 tileOffset { get; private set; }
        
        [field: SerializeField]
        public Sprite previewSprite { get; private set; }
        
        [field: SerializeField]
        public Sprite uiIcon { get; private set; }

        [field: SerializeField] 
        public GameObject gameObject { get; private set; }
        
        [field: SerializeField]
        public bool useCustomCollisionSpace { get; private set; }
        
        [field: SerializeField]
        public RectInt CollisionSpace { get; private set; }
    }
}
