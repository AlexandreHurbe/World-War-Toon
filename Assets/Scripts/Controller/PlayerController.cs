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
        
        playerCamera = GetComponentInChildren<PlayerCamera>();
        playerCamera.Init();

        playerMovement = GetComponentInChildren<PlayerMovement>();
        playerMovement.Init();
        //playerShooting = GetComponentInChildren<PlayerShooting>();



        //playerShooting.Init();
        isInit = true;
    }

    private void FixedUpdate()
    {
        playerMovement.FixedTick(Time.fixedDeltaTime);
    }

    // Update is called once per frame
    private void Update()
    {
        playerCamera.Tick(Time.deltaTime);
        
    }

    private void LateUpdate()
    {
        playerMovement.LateTick();
    }
}
