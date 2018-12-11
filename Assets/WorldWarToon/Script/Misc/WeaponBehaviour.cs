using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour {
    //THIS CLASS IS INHERITED BY ALL GUNS, THIS IS STILL A WORK IN PROGRESS


    // PROTECTED COMPONENTS \\
    protected LineRenderer aimLine;
    protected AudioSource gunAudio;
    protected Light gunLight;

    // PROTECTED VARIABLES \\
    //The rate in which the gun can fire, the higher the value the slower it is;
    protected float fireRate;
    //The amount of damage the weapon does
    protected float weaponDamage;
    //The amount of ammo the magazine carries
    protected int magSize;
    //The amount of total ammo the player can carry
    protected int totalAmmo;
    //The time it takes for user to reload
    protected float reloadTime;
    //Some guns leave a bullet in the chamber when it is not an empty reload
    protected bool chamberedRound;
    //Guns with scopes will allow players to view further and shoot further.
    protected float viewDist;
    

    // PRIVATE VARIABLES \\
    //Checks whether or not the player can fire again
    private bool canFire;
    //The amount of ammo in the mag currently
    private int currentAmmoInMag;
    //THe total ammount of ammo the user is carrying right now
    private int currentTotalAmmo;
    //Whether or not the player is reloading when the gun mag is empty or not
    private bool emptyReload;
    //Checks if script is already reloading gun
    private bool isReloading;

    // Use this for initialization
    protected void Start () {
        
    }
	
    protected void setCurrentAmmoInMag()
    {
        currentAmmoInMag = magSize;
    }

    protected void setCurrentTotalAmmo()
    {
        currentTotalAmmo = totalAmmo;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void fireWeapon()
    {
        if (canFire && currentAmmoInMag > 0)
        {
            StartCoroutine(fireRateCooldown());
            
            currentAmmoInMag--;
            Debug.Log("Shot fired");
            Debug.Log(currentAmmoInMag);
            gunAudio.Stop();
            gunAudio.Play();
            StartCoroutine(weaponFlash());
        }
        else if (currentAmmoInMag <= 0)
        {
            Reload();
            return;
        }
        
        
    }

    public void Reload()
    {
        Debug.Log("Current Total Ammo: " + currentTotalAmmo);
        Debug.Log("Current Mag Ammo: " + currentAmmoInMag);
        if (currentAmmoInMag != magSize+1)
        {
            if (currentTotalAmmo > 0 && !isReloading)
            {
                Debug.Log("Reloading");
                StartCoroutine(reloadTimer());
            }
            else if (isReloading)
            {
                Debug.Log("Currently reloading");
                //Out of ammo completely should mention in UI
            }
            else
            {
                Debug.Log("Out of ammo");
            }
            
        }
        else
        {
            return;
        }
    }



    //Draws a line from gun's end point to mouse cursor
    public void drawAimLine(Vector3 mousePosition)
    {
        //setAimLine(true);
        Debug.DrawRay(this.transform.position, this.transform.forward * 100f, Color.red);
        aimLine.SetPosition(0, this.transform.position);
        aimLine.SetPosition(1, mousePosition);
    }

    public void setAimLine(bool enabled)
    {
        aimLine.enabled = enabled;
    }

    //Makes sure the next bullet can only be fired x amount of time after first one
    IEnumerator fireRateCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    //Plays a flash on tip of gun for 0.5f this is for testing purposes
    IEnumerator weaponFlash()
    {
        gunLight.enabled = true;
        yield return new WaitForSeconds(0.5f);
        gunLight.enabled = false;
    }

    IEnumerator reloadTimer()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;

        //Checks if it is an empty reload or not, by seeing how much ammo is in the mag
        //and if this type of gun allows a chambered round;
        if (currentAmmoInMag > 0 && chamberedRound)
        {
            emptyReload = false;
        }
        else
        {
            emptyReload = true;
        }

        //Tops up total ammo stack with the bullets currently in the mag
        currentTotalAmmo += currentAmmoInMag;

        //Checks if there is enough ammo to fill the gun up
        if (currentTotalAmmo >= magSize)
        {   
            //Fills gun back to its original size
            currentAmmoInMag = magSize;
            //Reduces the total ammount of ammo;
            currentTotalAmmo -= magSize;

        }
        else if (currentTotalAmmo < magSize && currentTotalAmmo > 0)
        {
            //Tops up the gun with the remaining ammo left
            currentAmmoInMag = currentTotalAmmo;
            //Reduces current total ammo to 0 since there should be none left
            currentTotalAmmo = 0;
        }

        if (!emptyReload)
        {
            currentAmmoInMag++;
        }

    }

    public float getViewDist()
    {
        return this.viewDist;
    }

}
