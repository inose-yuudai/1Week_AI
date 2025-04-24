using UnityEngine;

[CreateAssetMenu(fileName = "TaskDatabase", menuName = "Hiragana Task Database", order = 1)]
public class HiraganaTaskDatabase : ScriptableObject
{
    public HiraganaTask[] tasks;
}
