using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 0.5f;
    public int scoreValue = 10;
    public HurtSounds[] hurtSounds;

    Animator anim;
    AudioSource audioSource;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    EnemySoldierBehaviour enemyBehaviour;
    bool isDead;
    bool isSinking;


    [System.Serializable]
    public class HurtSounds {
        public AudioClip dieNoise;
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        enemyBehaviour = GetComponent<EnemySoldierBehaviour>();
        audioSource = GetComponent<AudioSource>();
        //hitParticles = GetComponentInChildren<ParticleSystem>();
        hitParticles = enemyBehaviour.getBloodEffect();
        capsuleCollider = GetComponent<CapsuleCollider>();
        

        currentHealth = startingHealth;
    }
	
	// Update is called once per frame
	void Update () {
        if (isSinking) {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage(int amount) {
        if (isDead)
            return;

        //enemyAudio.Play();

        currentHealth -= amount;
        enemyBehaviour.setTakeHit(true);

        //Debug.Log(currentHealth);
        if (currentHealth <= 0) {

            anim.SetBool(enemyBehaviour.weapon.weaponName.ToString(), false);
            Death();
        }
        else {
            anim.SetTrigger("isHit");
        }
    }

    //Overloaded method that takes in hit position to display particle effect
    public void TakeDamage(int amount, Vector3 hitPoint) {
        if (isDead)
            return;

        //enemyAudio.Play();

        currentHealth -= amount;
        enemyBehaviour.setTakeHit(true);
        
        //Debug.Log(currentHealth);


        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if (currentHealth <= 0) {
            ScoreManager.score += scoreValue;
            EnemyManager.totalEnemiesStatic -= 1;
            anim.SetBool(enemyBehaviour.weapon.weaponName.ToString(), false);
            Death();
        }
        else {
            anim.SetTrigger("isHit");
        }
    }

    
    


    void Death() {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger("Dead");
        StartCoroutine(StartSink());

        int selectedNumber = Random.Range(0, hurtSounds.Length - 1);
        audioSource.PlayOneShot(hurtSounds[selectedNumber].dieNoise);
    }

    IEnumerator StartSink() {
        yield return new WaitForSeconds(2f);
        StartSinking();
    }

    public void StartSinking() {
        
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        isSinking = true;
        Destroy(gameObject, 2f);
    }

    public void setHitParticle(ParticleSystem hitParticle){
        this.hitParticles = hitParticle;
    }

}
