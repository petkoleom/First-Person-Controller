using UnityEngine;

public class scr_PlayerHealth : scr_PlayerBehaviour, itf_Damageable
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

    public void TakeDamage(float damage)
    {

    }

    public void TakeFallDamage()
    {
        float fallDistance = startOfFall - transform.localPosition.y;
        if(fallDistance > minFallDistance)
        {
            health -= fallDistance * 3;
            //if (health <= 0)
                //Destroy(transform.root.gameObject);

        }
    }

    bool isFalling { get { return (!player.playerGround.isGrounded && player.rb.velocity.y < 0); } }
}
