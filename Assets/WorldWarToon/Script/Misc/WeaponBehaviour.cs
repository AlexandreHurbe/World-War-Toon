using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour {
    //THIS CLASS IS INHERITED BY ALL GUNS, THIS IS STILL A WORK IN PROGRESS


    //PROTECTED VARIABLES\\
    protected LineRenderer aimLine;
    protected AudioSource gunAudio;
    protected Light gunLight;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void fireWeapon()
    {

        Debug.Log("Weapon Fired");
        gunAudio.Play();
        StartCoroutine(weaponFlash());
        
    }

    //Draws a line from gun's end point to mouse cursor
    public void drawAimLine(Vector3 mousePosition)
    {
        setAimLine(true);
        aimLine.SetPosition(0, this.transform.position);
        aimLine.SetPosition(1, mousePosition);
    }

    public void setAimLine(bool enabled)
    {
        aimLine.enabled = enabled;
    }

    //Plays a flash on tip of gun for 0.5f this is for testing purposes
    IEnumerator weaponFlash()
    {
        gunLight.enabled = true;
        yield return new WaitForSeconds(0.5f);
        gunLight.enabled = false;
    }


}
