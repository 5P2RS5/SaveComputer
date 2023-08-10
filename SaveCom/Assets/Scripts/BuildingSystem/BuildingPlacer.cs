using System;
using System.Collections;
using System.Collections.Generic;
using BuildingSystem.Models;
using GameInput;
using Unity.VisualScripting;
using UnityEngine;
using MouseButton = GameInput.MouseButton;
using Random = System.Random;

namespace BuildingSystem
{
    public class BuildingPlacer : MonoBehaviour
    {
        public event Action ActiveBuildableChanged;
        
        [field: SerializeField]
        public BuildableItem activeBuildable { get; private set; }
        
        [SerializeField] private float maxBuildingDistance = 2f; // 자유롭게 설정 가능
        [SerializeField] private ConstructionLayer constructionLayer;
        //[SerializeField] private MouseUser mouseUser;

        [SerializeField]
        private PreviewLayer previewLayer;

        [SerializeField] private GameObject previewObj;
        [SerializeField]
        private CursorLayer cursorLayer;

        private void Update()
        {
            var mousePos = GameSystem.instance.mouseUser.MouseInWorldPosition;

            // 설치 범위 제한 두기
            if (mousePos.x <= 0 || mousePos.x >= 10 || mousePos.y <= 0 || mousePos.y >= 10)
                return;
            cursorLayer.ShowCursor(mousePos);
            if (!IsMouseWithinBuildableRange() || constructionLayer == null)
            {
                previewLayer.ClearPreview(); // 설치할 수 있는 범위 밖으로 나가면 설치 끄기
                return;
            }
            
            if (GameSystem.instance.mouseUser.IsMouseButtonPressed(MouseButton.Left) && activeBuildable == null)
            {
                var isSpace = constructionLayer.IsEmpty(mousePos);
                if (isSpace)
                    return;
                GameSystem.instance.isOpenStatus = true;
                GameSystem.instance.towerStatusControl.coords = mousePos; // 마우스 좌표 설정
                GameSystem.instance.towerStatusControl.ViewTower();
                //GameManager.instance.mouseUser.inputActions.Disable(); // 
            }
            
            if (activeBuildable == null) return;

            var isSpaceEmpty = constructionLayer.IsEmpty(mousePos,
                activeBuildable.useCustomCollisionSpace ? activeBuildable.CollisionSpace : default);
            
            previewLayer.ShowPreview(activeBuildable, mousePos, isSpaceEmpty);

            if (GameSystem.instance.mouseUser.IsMouseButtonPressed(MouseButton.Left) && isSpaceEmpty)
            {
                constructionLayer.Build(mousePos, activeBuildable);
                SetInactiveBuildable();
            }
        }

        private bool IsMouseWithinBuildableRange()
        {
            return Vector3.Distance(GameSystem.instance.mouseUser.MouseInWorldPosition, transform.position) <= maxBuildingDistance;
        }

        public void SetActiveBuildable(BuildableItem item)
        {
            previewObj.SetActive(true);
            activeBuildable = item;
            ActiveBuildableChanged?.Invoke();
        }

        public void SetInactiveBuildable()
        {
            GameSystem.instance.isPick = false;
            previewObj.SetActive(false);
            activeBuildable = null;
        }
    }
}
