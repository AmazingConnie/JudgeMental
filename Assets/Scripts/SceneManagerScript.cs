using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour
{
    public void LoadSceneNeeded(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
