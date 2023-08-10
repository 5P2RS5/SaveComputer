using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingSystem.Models
{
    public enum TowerType
    {
        Wall, Basic, Ice, Laser, Missile
    }
    [CreateAssetMenu(menuName = "Building/New Buildable Item", fileName = "New Buildable Item")] // ScriptableObject이기 떄문에 사용
    public class BuildableItem : ScriptableObject
    {
        [field: SerializeField] // 지을 건물 이름
        public string name { get; private set; }
        
        [field: SerializeField] // 지을 건물 가격
        public int price { get; private set; } // 음수 : 구매 불가(Can't Buy)
        
        [field: SerializeField] // 지을 건물 팔기
        public int sell { get; private set; } // 음수 : 팔기 불가(Can't Sell)
        
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
        
        [field: SerializeField]
        public Vector3Int coords { get; set;}
        
        [field: SerializeField] // 지을 건물 체력
        public int init_hp { get; private set; } // 음수 : 무적(infinity)

        [field: SerializeField] // 지을 건물 데미지
        public int init_damage { get; private set; } // 음수 : 공격불가(Can't Attack)
        
        [field: SerializeField] // 공격 속도
        public float init_period { get; private set; } // 음수 : 공격불가(Can't Attack)
        
        [field: SerializeField] // 공격 속도
        public float init_ammoSpeed { get; private set; } // 음수 : 공격불가(Can't Attack)
        
        // [field: SerializeField]
        // public bool CanAttack { get; private set; }
        
        [field: SerializeField]
        public TowerType type { get; private set; }

        [field: SerializeField]
        public Sprite bulletSprite { get; private set; }
        
        [field: SerializeField] // 지을 건물 체력
        public int hp { get; set; } // 음수 : 무적(infinity)

        [field: SerializeField] // 지을 건물 데미지
        public int damage { get; set; } // 음수 : 공격불가(Can't Attack)
        
        [field: SerializeField] // 공격 속도
        public float period { get; set; } // 음수 : 공격불가(Can't Attack)
        
        [field: SerializeField] // 공격 속도
        public float ammoSpeed { get; set; } // 음수 : 공격불가(Can't Attack)
        
        [field: SerializeField] // 공격 속도
        public float scanRange { get; set; } // 음수 : 공격불가(Can't Attack)

        [field: SerializeField] 
        public string rangetext { get; private set; }
        
        [field: SerializeField] 
        public string description { get; private set; }
        
        [field: SerializeField] 
        public Sprite rangeSprite { get; private set; }
        
        [field: SerializeField] 
        public Sprite descriptionSprite { get; private set; }
        
        [field: SerializeField]
        public AudioClip hitSound { get; private set; }
        
    }
}
