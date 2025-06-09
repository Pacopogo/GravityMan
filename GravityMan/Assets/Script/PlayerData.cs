using UnityEngine;

[System.Serializable]
public class PlayerData : IPlayerInputs
{
    [Header("Health")]
    public float MaxHealth = 5;
    public float Health;

    [Header("Movement")]
    public Collider2D Collider;
    public Rigidbody2D PlayerBody;
    public float GravityPull = 2;
    private bool gravityFlip = false;

    public void FlipGravity(KeyCode[] key)
    {
        foreach (KeyCode keyCode in key)
        {
            if(!Input.GetKeyDown(keyCode))
                continue;

            gravityFlip = !gravityFlip;
            PlayerBody.linearVelocityY = 0;

            PlayerBody.gravityScale = gravityFlip ? GravityPull : -GravityPull;
            return;
        }
    }

    public void PauseGame()
    {
        //change game state
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
    }

}
