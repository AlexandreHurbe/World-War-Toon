using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : ItemBehaviour {

    #region Gun Components Serialized
    //COMPONENTS NEEDED FOR GUN
    [SerializeField]
    protected GameObject bullet;
    [SerializeField]
    protected AudioClip gunMagazineEmptyAudio;
    [SerializeField]
    protected AudioClip gunReloadAudio;
    [SerializeField]
    protected ParticleSystem shellCasing;
    [SerializeField]
    protected ParticleSystem gunSmoke;
    [SerializeField]
    protected Transform gunEnd;
    [SerializeField]
    protected Transform LeftHandPos;
    #endregion

    #region Protected Components
    // PROTECTED COMPONENTS \\
    protected LineRenderer aimLine;
    protected AudioSource gunAudio;
    protected Light gunLight;
    #endregion

    #region Protected Variables
    // PROTECTED VARIABLES \\
    //Weapon name
    protected string weaponName;
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
    //Is it a pistol
    protected bool isPistol;
    #endregion

    #region Private Variables
    // PRIVATE VARIABLES \\
    //Checks whether or not the player can fire again
    private bool canFire = true;
    //The amount of ammo in the mag currently
    private int currentAmmoInMag;
    //THe total ammount of ammo the user is carrying right now
    private int currentTotalAmmo;
    //Whether or not the player is reloading when the gun mag is empty or not
    private bool emptyReload;
    //Checks if script is already reloading gun
    private bool isReloading;
    #endregion

    // Use this for initialization
    protected void Start() {

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
    void Update() {

    }

    public void fireWeapon()
    {
        //Debug.Log(canFire);
        //Debug.Log(currentAmmoInMag);
        if (canFire && currentAmmoInMag > 0)
        {
            if (shellCasing)
            {
                shellCasing.Stop();
                shellCasing.Play();
            }
            if (gunSmoke)
            {
                gunSmoke.Play();
            }
            
            StartCoroutine(fireRateCooldown());

            currentAmmoInMag--;
            //Debug.Log("Shot fired");
            //Debug.Log(currentAmmoInMag);
            gunAudio.Stop();
            gunAudio.Play();
            StartCoroutine(weaponFlash());
            GameObject newBullet = Instantiate(bullet, gunEnd.transform.position, gunEnd.transform.rotation);
            //Debug.Log(newBullet);
            BulletBehaviour bulletProperties = bullet.GetComponent<BulletBehaviour>();
            bulletProperties.setDmg((int)weaponDamage);
            bulletProperties.setPlayerFriendly(true);
            //bulletProperties.setSparkEffect(currentWeapon.sparkEffect);
            if (currentAmmoInMag == 0)
            {
                gunAudio.PlayOneShot(gunMagazineEmptyAudio, 0.5f);
            }
        }
        else if (currentAmmoInMag <= 0)
        {
            Reload();
            return;
        }


    }

    public bool returnIsPistol()
    {
        return isPistol;
    }

    public void Reload()
    {
        //Debug.Log("Current Total Ammo: " + currentTotalAmmo);
        //Debug.Log("Current Mag Ammo: " + currentAmmoInMag);
        
        if (chamberedRound)
        {
            //Checks if the magazine is full
            if (currentAmmoInMag != magSize + 1)
            {
                if (currentTotalAmmo > 0 && !isReloading)
                {
                    //Debug.Log("Reloading");
                    StartCoroutine(reloadTimer());
                }
                else if (isReloading)
                {
                    //Debug.Log("Currently reloading");
                    
                }
                else
                {
                    //Out of ammo completely should mention in UI
                    //Debug.Log("Out of ammo");
                }

            }
            else
            {
                return;
            }
        }
        else
        {
            //Checks if the magazine is full
            if (currentAmmoInMag != magSize)
            {
                if (currentTotalAmmo > 0 && !isReloading)
                {
                    //Debug.Log("Reloading");
                    StartCoroutine(reloadTimer());
                }
                else if (isReloading)
                {
                    //Debug.Log("Currently reloading");
                    
                }
                else
                {
                    //Out of ammo completely should mention in UI
                    //Debug.Log("Out of ammo");
                }

            }
            else
            {
                return;
            }
        }

        
    }

    public bool getReloadState()
    {
        return isReloading;
    }

    //Draws a line from gun's end point to mouse cursor
    public void drawAimLine(Vector3 mousePosition)
    {
        if (aimLine)
        {
            //setAimLine(true);
            Debug.DrawRay(this.transform.position, gunEnd.transform.forward * 100f, Color.red);
            aimLine.SetPosition(0, gunEnd.transform.position);
            aimLine.SetPosition(1, gunEnd.transform.position + (gunEnd.transform.forward * 100f));
            //aimLine.SetColors(Color.red, Color.red);
        }

    }

    public void setAimLine(bool enabled)
    {
        if (aimLine)
        {
            aimLine.enabled = enabled;
        }
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
        gunAudio.PlayOneShot(gunReloadAudio, 0.4f);
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


        if (emptyReload)
        {
            //Tops up total ammo stack with the bullets currently in the mag
            currentTotalAmmo += currentAmmoInMag;
        }
        else
        {
            //Tops up total ammo stack with the bullets currently in the mag minus the one bullet that is stuck inside the chamber
            currentTotalAmmo += (currentAmmoInMag - 1);
        }
        

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

        //Basically adds the chambered round into the magazine as it is not taken out when reloading.
        if (!emptyReload)
        {
            currentAmmoInMag++;
        }

    }

    public void alignGunEnd(Vector3 mousePosition)
    {
        
        Vector3 direction = (mousePosition - this.transform.position).normalized;
        this.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        gunEnd.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        
    }

    public float getViewDist()
    {
        //Debug.Log("Weapon behaviour view dist: " + this.viewDist);
        return this.viewDist;
    }

    public Transform getLeftHandPos()
    {
        return this.LeftHandPos;
    }

}
