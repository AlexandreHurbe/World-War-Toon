using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesManager : MonoBehaviour
{

    // These are the possible states for the character to be in
    public enum CharacterState
    {
        normal,
        inAir,
        cover,
        vaulting
    }

    //These are the possible states of the character to be in
    [System.Serializable]
    public class ControllerStates
    {
        public bool onGround;
        public bool isAiming;
        public bool isCrouching;
        public bool isProning;
        public bool isSprinting;
        public bool isInteracting;
    }

    public void FixedTick(float d)
    {

    }

    public void Tick(float d)
    {

    }
}
