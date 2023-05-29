using System;
using BuildingSystem.Models;
using GameInput;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildingPlacer : MonoBehaviour
    {
        public event Action ActiveBuildableChanged;
        
        [field: SerializeField]
        public BuildableItem activeBuildable { get; private set; }
        
        [SerializeField] private float maxBuildingDistance = 100f; // 자유롭게 설정 가능
        [SerializeField] private ConstructionLayer constructionLayer;
        [SerializeField] private MouseUser mouseUser;

        [SerializeField]
        private PreviewLayer previewLayer;
        [SerializeField]
        private CursorLayer cursorLayer;

        private void Update()
        {
            var mousePos = mouseUser.MouseInWorldPosition;
            
            cursorLayer.ShowCursor(mousePos);
            if (!IsMouseWithinBuildableRange() || constructionLayer == null)
            {
                previewLayer.ClearPreview(); // 설치할 수 있는 범위 밖으로 나가면 설치 끄기
                return;
            }
            
            if (mouseUser.IsMouseButtonPressed(MouseButton.Right))
            {
                constructionLayer.Destroy(mousePos);
            }
            
            if (activeBuildable == null) return;

            var isSpaceEmpty = constructionLayer.IsEmpty(mousePos,
                activeBuildable.useCustomCollisionSpace ? activeBuildable.CollisionSpace : default);
            
            previewLayer.ShowPreview(activeBuildable, mousePos, isSpaceEmpty);

            if (mouseUser.IsMouseButtonPressed(MouseButton.Left) && isSpaceEmpty)
            {
                constructionLayer.Build(mousePos, activeBuildable);
            }
        }

        private bool IsMouseWithinBuildableRange()
        {
            return Vector3.Distance(mouseUser.MouseInWorldPosition, transform.position) <= maxBuildingDistance;
        }

        public void SetActiveBuildable(BuildableItem item)
        {
            activeBuildable = item;
            ActiveBuildableChanged?.Invoke();
        }
    }
}
