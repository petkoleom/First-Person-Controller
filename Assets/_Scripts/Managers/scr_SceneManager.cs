using UnityEngine.SceneManagement;

public class scr_SceneManager : PersistentSingleton<scr_SceneManager>
{
    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
