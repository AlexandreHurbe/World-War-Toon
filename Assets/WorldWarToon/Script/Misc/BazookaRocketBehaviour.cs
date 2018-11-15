using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaRocketBehaviour : MonoBehaviour {

    public GameObject explosionEffect;
    public AudioClip explosionSound;

    private AudioSource audioSource;
    private int dmg = 100;
    private float bulletDetectInFrontRange = 0.3f;
    private float speed = 15f;
    private bool isPlayerFriendly;
    private float destroyTime = 7f;
    private bool hasExploded;
    private float explosionRadius = 10f;


    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
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


    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, bulletDetectInFrontRange)) {
            if (!audioSource.isPlaying){
                audioSource.PlayOneShot(explosionSound, 0.6f);
            }
            
            Explode();
        }
        else {
            this.transform.position += this.transform.forward * (speed * Time.smoothDeltaTime);
        }
    }

    private void Explode() {
        //Replace with explosion efect
        //Instantiate(explosionEffect, transform.position, transform.rotation);
        if (explosionEffect != null) {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders) {
            EnemyHealth enemyHealth = nearbyObject.transform.root.GetComponent<EnemyHealth>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(dmg);
                
            }

            
        }
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        Destroy(gameObject, destroyTime);
    }

}
