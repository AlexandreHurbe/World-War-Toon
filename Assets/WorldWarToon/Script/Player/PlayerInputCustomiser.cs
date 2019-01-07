using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputCustomiser : MonoBehaviour {

    // Player Movement Inputs \\ 
    public static KeyCode Sprint = KeyCode.LeftShift;
    public static KeyCode Crouch = KeyCode.LeftControl;
    public static KeyCode Prone = KeyCode.C;

    // Player Shooting Inputs \\
    public static int Shoot = 0;
    public static int Aim = 1;
    public static KeyCode Reload = KeyCode.R;
    public static KeyCode Melee = KeyCode.F;


    //Player Camera Inputs \\
    public static int RotateCamera = 2;
    //public static idk mouseX = ??;
    //public static idk mouseY = ??;
    //public static idk Scrollwheel = ??;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
