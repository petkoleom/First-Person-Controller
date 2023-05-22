using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerMantle : scr_PlayerBehaviour
{
    [SerializeField]
    private Vector3 maxLedgeHeight = new Vector3(0, 2.5f, 0);
    [SerializeField]
    private float ledgeDetectionDistance = 1, mantleSpeed = 2;

    public bool CanMantle { get; private set; }

    private void Update()
    {
        LedgeDetection();

    }

    public void OnJump(InputValue _value)
    {
        if (CanMantle && Player.Ground.IsGrounded)
        {
            CanMantle = false;
            Player.Jump.DisableJump(.2f);
            StartCoroutine(Mantle(GetObstacleHeight()));
            
        }
    }

    private void LedgeDetection()
    {
        if (!Player.Ground.IsGrounded) return;
        RaycastHit _ledgeCheckHit;
        RaycastHit _obstacleHit;
        var _check1 = Physics.Raycast(transform.position + Vector3.up * 1.8f, Player.Orientation.forward, out _obstacleHit, ledgeDetectionDistance, Player.Ground.Ground);
        var _check2 = Physics.Raycast(transform.position+ Vector3.up * .7f, Player.Orientation.forward, out _obstacleHit, ledgeDetectionDistance, Player.Ground.Ground);
        var _check3 = Physics.Raycast(transform.position + Vector3.up * .2f, Player.Orientation.forward, out _obstacleHit, ledgeDetectionDistance, Player.Ground.Ground);

        if (_check1 || _check2 || _check3)
        {
            if (!Physics.Raycast(transform.position + maxLedgeHeight, Player.Orientation.forward, out _ledgeCheckHit, ledgeDetectionDistance, Player.Ground.Ground))
            {
                CanMantle = true;
                //print(GetObstacleHeight());
            }
            else
                CanMantle = false;
        }
        else
            CanMantle = false;
    }

    private float GetObstacleHeight()
    {
        float _currentPlayerHeight = transform.position.y;
        float[] _points = new float[3];

        RaycastHit _heightCheck;
        Vector3 _origin = transform.position + maxLedgeHeight;

        for (int i = 1; i < 4; i++)
        {
            Physics.Raycast(_origin + Player.Orientation.forward * (i * .3f), Vector3.down, out _heightCheck, 2.5f, Player.Ground.Ground);
            _points[i - 1] = _heightCheck.point.y;
        }

        float _height = 0;
        foreach (var _point in _points)
        {
            if(_point > _height)
                _height = _point;
        }


        return _height - _currentPlayerHeight;
    }

    private IEnumerator Mantle(float _height)
    {
        Vector3 _forward = Player.Orientation.forward;

        if (_height < 1.5f)
            Player.Rb.AddForce(Vector3.up * mantleSpeed * _height * 1.4f, ForceMode.VelocityChange);
        else if(_height > 2.2f)
            Player.Rb.AddForce(Vector3.up * mantleSpeed * _height * .8f, ForceMode.VelocityChange);
        else
            Player.Rb.AddForce(Vector3.up * mantleSpeed * _height * .9f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(.17f);
        Player.Rb.AddForce(_forward * mantleSpeed * .3f, ForceMode.VelocityChange);

    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + Vector3.up * .7f, Player.Orientation.forward);
    }

}
