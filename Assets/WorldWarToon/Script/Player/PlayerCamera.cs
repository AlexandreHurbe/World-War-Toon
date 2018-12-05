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
    private Vector3 rotationMask = new Vector3(0, 1, 0);
    //Bool to check if camera is still rotating
    private bool isRotating;
    //The invisible floor in game scene used for raycasting
    private int floorMask;
    //Distance to raycast
    private float camRayLength = 100f;



    public void Awake() {
        //Disables cursor so we can use our own custom mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Sets our custom mouse to middle of the screen
        mousePosition.x = Screen.width / 2;
        mousePosition.y = Screen.height / 2;
        mousePosition.z = 0;
    }

    private void Start() {
        this.transform.position = target.transform.position + offset;
        cameraDistance = this.transform.position;
        floorMask = LayerMask.GetMask("Floor");
        isRotating = false;
        
    }


    private void Update() {
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");



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


        Ray camRay = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit floorHit;

        //This allows the panning of camera but within fixed limits
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 mouseWorldPosition = floorHit.point;
            mouseWorldPosition.y = target.transform.position.y;

            panOffset = mouseWorldPosition - target.transform.position;
            //panOffset.x = (mouseWorldPosition.x - target.transform.position.x + offset.x);
            //panOffset.z = (mouseWorldPosition.z - target.transform.position.z + offset.z);
            //panOffset = panOffset / 10;

            //Debug.Log("Before: " + panOffset);
            //panOffset.x = Mathf.Clamp(panOffset.x, panLimit.x, panLimit.z);
            //panOffset.z = Mathf.Clamp(panOffset.z, panLimit.y, panLimit.w);
            //Debug.Log("After: " + panOffset);

        }

        //Sets the rotation point for rotation
        if (Input.GetMouseButtonDown(2)) {
            rotationPoint = target.transform.position;
            Debug.Log("Offset: " + offset);
            Debug.Log("PanOffset: " + panOffset);
        }

        //This is the rotation around the player
        if (Input.GetMouseButton(2) /*&& mouseX != 0*/) {
            isRotating = true;
            rotateCamera(mouseX);
            //transform.RotateAround(rotationPoint, rotationMask, rotationSpeed * mouseX * Time.deltaTime);
            //offset = this.transform.position - target.transform.position;
        }

        //When the player releases middle mouse click
        if (Input.GetMouseButtonUp(2)) {
            Debug.Log("Middle mouse has been released");
            Debug.Log("Offset: " + offset);
            Debug.Log("PanOffset: " + panOffset);
            
            isRotating = false;
        }


        //If the player is not rotating the camera then we want the cursor to follow. Otherwise we want the cursor to maintain a fixed position on the screen
        if (!isRotating) {
            mousePosition.x += mouseX * mouseSensitivity;
            mousePosition.y += mouseY * mouseSensitivity;
        }

        this.transform.position = target.transform.position + offset + (panOffset / 10);

        ////Makes sure the camera ratios are maintained
        //if (isRotating)
        //{
        //    this.transform.position = target.transform.position + offset;
        //}
        //else
        //{
        //    this.transform.position = target.transform.position + offset + (panOffset / 10);
        //    //this.transform.position = target.transform.position + offset + panOffset;
        //}
        //this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + offset, 5f * Time.deltaTime);

    }

    private void rotateCamera(float mouseX)
    {
        this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + rotationSpeed * mouseX * Time.deltaTime, this.transform.localEulerAngles.z);
        float x = ((cameraDistance.y / 3) * (-Mathf.Sin(Mathf.Deg2Rad * (this.transform.eulerAngles.y))));
        float z = (((cameraDistance.y / 3) * (-Mathf.Cos(Mathf.Deg2Rad * (this.transform.eulerAngles.y)))));
        this.transform.position = new Vector3(x, this.transform.position.y, z);
        offset = this.transform.position - target.transform.position;
    }



    private void OnGUI() {
        //This displays the mouse cursor on UI. Will be improved later but for testing purposes works fine
        GUI.Box(new Rect(mousePosition.x-5, Screen.height - (mousePosition.y+5), 10, 10), "");





    }


}
