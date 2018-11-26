using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    // PUBLIC VARIABLES \\
    //Target to follow
    public GameObject target;
    //Limits the distance the player can pan away from character
    public Vector4 panLimit;
    //Mouse sensitivity for cursor
    public float mouseSensitivity = 10f;
    //How fast the camera rotates
    public float rotationSpeed = 50f;
    //How fast the the player can zoom in and out
    public float scrollSpeed = 2f;
    //Smoothing factor for camera (Not currently in use)
    public float smoothing = 5f;


    //  PRIVATE VARIABLES  \\
    //Initial offset of camera from player
    private Vector3 offset = new Vector3(0, 18, -6);
    //Mouse position
    private Vector3 mousePosition;
    //How much the camera is offsetted from player
    private Vector3 panOffset;
    //Distance of camera from Player
    private Vector3 cameraDistance;
    //Point of rotation when player is rotating
    private Vector3 rotationPoint;
    //Closest distance camera can get to player
    private float cameraDistanceMin = 6f;
    //Furhtest distance camera can get to player
    private float cameraDistanceMax = 18f;
    //Which axis to rotate about
    private Vector3 rotationMask = Vector3.up;
    //Bool to check if camera is still rotating
    private bool isRotating;
    //The invisible floor in game scene used for raycasting
    private int floorMask;
    //Distance to raycast
    private float camRayLength = 100f;
    //whether the camera has rotated
    private bool rotated;


    private void Start() {
        this.transform.position = target.transform.position + offset;
        cameraDistance = this.transform.position;
        floorMask = LayerMask.GetMask("Floor");
        isRotating = false;
        rotated = false;

    }


    private void Update() {
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // get my mouse position from the MouseCursor Script
        mousePosition = MouseCursor.mousePosition;


        /* This is for user zooming in and out using scroll wheel */
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            cameraDistance = this.transform.position;
            cameraDistance.y -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            //Clamps the distance of how close and how far the user can zoom in/out
            cameraDistance.y = Mathf.Clamp(cameraDistance.y, cameraDistanceMin, cameraDistanceMax);
            cameraDistance.x = ((cameraDistance.y / 3) * (-Mathf.Sin(Mathf.Deg2Rad * (this.transform.eulerAngles.y))));
            cameraDistance.z = (((cameraDistance.y / 3) * (-Mathf.Cos(Mathf.Deg2Rad * (this.transform.eulerAngles.y)))));
            offset = cameraDistance;
        }

        //Sets the rotation point for rotation
        if (Input.GetMouseButtonDown(2)) {
            rotationPoint = target.transform.position;
            rotated = false;
        }

        //This is the rotation around the player
        if (Input.GetMouseButton(2) && mouseX != 0) {
            isRotating = true;
            
            transform.RotateAround(rotationPoint, rotationMask, rotationSpeed * mouseX * Time.deltaTime);
            offset = this.transform.position - target.transform.position;
            rotated = true;
            
        }

        //When the player releases middle mouse click
        if (Input.GetMouseButtonUp(2) && rotated) {
            isRotating = false;
            offset -= panOffset / 10;
        }

        Ray camRay = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit floorHit;
        
        //This allows the panning of camera but within fixed limits
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) {
            Vector3 mouseWorldPosition = floorHit.point;
            mouseWorldPosition.y = target.transform.position.y;
            panOffset = (mouseWorldPosition - target.transform.position);
            if (panOffset.x < panLimit.x || panOffset.x > panLimit.z) {
                Debug.Log("Too far in x");
            }
            if (panOffset.z < panLimit.y || panOffset.z > panLimit.w) {
                Debug.Log("Too far in z");
            }
            panOffset.x = Mathf.Clamp(panOffset.x, panLimit.x, panLimit.z);
            panOffset.z = Mathf.Clamp(panOffset.z, panLimit.y, panLimit.w);
            
        }

       
        //Makes sure the camera ratios are maintained
        if (isRotating) {
            this.transform.position = target.transform.position + offset;
        }
        else {
            this.transform.position = target.transform.position + offset + (panOffset / 10);
        }
        //this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + offset, 5f * Time.deltaTime);

    }


 


}
