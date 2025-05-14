using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    //waiting on theo for gamemanager script
    public void resume()
    {
        //gamemanager.instance.stateUnpause();
    }

    public void restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //gamemanager.instance.stateUnpause();
    }

    public void OnApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }


}
