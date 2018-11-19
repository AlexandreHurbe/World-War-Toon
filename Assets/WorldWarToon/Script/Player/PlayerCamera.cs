using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    public GameObject target;
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    public float rotationSpeed = 50f;
    public float scrollSpeed = 2f;
    public float smoothing = 5f;
    public float rotationSmoothing;

    private Vector3 offset = new Vector3(0, 18, -6);
    private float cameraDistance;
    private float cameraDistanceMin = 5f;
    private float cameraDistanceMax = 18f;
    Vector3 rotationMask = new Vector3(0, 1, 0);
    private bool isRotating;

    private void Start() {
        //offset = new Vector3(target.transform.position.x, target.transform.position.y + 18, target.transform.position.z - 6);
        this.transform.position = target.transform.position + offset;
        cameraDistance = this.transform.position.y;
        isRotating = false;
    }

    
    private void Update() {
        cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
        this.transform.position = new Vector3(this.transform.position.x, cameraDistance, this.transform.position.z);
        
        Vector3 pos = transform.position;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Debug.Log(isRotating);
        if (Input.GetMouseButton(2) && mouseX != 0) {
            isRotating = true;
            transform.RotateAround(target.transform.position, rotationMask, rotationSpeed * mouseX * Time.deltaTime);
            offset = this.transform.position - target.transform.position;
            return;
        }
        //pos.z += mouseY * Time.deltaTime;
        //pos.x += mouseX * Time.deltaTime;

        //pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        //pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
        
        if (Input.GetKeyDown(KeyCode.F)) {
            this.transform.position = target.transform.position + offset;
        }
    }

    
    private void LateUpdate() {
        //float currentAngle = transform.eulerAngles.y;
        //float desiredAngle = target.transform.eulerAngles.y;
        //float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * rotationSmoothing);

        //Quaternion rotation = Quaternion.Euler(0, angle, 0);
        //transform.position = target.transform.position - (rotation * offset);

        this.transform.position = target.transform.position + offset;
    }

    IEnumerator resetIsRotating() {
        yield return new WaitForSeconds(3f);
        isRotating = false;
    }

}
