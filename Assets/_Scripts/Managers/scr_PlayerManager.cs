using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerManager : StaticInstance<scr_PlayerManager>
{
    [SerializeField]
    private GameObject playerPrefab;

    //public List<scr_Player> playerList = new List<scr_Player>();
    public Dictionary<int, scr_Player> Players = new Dictionary<int, scr_Player>();
    private int id = 0;

    public List<Transform> Spawnpoints = new List<Transform>();


    public void AddPlayer()
    {
        Players.Add(id ,Instantiate(playerPrefab).GetComponentInChildren<scr_Player>());
        Players.TryGetValue(id, out scr_Player _player);
        _player.InitializePlayer(this, id);
        _player.Respawn(GetSpawnpoint());
        id++;
    }

    public void Respawn(int _id)
    {
        StartCoroutine(RespawnPlayer(_id));
    }    
    
    private IEnumerator RespawnPlayer(int _id)
    {
        Players.TryGetValue(_id, out scr_Player _player);
        yield return new WaitForSeconds(0);
        _player.Respawn(GetSpawnpoint());

    }

    private Transform GetSpawnpoint()
    {
        int _idx = Random.Range(0, Spawnpoints.Count);
        return Spawnpoints[_idx];
    }
}
