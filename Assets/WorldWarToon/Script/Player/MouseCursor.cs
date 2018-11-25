using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour {
    //Mouse position
    public static Vector3 mousePosition;


    public float sensitivity = 10.0f;


    public void Awake()
    {
        //Disables cursor so we can use our own custom mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Sets our custom mouse to middle of the screen
        mousePosition.x = Screen.width / 2;
        mousePosition.y = Screen.height / 2;
        mousePosition.z = 0;
    }

    // Update is called once per frame
    void Update () {

        float movementX = Input.GetAxis("Mouse X");
        float movementY = Input.GetAxis("Mouse Y");


        // if the mouse has moved also mid button is not pressed
        // then move the mouse
        if (movementX + movementY != 0.0f && !Input.GetMouseButton(2))
        {
            moveMouse(movementX, movementY);
        }


    }


    private void moveMouse(float movementX, float movementY)
    {
        float min = 0.005f;
        float max = 0.995f;
        float mousePosX, mousePosY;
        // set the old mouse location
        mousePosX = mousePosition.x;
        mousePosY = mousePosition.y;
        // use movement to get new mouse location
        mousePosX += movementX * sensitivity;
        mousePosY += movementY * sensitivity;
        

        // limit mouse inside screen
        mousePosX = Mathf.Clamp(mousePosX, min * Screen.width, max * Screen.width);
        mousePosY = Mathf.Clamp(mousePosY, min * Screen.height, max * Screen.height);

        Debug.Log("mousePosX: " + mousePosX + " min: " + min * Screen.width);
        Debug.Log("mousePosY: " + mousePosY);
        mousePosition.x = mousePosX;
        mousePosition.y = mousePosY;
    }

    private void OnGUI()
    {
        //This displays the mouse cursor on UI. Will be improved later but for testing purposes works fine
        GUI.Box(new Rect(mousePosition.x , Screen.height - (mousePosition.y), 10, 10), "");


    }
}
