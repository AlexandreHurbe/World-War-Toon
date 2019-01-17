using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "Controller/Camera Values") ]
public class CameraValues : ScriptableObject
{


    //public float turnSmooth = 0.1f;
    //public float moveSpeed = 9;
    //public float aimSpeed = 25f;
    //public float yRotateSpeed = 8;
    //public float xRotateSpeed = 8;
    //public float minAngle = -35;
    //public float maxAngle = 35;
    //public float normalZ = -3f;
    //public float normalX;
    //public float aimX = 0;
    //public float aimZ = -0.5f;
    //public float normalY;
    //public float crouchY;
    //public float adaptSpeed;


    public Transform cameraPosition;

    //Mouse sensitivity for cursor
    public float mouseSensitivity = 10f;
    //Smoothing factor for camera 
    public float transitionSmoothing = 15f;
    public float cameraDistanceMin = 6f;
    public float cameraDistanceMax = 18f;
    public float scrollSpeed = 20f;
    public float camRayLength = 40f;
    public float panLimitHorizontal = 10f;
    public float panLimitVertical = 10f;
    public float panSmoothingFactor = 4f;
    //How fast the camera rotates
    public float rotationSpeed = 50f;

    
    public Vector2 mouseScreenPosition;
    public Vector3 mousePosition;

    //The initial rotation of the camera
    public Vector3 defaultCameraRotation = new Vector3(70, 0, 0);
    //Initial offset of camera from player 
    private Vector3 defaultCameraOffset = new Vector3(0, 18, -6);


    public Vector3 getDefaultCameraRotation()
    {
        return defaultCameraRotation;
    }

    public Vector3 getDefaultCameraOffset()
    {
        return defaultCameraOffset;
    }
}
