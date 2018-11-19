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
    private Vector3 cameraDistance;
    private float cameraDistanceMin = 6f;
    private float cameraDistanceMax = 18f;
    Vector3 rotationMask = new Vector3(0, 1, 0);
    private bool needsCenter;
    private bool continueZoom;

    private void Start() {
        this.transform.position = target.transform.position + offset;
        cameraDistance = this.transform.position;
        needsCenter = false;
        continueZoom = false;
    }

    
    private void Update() {


        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        /* This is for user zooming in and out using scroll wheel, the logic is wrong and needs to be worked on*/
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && mouseX == 0) {
            cameraDistance = this.transform.position;
            cameraDistance.y -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            cameraDistance.y = Mathf.Clamp(cameraDistance.y, cameraDistanceMin, cameraDistanceMax);
            cameraDistance.x = ((cameraDistance.y / 3) * (-Mathf.Sin(Mathf.Deg2Rad*(this.transform.eulerAngles.y))));
            cameraDistance.z = (((cameraDistance.y / 3) * (-Mathf.Cos(Mathf.Deg2Rad * (this.transform.eulerAngles.y)))));
            offset = cameraDistance;
        }

        //this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(cameraDistance.x, cameraDistance.y, cameraDistance.z), 5f * Time.deltaTime);
        //this.transform.position = cameraDistance;





        ///* This is the rotation around the player */
        if (Input.GetMouseButton(2) && mouseX != 0) {
            continueZoom = false;
            transform.RotateAround(target.transform.position, rotationMask, rotationSpeed * mouseX * Time.deltaTime);
            offset = this.transform.position - target.transform.position;
        }

        ///*This allows users to extend their view also needs to be worked on*/
        //else if (mouseX != 0 || mouseY != 0) {
        //    Vector3 pos = transform.position;
        //    //pos.z += mouseY * 2f * Time.deltaTime;
        //    //pos.x += mouseX * 2f * Time.deltaTime;

        //    //pos.x = Mathf.Clamp(pos.x, panLimit.x, panLimit.z);
        //    //pos.z = Mathf.Clamp(pos.z, panLimit.y, panLimit.w);
        //    needsCenter = true;
        //    //continueZoom = false;
        //    //transform.position = pos;
        //}




    }

    
    private void LateUpdate() {
        //float currentAngle = transform.eulerAngles.y;
        //float desiredAngle = target.transform.eulerAngles.y;
        //float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * rotationSmoothing);

        //Quaternion rotation = Quaternion.Euler(0, angle, 0);
        //transform.position = target.transform.position - (rotation * offset);
        //this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + offset, 5f * Time.deltaTime);

        //this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + offset, 5f * Time.deltaTime);
        
        this.transform.position = target.transform.position + offset;
        

        /*if (needsCenter) {
            this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + offset, 0.5f * Time.deltaTime);
        }*/

    }
    

}
