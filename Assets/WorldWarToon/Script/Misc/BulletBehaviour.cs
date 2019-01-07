using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private int dmg = 10;
    private float bulletDetectInFrontRange = 1f;
    private float speed = 45f;
    private bool isPlayerFriendly;
    private float destroyTime = 2f;

    //private ParticleSystem bloodEffect;

    private ParticleSystem sparkEffect;

	// Use this for initialization
	void Start () {
        
		
	}
	
    public void setDmg(int dmg) {
        this.dmg = dmg;
    }

    public void setSpeed(float speed) {
        this.speed = speed;
    }

    public void setPlayerFriendly(bool isFriendly) {
        this.isPlayerFriendly = isFriendly;
    }

    public void setDestroyTime(float bulletLife) {
        this.destroyTime = bulletLife;
    }

    public void setSparkEffect(ParticleSystem sparkEffect){
        this.sparkEffect = sparkEffect;
    }

    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, bulletDetectInFrontRange)) {
            if (isPlayerFriendly) {
                EnemyHealth enemyHealth = hit.collider.transform.root.GetComponent<EnemyHealth>();
                if (enemyHealth != null) {
                    enemyHealth.TakeDamage(dmg, hit.point);
                    Destroy(gameObject);
                }
                else {
                    //This should create particle effects on the object that is hit;
                    // ParticleSystem sparkEffect = Instantiate(this.sparkEffect, 
                    //                                      gameObject.transform.position,
                    //                                      gameObject.transform.rotation);
                    if (sparkEffect != null) {
                        sparkEffect.transform.position = gameObject.transform.position;
                        sparkEffect.transform.rotation = gameObject.transform.rotation;
                        sparkEffect.Play();
                    }
                    Destroy(gameObject);
                }
            }
            else if (isPlayerFriendly == false) {
                PlayerHealth playerHealth = hit.collider.transform.root.GetComponent<PlayerHealth>();
                if (playerHealth != null) {
                    playerHealth.TakeDamage(dmg, hit.point);
                    Destroy(gameObject);
                }
                else {
                    //This should create particle effects on the object that is hit;
                    Destroy(gameObject);
                }
            }
            
            
        }
        else {
            this.transform.position += this.transform.forward * (speed * Time.smoothDeltaTime);
        }
        



        Destroy(gameObject, destroyTime);
	}
}
