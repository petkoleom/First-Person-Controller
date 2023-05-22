using Unity.Netcode;
using UnityEngine;

public class scr_PlayerBehaviour : NetworkBehaviour
{
    [HideInInspector]
    public scr_Player Player;
    public virtual void Initialize(scr_Player _player)
    {
        Player = _player;
    }
}
