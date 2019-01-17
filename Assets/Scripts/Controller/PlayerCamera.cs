using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {


    public CameraValues cameraValues;
    float mouseDeltaX;
    float mouseDeltaY;


    // PUBLIC VARIABLES \\
    //Target to follow
    [SerializeField]
    private Camera instantiateCamera;

    private PlayerShooting playerShooting;
    private WeaponBehaviour weaponBehaviour;

    //  PRIVATE VARIABLES  \\
    //The Camera object associated with the player
    private Camera playerCamera;
    //How much the camera is offsetted from player
    private Vector3 offset;
    //The offset of player when mouse is not in center of screen
    private Vector3 panOffset;
    //Mouse position in world
    private Vector3 mouseWorldPosition;
    
    
    //Bool to check if camera is still rotating
    private bool isRotating;
    //The invisible floor in game scene used for raycasting
    private int floorMask;
    
    


    public void Awake() {
        
    }

    public void Init() {

        //Disables cursor so we can use our own custom mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Gets shooting component
        playerShooting = GetComponent<PlayerShooting>();

        weaponBehaviour = GetComponentInChildren<WeaponBehaviour>();

        //Sets our custom mouse to middle of the screen
        cameraValues.mousePosition.x = Screen.width / 2;
        cameraValues.mousePosition.y = Screen.height / 2;
        cameraValues.mousePosition.z = 0;

        //Creating player camera
        playerCamera = Instantiate(instantiateCamera, this.transform.position + cameraValues.getDefaultCameraOffset(), Quaternion.Euler(cameraValues.getDefaultCameraRotation()));
        offset = cameraValues.getDefaultCameraOffset();
        cameraValues.cameraPosition = playerCamera.transform;


        //Gets the floor layer so all raycast are to this floor 
        floorMask = LayerMask.GetMask("CanRayCast");
        isRotating = false;

        
        
    }


    public void Tick(float d) {
        
        mouseDeltaX = Input.GetAxis("Mouse X") * cameraValues.mouseSensitivity;
        mouseDeltaY = Input.GetAxis("Mouse Y") * cameraValues.mouseSensitivity;



        HandleZoom();
        HandleRotation();
        HandlePan();

        //If the player is not rotating the camera then we want the cursor to follow. Otherwise we want the cursor to maintain a fixed position on the screen
        if (!isRotating) {
            //cameraValues.mousePosition.x += mouseDeltaX * cameraValues.mouseSensitivity;
            //cameraValues.mousePosition.y += mouseDeltaY * cameraValues.mouseSensitivity;
            cameraValues.mousePosition.x += mouseDeltaX;
            cameraValues.mousePosition.y += mouseDeltaY;
        }

        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, this.transform.position + offset + (panOffset/cameraValues.panSmoothingFactor), cameraValues.transitionSmoothing * Time.deltaTime);
        cameraValues.cameraPosition = playerCamera.transform;
    }

    private void HandleZoom()
    {
        /* This is for user zooming in and out using scroll wheel */
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            offset = playerCamera.transform.position;
            offset.y -= Input.GetAxis("Mouse ScrollWheel") * cameraValues.scrollSpeed;
            //Clamps the distance of how close and how far the user can zoom in/out
            offset.y = Mathf.Clamp(offset.y, cameraValues.cameraDistanceMin, cameraValues.cameraDistanceMax);
            offset.x = ((offset.y / 3) * (-Mathf.Sin(Mathf.Deg2Rad * (playerCamera.transform.eulerAngles.y))));
            offset.z = (((offset.y / 3) * (-Mathf.Cos(Mathf.Deg2Rad * (playerCamera.transform.eulerAngles.y)))));
        }

    }

    //This allows the panning of camera but within fixed limits
    private void HandlePan()
    {
        Ray camRay = playerCamera.ScreenPointToRay(cameraValues.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(camRay, out rayHit, cameraValues.camRayLength, floorMask))
        {
            mouseWorldPosition = rayHit.point;

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
            panOffset.x = Mathf.Clamp(panOffset.x, -cameraValues.panLimitHorizontal, cameraValues.panLimitHorizontal);
            panOffset.z = Mathf.Clamp(panOffset.z, -(cameraValues.panLimitVertical + 6f), cameraValues.panLimitVertical);

        }
    }

    private void HandleRotation()
    {
        //This is the rotation around the player
        if (Input.GetMouseButton(2) && mouseDeltaX != 0)
        {
            isRotating = true;
            rotateCamera(mouseDeltaX);
        }

        //When the player releases middle mouse click
        if (Input.GetMouseButtonUp(2))
        {
            isRotating = false;
        }
    }


    //Calculates rotation values for camera rotation and calculates the offset
    private void rotateCamera(float mouseX)
    {

        playerCamera.transform.eulerAngles = new Vector3(playerCamera.transform.eulerAngles.x, playerCamera.transform.eulerAngles.y + cameraValues.rotationSpeed * mouseX * Time.deltaTime, playerCamera.transform.eulerAngles.z);
        float x = ((playerCamera.transform.position.y / 3) * (-Mathf.Sin(Mathf.Deg2Rad * (playerCamera.transform.eulerAngles.y))));
        float z = (((playerCamera.transform.position.y / 3) * (-Mathf.Cos(Mathf.Deg2Rad * (playerCamera.transform.eulerAngles.y)))));
        offset = new Vector3(x, playerCamera.transform.position.y, z);
        offset.y = Mathf.Clamp(offset.y, cameraValues.cameraDistanceMin, cameraValues.cameraDistanceMax);

    }


    public Camera getPlayerCamera()
    {
        return playerCamera;
    }

    public Vector3 getMouseWorldPosition()
    {
        return mouseWorldPosition;
    }

    private void OnGUI() {
        float height = 10f;
        float width = 2f;
        float spread = 10f;
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.Apply();

        Vector2 mouseGUIPosition;

        mouseGUIPosition.y = Mathf.Clamp(cameraValues.mousePosition.y, 0, Screen.height);
        mouseGUIPosition.x = Mathf.Clamp(cameraValues.mousePosition.x, 0, Screen.width);

        if (!playerShooting.getIsAiming())
        {
            //This displays the mouse cursor on UI. Will be improved later but for testing purposes works fine
            GUI.Box(new Rect(mouseGUIPosition.x - 5, Screen.height - (mouseGUIPosition.y + 5), 10, 10), "");

        }
        else
        {
            //This creates a really crappy cross hair this will either need to be done using UI/a canvas. This is done when player is aiming
            //up rect
            GUI.DrawTexture(new Rect(mouseGUIPosition.x - width / 2, ((Screen.height - mouseGUIPosition.y) - height), width, height), texture);

            //down rect
            GUI.DrawTexture(new Rect(mouseGUIPosition.x - width / 2, (Screen.height - mouseGUIPosition.y), width, height), texture);

            //left rect
            GUI.DrawTexture(new Rect(mouseGUIPosition.x - height, (Screen.height - mouseGUIPosition.y), height, width), texture);

            //right rect
            GUI.DrawTexture(new Rect((mouseGUIPosition.x), (Screen.height - mouseGUIPosition.y), height, width), texture);
        }


    }




}
