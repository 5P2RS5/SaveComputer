using System;
using BuildingSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace GameInput
{
    public enum MouseButton
    {
        Left, 
        Right
    }

    public class MouseUser : MonoBehaviour
    {
        private InputActions inputActions;
        public Vector2 MousePos { get; private set; } // 마우스 위치
    
        public Vector2 MouseInWorldPosition => Camera.main.ScreenToWorldPoint(MousePos);
        // 스크린 상의 마우스 위치를 월드좌표로 MouseInWorldPosition에 리턴한다.

        private bool isLeftMouseButtonPressed;
        private bool isRightMouseButtonPressed;

        private void OnEnable()
        {
            inputActions = InputActions.Instance;
            inputActions.Player.MousePosition.performed += OnMousePositionPerformed;
            inputActions.Player.PerformAction.performed += OnPerformActionPerformed;
            inputActions.Player.PerformAction.canceled += OnPerformActionCanceled;
            inputActions.Player.CancelAction.performed += OnCancelActionPerformed;
            inputActions.Player.CancelAction.canceled += OnCancelActionCanceled;
        }
        // 즉, OnEnable() 메서드는 해당 컴포넌트가 활성화될 때 입력 액션과 관련된 이벤트 핸들러를 등록하는 작업을 수행합니다.
        // 이 경우에는 inputActions 인스턴스의 Player.MousePosition 액션이 발생할 때 OnMousePositionPerformed 메서드가 호출되도록 설정되었습니다.

        private void OnDisable()
        {
            inputActions.Player.MousePosition.performed -= OnMousePositionPerformed;
            inputActions.Player.PerformAction.performed -= OnPerformActionPerformed;
            inputActions.Player.PerformAction.canceled -= OnPerformActionCanceled;
            inputActions.Player.CancelAction.performed -= OnCancelActionPerformed;
            inputActions.Player.CancelAction.canceled -= OnCancelActionCanceled;
        }

        // CallbackContext: 클래스에는 입력 액션의 상태와 관련된 다양한 정보가 포함되어 있습니다. 이 정보에는 입력 액션의 상태 변화 여부, 입력 값을 읽어오는 메서드, 입력의 발생 시간, 입력 디바이스 등이 포함될 수 있습니다.
        private void OnMousePositionPerformed(InputAction.CallbackContext ctx)
        {
            MousePos = ctx.ReadValue<Vector2>();
        }

        private void OnPerformActionPerformed(InputAction.CallbackContext ctx)
        {
            isLeftMouseButtonPressed = true;
        }

        private void OnPerformActionCanceled(InputAction.CallbackContext ctx)
        {
            isLeftMouseButtonPressed = false;
        }

        private void OnCancelActionPerformed(InputAction.CallbackContext ctx)
        {
            isRightMouseButtonPressed = true;
        }

        private void OnCancelActionCanceled(InputAction.CallbackContext ctx)
        {
            isRightMouseButtonPressed = false;
        }

        public bool IsMouseButtonPressed(MouseButton button)
        {
            return button == MouseButton.Left ? isLeftMouseButtonPressed : isRightMouseButtonPressed;
        }
    }
}
