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
        print("Tapping");
        if (Touchscreen.current != null && StateManager.ValidStatesForMoving.Contains(StateManager.CurrPlayerState))
        {
            TouchControl touch = Touchscreen.current.primaryTouch;
            Vector2 touchPos = touch.position.ReadValue();
            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, _mainCamera.nearClipPlane));

            StateManager.ChangePlayerState(StateManager.PlayerState.Moving);
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
        if (Touchscreen.current != null && StateManager.CurrPlayerState != StateManager.PlayerState.Fishing)
        {
            if (StateManager.CurrPlayerState != StateManager.PlayerState.AdjustingRodStrength)
            {
                StateManager.ChangePlayerState(StateManager.PlayerState.AdjustingRodStrength);
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
        print("Pressing");
        if (Touchscreen.current != null)
        {
            
        }
    }

    private void OnReleasePress()
    {
        print("Releasing");
        print("State on Release: " + StateManager.CurrPlayerState);
        if (Touchscreen.current != null && StateManager.CurrPlayerState == StateManager.PlayerState.AdjustingRodStrength)
        {
            player.ReleaseRod();
        }
    }
}
