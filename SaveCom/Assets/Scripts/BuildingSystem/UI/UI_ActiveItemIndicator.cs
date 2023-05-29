using System;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem.UI
{
    public class UI_ActiveItemIndicator : MonoBehaviour
    {
        [SerializeField] private BuildingPlacer buildingPlacer;

        private Image icon;

        private void Awake()
        {
            icon = GetComponent<Image>();
            buildingPlacer.ActiveBuildableChanged += OnActiveBuildableChanged;
        }

        private void Start()
        {
            OnActiveBuildableChanged();
        }

        private void OnActiveBuildableChanged()
        {
            if (buildingPlacer.activeBuildable != null)
            {
                icon.enabled = true;
                icon.sprite = buildingPlacer.activeBuildable.uiIcon;
            }
            else
            {
                icon.enabled = false;
            }
        }
    }
}
