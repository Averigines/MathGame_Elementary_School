using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TouchManager : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void OnTap()
    {
        if (Touchscreen.current != null)
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
        }
    }
}
