using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneManager : MonoBehaviour
{
    /// <summary>
    /// 現在のシーンをリロードする
    /// </summary>
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// 次のシーンをロードする（ビルド順序に基づく）
    /// </summary>
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings; // ループする
        SceneManager.LoadScene(nextSceneIndex);
    }

    /// <summary>
    /// 指定されたシーン名のシーンをロードする
    /// </summary>
    /// <param name="sceneName">ロードしたいシーンの名前</param>
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// 指定されたシーンインデックスのシーンをロードする
    /// </summary>
    /// <param name="sceneIndex">ロードしたいシーンのビルドインデックス</param>
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning("指定されたシーンインデックスが無効です: " + sceneIndex);
        }
    }

    /// <summary>
    /// アプリケーションを終了する
    /// </summary>
    public void QuitApplication()
    {
        Debug.Log("アプリケーションを終了します。");
        Application.Quit();
    }
}