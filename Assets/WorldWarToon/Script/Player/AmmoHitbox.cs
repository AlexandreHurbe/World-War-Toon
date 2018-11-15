using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AmmoHitbox : MonoBehaviour {

	//BoxCollider collider;

	void Awake(){
		//collider = GetComponent<BoxCollider>();
	}

	void onTriggerEnter(Collider collider){
		Debug.Log("Enter trigger");
    }

    void onTriggerStay(Collider collider){
		Debug.Log("Stay trigger");
    }

    void onCollisionExit(Collider collider){
		Debug.Log("Exit trigger");
    }

	void onCollisionEnter(Collision other){
		Debug.Log("Enter trigger");
    }

    void onCollisionStay(Collision other){
		Debug.Log("Stay trigger");
    }

    void onTriggerExit(Collision other){
		Debug.Log("Exit trigger");
    }


	

/* 
    void onCollisionEnter(Collision other){
        Debug.Log(other.gameObject.name);
    }*/
}
