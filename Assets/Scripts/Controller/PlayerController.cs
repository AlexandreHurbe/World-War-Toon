using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum GamePhase
    {
        inGame,
        inMenu,
        inInventory
    }

    //PRIVATE COMPONENTS \\
    private PlayerCamera playerCamera;
    private PlayerShooting playerShooting;
    private PlayerMovement playerMovement;


    //Checks if all components of the player are have started
    private bool isInit;


    // Start is called before the first frame update
    void Start()
    {
        InitInGame();
    }

    void InitInGame()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponent<PlayerCamera>();
        playerShooting = GetComponent<PlayerShooting>();

        playerMovement.Init();
        playerCamera.Init();
        playerShooting.Init();
        isInit = true;
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        playerCamera.Tick(Time.deltaTime);
    }
}
