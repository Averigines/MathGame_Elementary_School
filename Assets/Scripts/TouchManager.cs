using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        if (Touchscreen.current != null && player.ValidStatesForMoving.Contains(player.CurrPlayerState))
        {
            TouchControl touch = Touchscreen.current.primaryTouch;
            Vector2 touchPos = touch.position.ReadValue();
            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, _mainCamera.nearClipPlane));
            
            player.SetTargetPosition(worldPoint.x);
        }
        
        // OLD: TRANSFER TO NEW SECTION WHEN DONE
        /*if (Touchscreen.current != null)
        {
            TouchControl touch = Touchscreen.current.primaryTouch;
            Vector2 touchPos = touch.position.ReadValue();
            
            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, _mainCamera.nearClipPlane));
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Fish"))
                {
                    var fishCluster = hit.collider.gameObject.GetComponent<FishClusterContainer>();
                    fishCluster.GetReeledIn();
                }
            }
        }*/
    }

    private void OnDefineThrowStrength()
    {
        if (Touchscreen.current != null && player.CurrPlayerState != Player.PlayerState.Fishing)
        {
            if (player.CurrPlayerState != Player.PlayerState.AdjustingRodStrength)
            {
                player.StartRodUse();
            }
            
            var strengthChangeInPixels = Touchscreen.current.primaryTouch.delta.x.ReadValue();
            float screenWidth = Screen.width;
            float strengthChange = strengthChangeInPixels / screenWidth;
            player.ChangeRodStrength(strengthChange);
        }
    }

    private void OnPress()
    {
        if (Touchscreen.current != null)
        {
            
        }
    }

    private void OnReleasePress()
    {
        if (Touchscreen.current != null && player.CurrPlayerState == Player.PlayerState.AdjustingRodStrength)
        {
            player.ReleaseRod();
        }
    }
}
