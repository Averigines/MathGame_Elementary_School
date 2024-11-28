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

        if (worldPoint.y < ScreenManager.backgroundArea.top && worldPoint.y > ScreenManager.backgroundArea.bottom &&
            player.ValidStatesForMoving.Contains(player.CurrPlayerState))
        {
            player.SetTargetPosition(worldPoint.x);
            return;
        }
        
        if (worldPoint.y < ScreenManager.seaArea.top && worldPoint.y > ScreenManager.seaArea.bottom &&
            player.ValidStatesForFishing.Contains(player.CurrPlayerState))
        {
            player.StartFishing();
            return;
        }
        
        if (player.CurrPlayerState == Player.PlayerState.Fishing)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Fish"))
                {
                    if (hit.transform.TryGetComponent<FishClusterContainer>(out FishClusterContainer cluster))
                    {
                        if (cluster.CanGetReeledIn())
                        {
                            cluster.GetReeledIn();
                            return;
                        } 
                    }
                }
            }
        }
        
        if (worldPoint.y < ScreenManager.seaArea.top && worldPoint.y > ScreenManager.seaArea.bottom &&
                 player.CurrPlayerState == Player.PlayerState.Fishing)
        {
            player.StopFishing(Player.PlayerState.Idle);
            return;
        }
    }

    private void OnPress()
    {
        if (Touchscreen.current == null) return;

        TouchControl touch = Touchscreen.current.primaryTouch;
        if (IsTouchOverUI(touch)) return;
        
        Vector2 touchPos = touch.position.ReadValue();
        Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, _mainCamera.nearClipPlane));
        
        if (worldPoint.y < ScreenManager.seaArea.top && worldPoint.y > ScreenManager.seaArea.bottom &&
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
        
        if (worldPoint.y < ScreenManager.seaArea.top && worldPoint.y > ScreenManager.seaArea.bottom &&
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
