using UnityEngine;

public class scr_PlayerHealth : scr_PlayerBehaviour, itf_Damage
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float minFallDistance = 2;

    private bool wasGrounded;
    private bool wasFalling;
    private float startOfFall;

    private void FixedUpdate()
    {
        if (!wasFalling && isFalling)
            startOfFall = transform.localPosition.y;

        if (!wasGrounded && player.playerGround.isGrounded)
            TakeFallDamage();
        wasGrounded = player.playerGround.isGrounded;
        wasFalling = isFalling;
    }

    public int TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
        return 0;
    }

    public void TakeFallDamage()
    {
        float fallDistance = startOfFall - transform.localPosition.y;
        if(fallDistance > minFallDistance)
        {
            TakeDamage(fallDistance * 3);

        }
    }

    bool isFalling { get { return (!player.playerGround.isGrounded && player.rb.velocity.y < 0); } }

    private void Die()
    {
        player.playerManager.Respawn(player.PlayerID);
        health = 100;
    }

}
