using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_GameManager : PersistentSingleton<scr_GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    void Start()
    {
        ChangeState(0);
    }

    public void ChangeState(int newState)
    {
        OnBeforeStateChanged?.Invoke((GameState)newState);

        State = (GameState)newState;
        switch (newState)
        {
            case (int)GameState.Menu:
                HandleMenu();
                break;
            case (int)GameState.StartPlaying:
                StartCoroutine(HandleStartPlaying());
                break;
            case (int)GameState.Playing:
                HandlePlaying();
                break;
            case (int)GameState.Paused:
                HandlePause();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke((GameState)newState);

        Debug.Log($"New state: {(GameState)newState}");
    }

    private void HandleMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            scr_SceneManager.Instance.ChangeScene(0);

    }

    private IEnumerator HandleStartPlaying()
    {
        scr_SceneManager.Instance.ChangeScene(1);

        while (SceneManager.GetActiveScene().buildIndex != 1)
        {
            yield return null;
        }

        ChangeState((int)GameState.Playing);
        
        scr_PlayerManager.Instance.AddPlayer();
    }

    private void HandlePlaying()
    {

    }

    private void HandlePause()
    {

    }

    private void HandleSpawn()
    {

    }

}

[Serializable]
public enum GameState
{
    Menu = 0,
    StartPlaying = 1,
    Playing = 2,
    Paused = 3,
}
