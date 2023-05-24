using UnityEngine;

public class scr_PlayerBehaviour : MonoBehaviour
{
    [HideInInspector]
    public scr_Player Player;
    public virtual void Initialize(scr_Player _player)
    {
        Player = _player;
    }
}
