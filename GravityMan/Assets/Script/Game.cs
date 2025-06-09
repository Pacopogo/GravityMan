using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    [SerializeField] private PlayerData player;
    [Space(20)]
    [SerializeField] private Obstacle[] obstacles;

    [SerializeField] private PlayerInputKeys keys;
    private IPlayerInputs inputs;


    private void Start()
    {
        inputs = player;

        //set Player hp
        player.Health = player.MaxHealth;

        //spawn 5 of each added object type
        InizializeObjects();
    }

    private void InizializeObjects()
    {
        
    }



    private void Update()
    {
        inputs.FlipGravity(keys.Jump);
        
    }

    
}
