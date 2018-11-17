using System.Collections;
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
	void Update () {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Debug.Log("Horizontal: " + h);
        Debug.Log("Vertical: " + v);
        Move(h, v);
    }

    private void Move(float h, float v) {
        Vector3 movement = new Vector3 (0, 0, 0);
        
        if (v > 0) {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        movement.Set(h, 0, v);
        playerRigidBody.MovePosition(transform.position + (movement*Time.deltaTime*3f));
    }
}
