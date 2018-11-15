using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    public Transform target;
    public float smoothing = 5f;
    public float rotationSmoothing;
    private Vector3 offset;

    private void Start() {
        this.transform.position = new Vector3(target.position.x, target.position.y + 12, target.position.z-10);
        offset = target.position - transform.position;
    }

    /*
    private void FixedUpdate() {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        Quaternion reformedTargetRotation = target.rotation;
        //reformedTargetRotation.z = 0;
        //reformedTargetRotation.y = target.rotation.y;
        //reformedTargetRotation.x = 55;
        transform.rotation = reformedTargetRotation;
        //reformedTargetRotation.y = transform.rotation.y;
        //transform.rotation = Quaternion.Lerp(transform.rotation, reformedTargetRotation, Time.deltaTime);
    }*/

    private void LateUpdate() {
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * rotationSmoothing);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.transform.position - (rotation * offset);

        transform.LookAt(target.transform);

    }
}
