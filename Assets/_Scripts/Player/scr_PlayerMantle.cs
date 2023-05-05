using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerMantle : scr_PlayerBehaviour
{
    [SerializeField]
    private Vector3 maxLedgeHeight = new Vector3(0, 2.5f, 0);
    [SerializeField]
    private float ledgeDetectionDistance = 1;
    [SerializeField]
    private float mantleSpeed = 2;

    public bool canMantle;

    private void Update()
    {
        LedgeDetection();

    }

    public void OnJump(InputValue value)
    {
        if (canMantle && player.playerGround.isGrounded)
        {
            canMantle = false;
            player.playerJump.DisableJump(.2f);
            StartCoroutine(Mantle(GetObstacleHeight()));
            
        }
    }

    private void LedgeDetection()
    {
        if (!player.playerGround.isGrounded) return;
        RaycastHit ledgeCheckHit;
        RaycastHit obstacleHit;
        var check1 = Physics.Raycast(transform.localPosition + Vector3.up * 1.8f, player.orientation.forward, out obstacleHit, ledgeDetectionDistance, player.playerGround.ground);
        var check2 = Physics.Raycast(transform.localPosition + Vector3.up * .7f, player.orientation.forward, out obstacleHit, ledgeDetectionDistance, player.playerGround.ground);
        var check3 = Physics.Raycast(transform.localPosition + Vector3.up * .2f, player.orientation.forward, out obstacleHit, ledgeDetectionDistance, player.playerGround.ground);

        if (check1 || check2)
        {
            if (!Physics.Raycast(transform.localPosition + maxLedgeHeight, player.orientation.forward, out ledgeCheckHit, ledgeDetectionDistance, player.playerGround.ground))
            {
                canMantle = true;
                //print(GetObstacleHeight());
            }
            else
                canMantle = false;
        }
        else
            canMantle = false;
    }

    private float GetObstacleHeight()
    {
        float currentPlayerHeight = transform.localPosition.y;
        RaycastHit heightCheck;
        Vector3 origin = transform.localPosition + maxLedgeHeight + player.orientation.forward * ledgeDetectionDistance;
        Physics.Raycast(origin, Vector3.down, out heightCheck, 2.5f, player.playerGround.ground);


        return heightCheck.point.y - currentPlayerHeight;
    }

    private IEnumerator Mantle(float height)
    {
        if(height < 1.5f)
            player.rb.AddForce(Vector3.up * mantleSpeed * height * 1.4f, ForceMode.VelocityChange);
        else if(height > 2.2f)
            player.rb.AddForce(Vector3.up * mantleSpeed * height * .8f, ForceMode.VelocityChange);
        else
            player.rb.AddForce(Vector3.up * mantleSpeed * height * .9f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(.17f);
        player.rb.AddForce(player.orientation.forward * mantleSpeed * .3f, ForceMode.VelocityChange);

    }


}
