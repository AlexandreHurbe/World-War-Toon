using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    //Max allowed amount of enemies on screen
    public int startingMaxEnemies;
    private int maxEnemies;
    private int finalMaxEnemies = 30;

    //current amount of enemies
    public static int totalEnemiesStatic;
    public float spawnTime = 3f;
    [Tooltip("The higher value means tougher enemies will spawn sooner into the game")]
    public float difficultySetting;
    public GameObject player;
    public GameObject[] enemies;

    
    
    public Transform[] spawnPoints;
    public ParticleSystem bloodEffect;


    private PlayerHealth playerHealth;
    private float timer;
    private GameObject newEnemy;
    private int currentSpawns;
    private int enemyIndex;
    private int enemiesLength;
    private int totalEnemies;
    private bool addEnemies;
    // Use this for initialization
    void Start () {
        timer = spawnTime;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemiesLength = enemies.Length-1;
        //Debug.Log("Start function called in Spawn");
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        maxEnemies = startingMaxEnemies;
        addEnemies = true;
    }

    // Update is called once per frame
    void Update() {
        timer += (Time.deltaTime * difficultySetting);
        if (addEnemies) {
            if (maxEnemies <= finalMaxEnemies) {
                StartCoroutine(addMoreMaxEnemies());
                maxEnemies++;
            }
        }
        
        totalEnemies = totalEnemiesStatic;

    }

    IEnumerator addMoreMaxEnemies() {
        addEnemies = false;
        yield return new WaitForSeconds(10f);
        addEnemies = true;
    }

    private void Spawn() {
        if (totalEnemies >= maxEnemies) {
            
            return;
        }

        if (playerHealth.currentHealth <= 0f) {
            return;
        }

        //Max variable is a variable used multiple times for different purposes in the following functions.
        //i.e. used to set the amount enemies that can be spawned in this spawn cycle.
        //i.e used to set the the furthest gameObject in the enemy array;
        int maxVariable = (int)Mathf.Floor(timer);
        int currentSpawns = 0;
        foreach (Transform spawn in spawnPoints) {

            if (currentSpawns > maxVariable) {
                return;
            }

            float distanceSpawnPointPlayer = Vector3.Distance(spawn.position, player.transform.position);
            if ( 15f < distanceSpawnPointPlayer && distanceSpawnPointPlayer < 80f ) {
                enemyIndex = Mathf.Clamp(Random.Range(0, maxVariable), 0, enemiesLength);
                newEnemy = Instantiate(enemies[enemyIndex], spawn.position, spawn.rotation);
                newEnemy.GetComponent<EnemySoldierBehaviour>().setBloodEffect(this.bloodEffect);
                currentSpawns++;
                totalEnemies++;
            }
        }
        

    }
}
