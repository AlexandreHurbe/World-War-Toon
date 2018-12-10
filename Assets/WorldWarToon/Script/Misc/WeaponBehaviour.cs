using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour {

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

    IEnumerator weaponFlash()
    {
        gunLight.enabled = true;
        yield return new WaitForSeconds(0.5f);
        gunLight.enabled = false;
    }


}
