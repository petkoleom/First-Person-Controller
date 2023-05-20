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

    public bool TakeDamage(float damage)
    {
        return false;
    }

    public void TakeFallDamage()
    {
        float fallDistance = startOfFall - transform.localPosition.y;
        if(fallDistance > minFallDistance)
        {
            health -= fallDistance * 3;

        }
    }

    bool isFalling { get { return (!player.playerGround.isGrounded && player.rb.velocity.y < 0); } }
}
