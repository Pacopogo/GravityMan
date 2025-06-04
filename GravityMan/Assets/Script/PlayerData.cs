using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("Health")]
    public float MaxHealth = 5;
    public float Health;

    [Header("Movement")]
    public Rigidbody2D PlayerBody;
    public float GravityPull = 2;
    private bool gravityFlip = false;


    public void PlayerUpdate()
    {
        //quick test if it would flip based on the input being here
        if (Input.GetKeyUp(KeyCode.Space))
            FlipGravity();
    }

    public void FlipGravity()
    {
        gravityFlip = !gravityFlip;
        PlayerBody.linearVelocityY = 0;

        PlayerBody.gravityScale = gravityFlip ? GravityPull : -GravityPull;
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
    }
}
