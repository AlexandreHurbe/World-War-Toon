using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
                              
    public float currentHealth;                                  
    public Slider healthSlider;                                
    public Image damageImage;                                   
    public AudioClip deathClip;                                 
    public float flashSpeed = 5f;                               
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     

    public ParticleSystem hitParticles;
    Animator anim;                                              
    AudioSource audioSource;                                    
    PlayerSoldierMovement playerMovement;                             
    PlayerSoldierShoot playerShooting;                            
    bool isDead;                                               
    bool damaged;                                              
    bool canHeal;

    private void Awake() {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerSoldierMovement>();
        playerShooting = GetComponent<PlayerSoldierShoot>();

        currentHealth = SoldierUpgrades.maxHealth;
        //UNCOMMENT THIS AFTERWARDS
        healthSlider.maxValue = currentHealth;
        healthSlider.value = currentHealth;
        canHeal = true;
    }


    void Update() {
        // If the player has just been damaged...
        if (damaged) {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;

        if (canHeal && currentHealth < SoldierUpgrades.maxHealth) {
            currentHealth += Time.deltaTime * (float)SoldierUpgrades.healthRegen;
            healthSlider.value = currentHealth;
        }
    }


    public void TakeDamage(int amount, Vector3 hitPoint) {
        
        damaged = true;

        StartCoroutine(pauseHealing());
        currentHealth -= amount;

        
        healthSlider.value = currentHealth;

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        // Play the hurt sound effect.
        audioSource.Play();

        
        if (currentHealth <= 0 && !isDead) {
            
            Death();
        }
    }

    IEnumerator pauseHealing() {
        canHeal = false;
        yield return new WaitForSeconds(10f);
        canHeal = true;
    }


    void Death() {
        // Set the death flag so this function won't be called again.
        isDead = true;
        // Turn off any remaining shooting effects.
        
        // Turn off the movement and shooting scripts.
        playerMovement.enabled = false;
        playerShooting.enabled = false;
        anim.SetBool(GetComponent<PlayerSoldierShoot>().currentWeapon.weaponName.ToString(), false);

        // Tell the animator that the player is dead.
        anim.SetTrigger("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        if (!audioSource.isPlaying) {
            audioSource.clip = deathClip;
            audioSource.Play();
        }
        

        SoldierUpgrades.money += (ScoreManager.score / 10);
        
    }
}
