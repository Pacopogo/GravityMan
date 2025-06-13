using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData : IPlayerInputs
{
    [SerializeField] private Animator anim;

    [Header("UI Components")]
    [SerializeField] private Slider healthSlider;

    [Header("Health")]
    public float MaxHealth = 5;
    public float Health;

    [Header("Movement")]
    public Collider2D Collider;
    public Rigidbody2D PlayerBody;
    public float GravityPull = 2;
    private float playerMoveDir;
    private bool gravityFlip = false;

    private CommandManager commandManager;

    private Action jumpAction;
    private Action pauseAction;

    public void PlayerStart()
    {
        commandManager = new CommandManager(this);

        jumpAction += FlipGravity;
        pauseAction += PauseToggle; 

        anim.SetBool("Flip", !gravityFlip);

        Health = MaxHealth;

        healthSlider.maxValue = MaxHealth;
        healthSlider.value = Health;

    }

    public void Jump(KeyCode[] key)
    {
        foreach (KeyCode keyCode in key)
        {
            if(!Input.GetKeyDown(keyCode))
                continue;

            commandManager.DoCommand(commandManager.Jump, jumpAction);

            return;
        }
    }

    public void PauseGame(KeyCode[] key)
    {
        foreach (KeyCode keyCode in key)
        {
            if (!Input.GetKeyDown(keyCode))
                continue;

            commandManager.DoCommand(commandManager.Pause, pauseAction);

            return;
        }
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;

        healthSlider.value = Health;

        if (Health <= 0)
            Death();
    }

    public void Heal(float heal)
    {
        Health += heal;

        if (Health > MaxHealth)
            Health = MaxHealth;

        healthSlider.value = Health;
    }

    public void PlayerUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        PlayerBody.linearVelocityY = playerMoveDir * Time.deltaTime;    
    }

    private void FlipGravity()
    {
        gravityFlip = !gravityFlip;
        PlayerBody.linearVelocityY = 0;

        playerMoveDir = gravityFlip ? GravityPull : -GravityPull;

        anim.SetBool("Flip", !gravityFlip);
    }

    private void PauseToggle()
    {
        Game.isPlaying = !Game.isPlaying;

        PlayerBody.simulated = Game.isPlaying;
    }

    private void Death()
    {
        SceneManager.LoadScene(0);
    }
}
