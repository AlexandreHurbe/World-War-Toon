using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySoldierBehaviour : MonoBehaviour {
    

    EnemyHealth enemyHealth;
    Animator anim;
    Transform player;
    PlayerHealth playerHealth;
    NavMeshAgent agent;
    AudioSource audioSource;
    //Rigidbody rigidBody;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    bool playerInRange = false;
    Vector3 movement;
    private float currentWeight = 0;
    private bool shouldAim = false;
    private bool canShoot = true;
    private bool aimReady = false;
    private bool isReloading = false;
    private int currentAmmo;
    private bool isHit;

    private ParticleSystem bloodEffect;

    public Transform spine;
    public WeaponDetails weapon;
    public float rotationSpeed;


    [System.Serializable]
    public class WeaponDetails {
        [Tooltip("Weapon Name Used for selecting wich animation to chose in animator")]
        public string weaponName;

        [Tooltip("Weapon object for switching out the weapon")]
        public GameObject WeaponGameobject;

        [Tooltip("Bullet GameObject you want to use - leave clear if you do not want one")]
        public GameObject bulletGameobject;

        [Tooltip("Weapon object for switching out the weapon")]
        public float switchOutTime = 0.2f;

        [Tooltip("The empty transform placed at the end of the gun, used all sorts of gun logic")]
        public Transform weaponEnd;

        [Tooltip("This is used by the left hand Ik for the character to have its left hand placed in the correct position on this gun")]
        public Transform leftHandPos;

        [Tooltip("How much health this will take away from enemys")]
        public int weaponDamage;

        [Tooltip("How far this weapon can hit the target")]
        public float weaponRange;

        [Tooltip("How fast you can shot the gun")]
        public float bulletSpeed;

        [Tooltip("Time interval between bullets")]
        public float fireRate;

        [Tooltip("Magazine/Clip Size")]
        public int magazineSize;

        [Tooltip("Total Ammo")]
        public int totalAmmo;

        [Tooltip("The time it takes to reload")]
        public float reloadTime = 1f;

        [Tooltip("Bullet Deviation")]
        public float strayFactor;

        [Tooltip("Which Key do you want to press to use the weapon")]
        public KeyCode switchWeaponKey;

        [Tooltip("Used to make the Soldier aim properly when holding the weapon")]
        public float aimingZ = -80f;

        [Tooltip("Used to make the Soldier aim properly when holding the weapon")]
        public float aimingY = -80f;

        [Tooltip("Used to make the Soldier aim properly when holding the weapon")]
        public float aimingX = -9f;

        [Tooltip("Weapon Sound should be added to this slot")]
        public AudioClip weaponAudio;
    }


    

    // Use this for initialization
    void Awake () {
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        audioSource = GetComponent<AudioSource>();
        //enemyHealth.setHitParticle(this.bloodEffect);
        //rigidBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();

        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        agent.updatePosition = false;
        //agent.updateRotation = false;
        currentAmmo = weapon.magazineSize;
        
    }
	
	// Update is called once per frame
	void Update () {
        

        try {
            if (isHit) {
                StartCoroutine(hitRecovery());
                shouldAim = false;
            }
            if (enemyHealth.currentHealth > 0 && isHit == false && playerHealth.currentHealth > 0) {
                agent.enabled = true;
                agent.destination = player.transform.position;
                /* The following code was taken from unity documentation */
                Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
                // Map 'worldDeltaPosition' to local space
                float dx = Vector3.Dot(transform.right, worldDeltaPosition);
                float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
                Vector2 deltaPosition = new Vector2(dx, dy);
                // Low-pass filter the deltaMove
                float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
                smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);
                // Update velocity if time advances
                if (Time.deltaTime > 1e-5f)
                    velocity = smoothDeltaPosition / Time.deltaTime;
                bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
                /* This is where it ends */

                if (shouldMove) {
                    shouldAim = false;
                    anim.SetFloat("Horizontal", velocity.x);
                    anim.SetFloat("Vertical", velocity.y);
                }
                else {
                    shouldAim = true;
                    RotateTowards();
                    anim.SetFloat("Horizontal", 0);
                    anim.SetFloat("Vertical", 0);
                }
                
                shootingMechanic();
            }

            else {
                agent.enabled = false;
            }
        }
        catch (System.Exception ) {
            agent.enabled = false;
            agent.enabled = true;
        }
        
    }

    IEnumerator hitRecovery() {
        yield return new WaitForSeconds(2f);
        isHit = false;
        shouldAim = true;
    }

    private void LateUpdate() {
        adjustSpineRotation();
    }

    public NavMeshAgent returnAgent() {
        return this.agent;
    }

    public void setTakeHit(bool shot) {
        this.isHit = shot;
    }

    private void RotateTowards() {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*rotationSpeed);
    }

    IEnumerator shootSpeed() {
        canShoot = false;
        yield return new WaitForSeconds(weapon.fireRate);
        canShoot = true;
    }

    IEnumerator aimSteady() {
        yield return new WaitForSeconds(0.2f);
        aimReady = true;
    }

    IEnumerator Reload() {
        isReloading = true;

        anim.SetBool("isReloading", isReloading);
        yield return new WaitForSeconds(weapon.reloadTime);
        isReloading = false;
        anim.SetBool("isReloading", isReloading);
        yield return new WaitForSeconds(0.5f);
        currentAmmo = weapon.magazineSize;
    }

    private void shootingMechanic() {
        //This function will be responsible for the AI to shoot
        //StartCoroutine(aimSteady());
        if (Vector3.Distance(player.transform.position, this.transform.position) > agent.stoppingDistance) {
            return;
        }

        if (shouldAim) {
            //adjustSpineRotation();
            if (aimReady == false) {
                StartCoroutine(aimSteady());
                currentWeight = Mathf.Lerp(currentWeight, 1, Time.deltaTime * 10);
                anim.SetLayerWeight(1, currentWeight);
                anim.SetBool(weapon.weaponName, true);
            }
            //Debug.Log(aimReady);
            if (canShoot && currentAmmo > 0 && aimReady) {
                StartCoroutine(shootSpeed());
                Fire();
            }
            else if(currentAmmo <= 0) {
                StartCoroutine(Reload());
            }
        }
        else {
            currentWeight = Mathf.Lerp(currentWeight, 0, Time.deltaTime * 10);
            anim.SetLayerWeight(1, currentWeight);
            anim.SetBool(weapon.weaponName, false);
            aimReady = false;
        }
        
    }

    private void adjustSpineRotation() {

        Vector3 eulerAngleOffset = Vector3.zero;
        eulerAngleOffset = new Vector3(weapon.aimingX, weapon.aimingY, weapon.aimingZ);
        spine.transform.Rotate(eulerAngleOffset, Space.Self);
    }

    private void Fire() {
        currentAmmo --;
        audioSource.Stop();
        audioSource.PlayOneShot(weapon.weaponAudio, 0.3f);
        float randomNumberX = Random.Range(-weapon.strayFactor, weapon.strayFactor);
        float randomNumberZ = Random.Range(-weapon.strayFactor, weapon.strayFactor);
        GameObject bullet = Instantiate(weapon.bulletGameobject, weapon.weaponEnd.transform.position, weapon.weaponEnd.transform.rotation);
        bullet.transform.Rotate(randomNumberX, 0, randomNumberZ);

        BulletBehaviour bulletProperties = bullet.GetComponent<BulletBehaviour>();
        bulletProperties.setDmg(weapon.weaponDamage);
        bulletProperties.setSpeed(12f);
        bulletProperties.setPlayerFriendly(false);
    }

    

    void OnAnimatorMove() {
        // Update position based on animation movement using navigation surface height
        //Vector3 position = anim.rootPosition;
        //position.y = agent.nextPosition.y;
        if (!isHit && playerHealth.currentHealth > 0) {
            transform.position = agent.nextPosition;
        }
    }

    public void setBloodEffect(ParticleSystem bloodEffect){
        this.bloodEffect = bloodEffect;
    }

    public ParticleSystem getBloodEffect(){
        return this.bloodEffect;
    }
}
