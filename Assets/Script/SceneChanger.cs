using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static void SceneChange(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
public enum SceneType
{
    Title = 0,
    Ingame = 1,
    Result = 2,
}