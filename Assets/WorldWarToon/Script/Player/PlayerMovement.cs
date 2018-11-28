using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed;
    public float sprintSpeed;
    public float rotationSpeed = 30f;



    private Vector3 camForward;
    private Vector3 camRight;
    private Rigidbody playerRigidBody;
    private Animator anim;

    private bool isSprinting;

    private void Awake() {
        playerRigidBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        isSprinting = false;
    }

    // Use this for initialization
    void Start () {
        camForward = Camera.main.transform.forward;
        camRight = Camera.main.transform.right;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        camForward = Camera.main.transform.forward;
        camRight = Camera.main.transform.right;

        if (Input.GetKey(KeyCode.LeftShift)) {
            isSprinting = true;
        }
        else {
            isSprinting = false;
        }


        Move(h, v);
        Animate(h, v);
    }

    private void Animate(float h, float v) {
        if (h == 0 && v == 0) {
            anim.SetBool("isMoving", false);
            anim.SetFloat("TurnSpeed", 0);
        }
        else {
            if (isSprinting) {
                anim.SetBool("isSprinting", true);
            }
            else {
                anim.SetBool("isSprinting", false);
            }
            anim.SetBool("isMoving", true);
        }

        Vector2 animMovement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized;
        anim.SetFloat("Vertical", animMovement.y);
        anim.SetFloat("Horizontal", animMovement.x);
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
        if (isSprinting) {
            movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * sprintSpeed * Time.deltaTime;
        }
        else {
            movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * speed * Time.deltaTime;
        }
        
        playerRigidBody.MovePosition(transform.position + movement);






        //IGNORE ALL OF THIS FOR NOW
        /*
        float playerH = (this.transform.right * h).z;
        //Debug.Log(playerH);
        float playerV = (this.transform.forward * v).z;
        Debug.Log((this.transform.forward * v));
        //Debug.Log("H normalised: " + testH);
        //Debug.Log("V normalised: " + testV);


        //Several crappy implementations so that user can not rotate instantly but its not working right now
        
        if (playerV > 0 && playerH > 0) {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, Camera.main.transform.rotation.y + 45, transform.rotation.z));
        }
        else if (playerV > 0 && playerH < 0) {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, Camera.main.transform.rotation.y - 45, transform.rotation.z));
        }
        else if (playerV < 0 && playerH > 0) {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, Camera.main.transform.rotation.y + 135, transform.rotation.z));
        }
        else if (playerV < 0 && playerH < 0) {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, Camera.main.transform.rotation.y + 225, transform.rotation.z));
        }
        else if (playerV > 0) {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, Camera.main.transform.rotation.y, transform.rotation.z));
        }
        else if (playerV < 0) {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, Camera.main.transform.rotation.y + 180, transform.rotation.z));
        }
        else if (playerH > 0) {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, Camera.main.transform.rotation.y + 90, transform.rotation.z));
        }
        else if (playerH < 0) {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, Camera.main.transform.rotation.y - 90, transform.rotation.z));
        }

        
        if (v > 0 && h > 0) {
            Vector3 rotateAngle = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y + 45, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, rotateAngle, Time.deltaTime * rotationSpeed);
        }
        else if (v > 0 && h < 0) {
            Vector3 rotateAngle = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y - 45, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, rotateAngle, Time.deltaTime * rotationSpeed);
        }
        else if (v < 0 && h > 0) {
            Vector3 rotateAngle = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y + 135, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, rotateAngle, Time.deltaTime * rotationSpeed);
        }
        else if (v < 0 && h < 0) {
            Vector3 rotateAngle = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y + 225, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, rotateAngle, Time.deltaTime * rotationSpeed);
        }
        else if (v > 0) {
            Vector3 rotateAngle = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, rotateAngle, Time.deltaTime * rotationSpeed);
        }
        else if (v < 0) {
            Vector3 rotateAngle = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y + 180, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, rotateAngle, Time.deltaTime * rotationSpeed);
        }
        else if (h > 0) {
            Vector3 rotateAngle = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y + 90, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, rotateAngle, Time.deltaTime * rotationSpeed);
        }
        else if (h < 0) {
            Vector3 rotateAngle = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y - 90, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, rotateAngle, Time.deltaTime * rotationSpeed);
        }
        */
    }

    /*
    private void RotateTowards() {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    */

}
