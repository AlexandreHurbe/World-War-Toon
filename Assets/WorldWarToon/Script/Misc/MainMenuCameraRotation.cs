using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraRotation : MonoBehaviour {
    public float rotationSpeed = 5f;
    public GameObject rotateAroundObject;

    Vector3 rotationMask = new Vector3(0, 1, 0);
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.RotateAround(rotateAroundObject.transform.position, rotationMask, rotationSpeed*Time.deltaTime);
	}
}
