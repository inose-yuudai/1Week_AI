using UnityEngine;

[CreateAssetMenu(fileName = "NewHiraganaTask", menuName = "Hiragana Task", order = 0)]
public class HiraganaTask : ScriptableObject
{
    [TextArea]
    public string taskText; // お題文（見た目に出すだけ）
    public string highlightText; // ハイライト対象文字
    public string inputText; // プレイヤーが完成させるべき文字列
}
