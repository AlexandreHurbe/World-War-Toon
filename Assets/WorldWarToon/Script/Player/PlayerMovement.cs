﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    Rigidbody playerRigidBody;


    private void Awake() {
        playerRigidBody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Move(h, v);
    }

    private void Move(float h, float v) {
        Vector3 movement = new Vector3 (0, 0, 0);
        
        if (v > 0 && h > 0) {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y + 45, transform.localEulerAngles.z);
        }
        else if (v > 0 && h < 0) {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y - 45, transform.localEulerAngles.z);
        }
        else if (v < 0 && h > 0) {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y + 135, transform.localEulerAngles.z);
        }
        else if (v < 0 && h < 0) {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y + 225, transform.localEulerAngles.z);
        }
        else if (v > 0) {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);  
        }
        else if (v < 0) {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y + 180, transform.localEulerAngles.z);  
        }
        else if (h > 0) {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y + 90, transform.localEulerAngles.z);
        }
        else if (h < 0) {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y - 90, transform.localEulerAngles.z);
        }

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        movement = ((camRight.normalized * h) + (camForward.normalized * v)) * 3f * Time.deltaTime;
        movement.Set(movement.x, 0, movement.z);
        playerRigidBody.MovePosition(transform.position + movement);
    }
}
