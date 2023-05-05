using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerMantle : scr_PlayerBehaviour
{
    [SerializeField]
    private Vector3 ledgeDetectionOffset = new Vector3(0, 2.5f, 0);
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
        if (canMantle)
        {
            player.playerJump.DisableJump(.2f);
            StartCoroutine(Mantle(GetObstacleHeight()));
        }
    }

    private void LedgeDetection()
    {
        if (!player.playerGround.isGrounded) return;
        RaycastHit ledgeCheckHit;
        RaycastHit obstacleHit;
        if (Physics.Raycast(transform.localPosition + Vector3.up * 1.8f, player.orientation.forward, out obstacleHit, ledgeDetectionDistance, player.playerGround.ground))
        {
            if (!Physics.Raycast(transform.localPosition + ledgeDetectionOffset, player.orientation.forward, out ledgeCheckHit, ledgeDetectionDistance, player.playerGround.ground))
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
        Vector3 origin = transform.localPosition + ledgeDetectionOffset + player.orientation.forward * ledgeDetectionDistance;
        Physics.Raycast(origin, Vector3.down, out heightCheck, 1, player.playerGround.ground);
        return heightCheck.point.y - currentPlayerHeight;
    }

    private IEnumerator Mantle(float height)
    {
        player.rb.AddForce(Vector3.up * mantleSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(.25f);
        player.rb.AddForce(player.orientation.forward * mantleSpeed * .2f, ForceMode.Impulse);

    }



}
