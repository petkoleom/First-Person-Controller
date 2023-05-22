
using UnityEngine;

public class scr_PlayerCam : MonoBehaviour
{
    [SerializeField]
    private Transform camPos;


    private void Update()
    {
        transform.position = camPos.position;
    }
}
