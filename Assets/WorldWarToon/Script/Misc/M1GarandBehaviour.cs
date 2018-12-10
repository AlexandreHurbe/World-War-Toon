using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1GarandBehaviour : WeaponBehaviour {

    private void Awake()
    {
        aimLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
