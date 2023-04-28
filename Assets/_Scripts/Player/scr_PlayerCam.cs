using UnityEngine;

public class scr_PlayerCam : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.position = player.position + new Vector3(0, 1.5f, 0);
    }
}
