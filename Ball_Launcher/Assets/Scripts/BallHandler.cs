using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BallHandler : MonoBehaviour
{
    // we need to delete the current serializefield as there is no ball in the scene
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay = 0.1f;
    [SerializeField] private float respawnDelay = 1.0f;
    private bool isDragging;
    private Camera mainCamera;
    private Rigidbody2D currentBallRigidbody2d;
    private SpringJoint2D currentBallSpringJoint;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        // Spawn the ball
        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        // If we dont have rigidbody attached to the script then exit update loop
        if(currentBallRigidbody2d == null) {
            Debug.LogError("Ball Rigidbody is not attched in inspector");
            return;}
        if(currentBallSpringJoint == null) {
           Debug.LogError("Ball Spring Joint is not attched in inspector");
            return;} 

        if(!Touchscreen.current.primaryTouch.press.isPressed)
        // When we released the touch
        {
            if(isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            return;
        }

        isDragging = true;
        currentBallRigidbody2d.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        // we will use camera to convert screen space to world space
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody2d.position = worldPosition; 
    }
    void LaunchBall()
    {
        currentBallRigidbody2d.isKinematic = false;
        currentBallRigidbody2d = null; // After releasing the ball, it wont snap back to the touch location

        Invoke(nameof(DetachBall), detachDelay);   
    }
    void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnBall), respawnDelay);
    }
    void SpawnBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
        // Fetching the ball rigidbody and spring joint
        currentBallRigidbody2d = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        // Attach the pivot to ball's spring joint 2d component
        currentBallSpringJoint.connectedBody = pivot;
    }
}
