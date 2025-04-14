using UnityEngine;

public class OnClickSceneChanger : MonoBehaviour
{
   [SerializeField,Header("シーンを切り替える処理を持つボタン")] private UIButton _uiButton;
   [SerializeField,Header("シーン切り換えるクラス")] private SceneLoader _sceneLoader;
   [SerializeField,Header("切り換えたいシーン")]　private string _nextSceneName;

   private void Awake()
   {
      _uiButton.OnClickButton(() => _sceneLoader.ChangeScene(_nextSceneName));
   }
}
