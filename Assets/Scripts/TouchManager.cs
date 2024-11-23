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
    }
}
