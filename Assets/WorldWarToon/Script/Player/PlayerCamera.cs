using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    public GameObject target;
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector4 panLimit;
    public float rotationSpeed = 50f;
    public float scrollSpeed = 2f;
    public float smoothing = 5f;
    public float rotationSmoothing;

    private Vector3 offset = new Vector3(0, 18, -6);
    private Vector3 panOffset;
    private Vector3 cameraDistance;
    private Vector3 rotationPoint;
    private float cameraDistanceMin = 6f;
    private float cameraDistanceMax = 18f;
    Vector3 rotationMask = new Vector3(0, 1, 0);
    private bool isRotating;
    private bool continueZoom;

    int floorMask;
    float camRayLength = 100f;

    private void Start() {
        this.transform.position = target.transform.position + offset;
        cameraDistance = this.transform.position;
        floorMask = LayerMask.GetMask("Floor");
        isRotating = false;
        
    }


    private void Update() {

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) {
            Vector3 mousePosition = floorHit.point;
            mousePosition.y = target.transform.position.y;
            panOffset = (mousePosition - target.transform.position);
            //Debug.Log(((mousePosition - target.transform.position)/10)+target.transform.position);
        }




        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        /* This is for user zooming in and out using scroll wheel */
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            cameraDistance = this.transform.position;
            cameraDistance.y -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            cameraDistance.y = Mathf.Clamp(cameraDistance.y, cameraDistanceMin, cameraDistanceMax);
            cameraDistance.x = ((cameraDistance.y / 3) * (-Mathf.Sin(Mathf.Deg2Rad * (this.transform.eulerAngles.y))));
            cameraDistance.z = (((cameraDistance.y / 3) * (-Mathf.Cos(Mathf.Deg2Rad * (this.transform.eulerAngles.y)))));
            offset = cameraDistance;
        }

        if (Input.GetMouseButtonDown(2)) {
            rotationPoint = target.transform.position + panOffset;
        }

        /* This is the rotation around the player */
        if (Input.GetMouseButton(2) && mouseX != 0) {
            isRotating = true;
            transform.RotateAround(rotationPoint, rotationMask, rotationSpeed * mouseX * Time.deltaTime);
            offset = this.transform.position - target.transform.position;
        }

        if (Input.GetMouseButtonUp(2)) {
            isRotating = false;
        }
        
        ///*This allows users to extend their view also needs to be worked on*/
        //if (mouseX != 0 || mouseY != 0) {

        //Vector3 pos = transform.position;
        //pos.z += mouseY * 8f * Time.deltaTime;
        //pos.x += mouseX * 8f * Time.deltaTime;

        //pos.x = Mathf.Clamp(pos.x, panLimit.x, panLimit.z);
        //pos.z = Mathf.Clamp(pos.z, panLimit.y, panLimit.w);

        //transform.position = pos;

        //Vector3 pos = transform.position;
        //offset.z += mouseY * 5f * Time.deltaTime;
        //offset.x += mouseX * 5f * Time.deltaTime;

        //offset.x = Mathf.Clamp(offset.x, target.transform.position.x + panLimit.x, target.transform.position.x + panLimit.z);
        //offset.z = Mathf.Clamp(offset.z, target.transform.position.z + panLimit.y, target.transform.position.z + panLimit.w);

        //}

        if (isRotating) {
            this.transform.position = target.transform.position + offset;
        }
        else {
            this.transform.position = target.transform.position + offset + (panOffset / 10);
        }
        //this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + offset, 5f * Time.deltaTime);
    }

    
    private void LateUpdate() {
        //float currentAngle = transform.eulerAngles.y;
        //float desiredAngle = target.transform.eulerAngles.y;
        //float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * rotationSmoothing);

        //Quaternion rotation = Quaternion.Euler(0, angle, 0);
        //transform.position = target.transform.position - (rotation * offset);
        //this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + offset, 5f * Time.deltaTime);

        //this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + offset, 5f * Time.deltaTime);
        
        
        

        /*if (needsCenter) {
            this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + offset, 0.5f * Time.deltaTime);
        }*/

    }
    

}
