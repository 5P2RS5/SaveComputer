using System;
using System.Collections.Generic;
using BuildingSystem.Models;
using GameInput;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BuildingSystem
{
    public class BuildingSelector : MonoBehaviour
    {
        [SerializeField] private List<BuildableItem> buildables;
        [SerializeField] private BuildingPlacer BuildingPlacer;
        private int activeBuildableIndex;

        private void OnEnable()
        {
            //InputActions.Instance.Player.NextItem.performed += OnNextPerformed;
        }

        private void OnNextPerformed(InputAction.CallbackContext ctx)
        {
            NextItem();
        }

        private void NextItem()
        {
            activeBuildableIndex = (activeBuildableIndex + 1) % buildables.Count;
            BuildingPlacer.SetActiveBuildable(buildables[activeBuildableIndex]);
        }
    }
}
