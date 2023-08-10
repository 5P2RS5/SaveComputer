using UnityEngine;

namespace VirusSystem.Model
{
    [CreateAssetMenu(menuName = "Virus/New Virus", fileName = "New Virus")] // ScriptableObject이기 떄문에 사용
    public class VirusInfo : ScriptableObject
    {
        [field: SerializeField] // 바이러스 이름
        public string name { get; private set; }
        
        [field: SerializeField] // 바이러스 체력
        public int hp { get; private set; }
        
        [field: SerializeField] // 바이러스 이동속도
        public float speed { get; private set; }
        
        [field: SerializeField] // 바이러스 공격력
        public int power { get; private set; }
        
        [field: SerializeField] // 바이러스 공격력
        public float attackPeriod { get; private set; }
        
        [field: SerializeField] // 바이러스 애니메이터
        public RuntimeAnimatorController animCon { get; private set; }

        [field: SerializeField] // 바이러스 타워 감지 범위
        public float scanRange { get; private set; }

        [field: SerializeField] // 바이러스 오브젝트
        public GameObject gameObject { get; private set; }
        
        [field: SerializeField] 
        public int cost { get; private set; }
        
        [field: SerializeField] // 바이러스 타입
        public VirusType Type { get; private set; }
    }
}
