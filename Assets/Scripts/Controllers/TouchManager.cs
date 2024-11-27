using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TouchManager : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private Player player;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnTap()
    {
        if (Touchscreen.current == null) return;
        TouchControl touch = Touchscreen.current.primaryTouch;
        if (IsTouchOverUI(touch)) return;
        
        Vector2 touchPos = touch.position.ReadValue();
        Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, _mainCamera.nearClipPlane));

        if (worldPoint.y < ScreenData.backgroundArea.top && worldPoint.y > ScreenData.backgroundArea.bottom &&
            player.ValidStatesForMoving.Contains(player.CurrPlayerState))
        {
            player.SetTargetPosition(worldPoint.x);
        }
        
        else if (worldPoint.y < ScreenData.seaArea.top && worldPoint.y > ScreenData.seaArea.bottom &&
            player.ValidStatesForFishing.Contains(player.CurrPlayerState))
        {
            player.StartFishing();
        }
    }

    private void OnPress()
    {
        if (Touchscreen.current == null) return;

        TouchControl touch = Touchscreen.current.primaryTouch;
        Vector2 touchPos = touch.position.ReadValue();
        Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, _mainCamera.nearClipPlane));
        
        if (worldPoint.y < ScreenData.seaArea.top && worldPoint.y > ScreenData.seaArea.bottom &&
            player.CurrPlayerState == Player.PlayerState.Fishing)
        {
            player.StartAcceleratingFishingRod();
        }
    }

    private void OnReleasePress()
    {
        if (Touchscreen.current == null) return;
        
        TouchControl touch = Touchscreen.current.primaryTouch;
        Vector2 touchPos = touch.position.ReadValue();
        Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, _mainCamera.nearClipPlane));
        
        if (worldPoint.y < ScreenData.seaArea.top && worldPoint.y > ScreenData.seaArea.bottom &&
            player.CurrPlayerState == Player.PlayerState.Fishing)
        {
            player.StartDeceleratingFishingRod();
        }
    }
    
    private bool IsTouchOverUI(TouchControl touch)
    {
        return EventSystem.current.IsPointerOverGameObject(touch.ReadValue().touchId);
    }
}
