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

        if (!wasGrounded && Player.Ground.IsGrounded)
            TakeFallDamage();
        wasGrounded = Player.Ground.IsGrounded;
        wasFalling = isFalling;
    }

    public int TakeDamage(float _damage)
    {
        health -= _damage;
        if (health <= 0)
            Die();
        return 0;
    }

    public void TakeFallDamage()
    {
        float _fallDistance = startOfFall - transform.localPosition.y;
        if(_fallDistance > minFallDistance)
        {
            TakeDamage(_fallDistance * 3);

        }
    }

    private bool isFalling { get { return (!Player.Ground.IsGrounded && Player.Rb.velocity.y < 0); } }

    private void Die()
    {
        Player.PlayerManager.Respawn(Player.PlayerID);
        health = 100;
    }

}
