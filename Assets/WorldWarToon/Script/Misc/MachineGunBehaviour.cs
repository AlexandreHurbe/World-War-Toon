using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBehaviour : MonoBehaviour {

    public GameObject weaponEnd;
    public GameObject bulletGameobject;
    public float fireRate;
    public float strayFactor;

    private bool canShoot = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canShoot) {
            StartCoroutine(shootSpeed());
            Fire();
        }
    }


    private void Fire() {
        float randomNumberX = Random.Range(-strayFactor, strayFactor);
        float randomNumberY = Random.Range(-strayFactor, strayFactor);
        float randomNumberZ = Random.Range(-strayFactor, strayFactor);
        GameObject bullet = Instantiate(bulletGameobject, weaponEnd.transform.position, weaponEnd.transform.rotation);
        BulletBehaviour bulletProperties = bullet.GetComponent<BulletBehaviour>();
        bulletProperties.setDestroyTime(0.8f);
        bullet.transform.Rotate(randomNumberX, randomNumberY, randomNumberZ);
        
    }

    IEnumerator shootSpeed() {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
