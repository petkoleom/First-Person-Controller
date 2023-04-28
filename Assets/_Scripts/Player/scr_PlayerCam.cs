using UnityEngine;

public class scr_PlayerCam : MonoBehaviour
{
    private Transform camPos;

    private void Awake()
    {
        camPos = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2);
    }

    private void Update()
    {
        transform.position = camPos.position;
    }
}
