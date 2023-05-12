using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTags 
{
    public const string sphereTag = "Sphere"; //the name-tag attached to the sphere gameObject
    public const string playerTag = "Player"; //the name-tag attached to the Player gameObject
    public const string puzzleMan = "PuzzleManager"; //the name of the gameObject that has the PuzzleManager script
    public const string mainCam = "MainCamera"; //the name of the MainCamera gameObject
}

public class AnimTags
{
    public const string speed = "Speed";
    public const string grounded = "IsGrounded";
    public const string jump = "Jump";
    public const string fire = "Fire";
    public const string crouch = "Crouch";
}
