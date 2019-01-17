using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {
    public PlayerHealth playerHealth;
    public GameObject backButton;
    public Text moneyText;

    Animator anim;                          
                     

    void Awake() {
      
        anim = GetComponent<Animator>();
        
        backButton.SetActive(false);
    }


    void Update() {
        
        if (playerHealth.currentHealth <= 0) {
            anim.SetTrigger("GameOver");
            Cursor.visible = true;
            StartCoroutine(setBackButtonActive());
            moneyText.text = "You have earnt: " + (ScoreManager.score/10) + " cash";
        }
    }

    public void returnButton() {
        Debug.Log("Back button pressed");
        SceneManager.LoadScene("CustomisationScreen");
    }

    IEnumerator setBackButtonActive() {
        yield return new WaitForSeconds(3f);
        backButton.SetActive(true);
    }
}