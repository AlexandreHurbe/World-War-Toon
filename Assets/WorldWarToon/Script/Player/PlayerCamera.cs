using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    // PUBLIC VARIABLES \\
    //Target to follow
    public Camera instantiateCamera;
    //Limits the distance the player can pan away from character
    public Vector4 panLimit;
    //Mouse sensitivity for cursor
    public float mouseSensitivity = 10f;
    //How fast the camera rotates
    public float rotationSpeed = 50f;
    //How fast the the player can zoom in and out
    public float scrollSpeed = 2f;
    //Smoothing factor for camera 
    public float smoothing = 15f;


    //PRIVATE COMPONENTS \\ 
    //PlayerShooting component 
    private PlayerShooting playerShooting;
    private WeaponBehaviour weaponBehaviour;


    //  PRIVATE VARIABLES  \\
    //The Camera object associated with the player
    private Camera playerCamera;
    //The initial rotation of the camera
    private Vector3 cameraRotation = new Vector3(70, 0, 0);
    //Initial offset of camera from player 
    private Vector3 offset = new Vector3(0, 18, -6);
    //Mouse position in screen space
    private Vector3 mousePosition;
    //Mouse position in world space
    private Vector3 mouseWorldPosition;
    //How much the camera is offsetted from player
    private Vector3 panOffset;
    //Distance of camera from Player
    //private Vector3 cameraDistance;
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
    //View distance factor given by gun
    private float viewDist = 1;
    


    public void Awake() {
        
    }

    private void Start() {

        //Disables cursor so we can use our own custom mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Gets shooting component
        playerShooting = GetComponent<PlayerShooting>();

        weaponBehaviour = GetComponentInChildren<WeaponBehaviour>();

        //Sets our custom mouse to middle of the screen
        mousePosition.x = Screen.width / 2;
        mousePosition.y = Screen.height / 2;
        mousePosition.z = 0;

        //Creating player camera
        playerCamera = Instantiate(instantiateCamera, this.transform.position + offset, Quaternion.Euler(cameraRotation));



        //Gets the floor layer so all raycast are to this floor 
        floorMask = LayerMask.GetMask("CanRayCast");
        isRotating = false;

        
        
    }


    private void Update() {
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");



        /* This is for user zooming in and out using scroll wheel */
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            offset = playerCamera.transform.position;
            offset.y -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            //Clamps the distance of how close and how far the user can zoom in/out
            offset.y = Mathf.Clamp(offset.y, cameraDistanceMin, cameraDistanceMax);
            offset.x = ((offset.y / 3) * (-Mathf.Sin(Mathf.Deg2Rad * (playerCamera.transform.eulerAngles.y))));
            offset.z = (((offset.y / 3) * (-Mathf.Cos(Mathf.Deg2Rad * (playerCamera.transform.eulerAngles.y)))));
        }


        //This allows the panning of camera but within fixed limits
        Ray camRay = playerCamera.ScreenPointToRay(mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(camRay, out rayHit, camRayLength, floorMask))
        {
            mouseWorldPosition = rayHit.point;
            //mouseWorldPosition.y = 1;

            //This needs to worked on so that players can aim at players on the ground or perhaps 
            //Debug.Log(rayHit.collider.GetComponent<MeshRenderer>().bounds.size.y);
            if (!rayHit.collider.tag.Equals("Shootable"))
            {
                mouseWorldPosition.y = 1f;
                
            }
            else
            {
                mouseWorldPosition.y = rayHit.collider.GetComponent<MeshRenderer>().bounds.size.y;
            }


            panOffset = mouseWorldPosition - this.transform.position;

            //The pan offset needs to be finetuned. It has just been left out for now
            //panOffset.x = (mouseWorldPosition.x - target.transform.position.x + offset.x);
            //panOffset.z = (mouseWorldPosition.z - target.transform.position.z + offset.z);
            //panOffset = panOffset / 10;

            //Debug.Log("Before: " + panOffset);
            //panOffset.x = Mathf.Clamp(panOffset.x, panLimit.x, panLimit.z);
            //panOffset.z = Mathf.Clamp(panOffset.z, panLimit.y, panLimit.w);
            //Debug.Log("After: " + panOffset);

        }

        
        //This is the rotation around the player
        if (Input.GetMouseButton(PlayerInputCustomiser.RotateCamera) && mouseX != 0)
        {
            isRotating = true;
            rotateCamera(mouseX);
        }

        //When the player releases middle mouse click
        if (Input.GetMouseButtonUp(PlayerInputCustomiser.RotateCamera))
        {
            isRotating = false;
        }


        //If the player is not rotating the camera then we want the cursor to follow. Otherwise we want the cursor to maintain a fixed position on the screen
        if (!isRotating) {
            mousePosition.x += mouseX * mouseSensitivity;
            mousePosition.y += mouseY * mouseSensitivity;
        }

        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, this.transform.position + offset + (panOffset / viewDist), smoothing * Time.deltaTime);
        

    }

    //Calculates rotation values for camera rotation and calculates the offset
    private void rotateCamera(float mouseX)
    {
        
        playerCamera.transform.eulerAngles = new Vector3(playerCamera.transform.eulerAngles.x, playerCamera.transform.eulerAngles.y + rotationSpeed * mouseX * Time.deltaTime, playerCamera.transform.eulerAngles.z);
        float x = ((playerCamera.transform.position.y / 3) * (-Mathf.Sin(Mathf.Deg2Rad * (playerCamera.transform.eulerAngles.y))));
        float z = (((playerCamera.transform.position.y / 3) * (-Mathf.Cos(Mathf.Deg2Rad * (playerCamera.transform.eulerAngles.y)))));
        offset = new Vector3(x, playerCamera.transform.position.y, z);
        offset.y = Mathf.Clamp(offset.y, cameraDistanceMin, cameraDistanceMax);
        
    }


    public Camera getPlayerCamera()
    {
        return playerCamera;
    }

    public Vector3 getMouseWorldPosition()
    {
        return mouseWorldPosition;
    }

    public void setViewDist(float viewDistValue)
    {
        //Debug.Log("viewDistValue: " + viewDistValue);
        this.viewDist = viewDistValue;
        //Debug.Log("this.viewDist: " + this.viewDist);
    }


    private void OnGUI() {
        float height = 10f;
        float width = 2f;
        float spread = 10f;
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.Apply();

        mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Screen.height);
        mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Screen.width);

        if (!playerShooting.getIsAiming())
        {
            //This displays the mouse cursor on UI. Will be improved later but for testing purposes works fine
            GUI.Box(new Rect(mousePosition.x - 5, Screen.height - (mousePosition.y + 5), 10, 10), "");

        }
        else
        {
            //This creates a really crappy cross hair this will either need to be done using UI/a canvas. This is done when player is aiming
            //up rect
            GUI.DrawTexture(new Rect(mousePosition.x - width / 2, ((Screen.height - mousePosition.y) - height), width, height), texture);

            //down rect
            GUI.DrawTexture(new Rect(mousePosition.x - width / 2, (Screen.height - mousePosition.y), width, height), texture);

            //left rect
            GUI.DrawTexture(new Rect(mousePosition.x - height, (Screen.height - mousePosition.y), height, width), texture);

            //right rect
            GUI.DrawTexture(new Rect((mousePosition.x), (Screen.height - mousePosition.y), height, width), texture);
        }


    }




}
