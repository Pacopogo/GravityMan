using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData : IPlayerInputs
{
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

    public void PlayerStart()
    {
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

            gravityFlip = !gravityFlip;
            PlayerBody.linearVelocityY = 0;

            playerMoveDir = gravityFlip ? GravityPull : -GravityPull;

            return;
        }
    }

    public bool PauseGame(KeyCode[] key, bool isPaused)
    {
        
        //change game state
        foreach (KeyCode keyCode in key)
        {
            if (!Input.GetKeyDown(keyCode))
                continue;

            isPaused = !isPaused;

            return isPaused;
        }
        return isPaused;
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;

        healthSlider.value = Health;
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
        PlayerBody.transform.Translate(Vector3.down * playerMoveDir * Time.fixedDeltaTime);
    }
}
