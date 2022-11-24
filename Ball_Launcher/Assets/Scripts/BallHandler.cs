using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BallHandler : MonoBehaviour
{
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Touchscreen.current.primaryTouch.press.isPressed)
        {
            return;
        }
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        // we will use camera to convert screen space to world space
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        Debug.Log(worldPosition);
    }
}
