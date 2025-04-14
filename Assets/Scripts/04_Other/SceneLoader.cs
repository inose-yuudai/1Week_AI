using UnityEngine;
using UnityEngine.SceneManagement;

// シーン遷移を管理するクラス
public class SceneLoader : MonoBehaviour
{ 
    // 引数で受け取ったシーンに切り換える
    public void ChangeScene(string sceneName)
    {
       SceneManager.LoadScene(sceneName);
    }
}
