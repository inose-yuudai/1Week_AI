using UnityEngine;
using TMPro;
using DG.Tweening;
using Fungus;

public class GameManager : MonoBehaviour
{
    // お題とスコア関連
    public string currentTask;
    public int currentScore;

    // 制限時間関連
    public float timeLimit = 60.0f;
    private float currentTime;

    [Header("参照設定")]
    public HiraganaTextGenerator hiraganaGenerator;
    public ChatManager chatManager;
    public HiraganaBoardController boardController;

    public ChatCommenter chatCommenter; // ← インスペクタでセット忘れずに！

    public TMP_Text taskText;
    public TMP_Text timeText;

    public HiraganaTaskDatabase taskDatabase; // ← ScriptableObjectで管理するお題集
    private HiraganaTask currentTaskData;

    public bool timeStarted = false;

    void Start()
    {
        currentTime = timeLimit;
        currentScore = 0;

        if (hiraganaGenerator != null)
        {
            hiraganaGenerator.OnTextGenerated += OnGeneratedText;
        }
    }

    public void StartGame()
    {
        timeStarted = true;
    }

    void Update()
    {
        if (timeStarted)
        {
            currentTime -= Time.deltaTime;
            UpdateTimeText();

            if (currentTime <= 0)
            {
                chatCommenter.CheckSceneTransition();
            }
        }
    }

    void OnGeneratedText(string generatedText)
    {
        Debug.Log("生成されたテキスト: " + generatedText);

        if (chatManager != null)
        {
            chatManager.AddChatMessage(
                generatedText,
                chatManager.playerBubblePrefab,
                chatManager.playerStartPos,
                true
            );
        }
    }

    public void SetNewTask(string task)
    {
        currentTask = task;
        if (taskText != null)
        {
            taskText.text = task;
        }
    }

    public void SetRandomTask()
    {
        if (taskDatabase == null || taskDatabase.tasks.Length == 0)
        {
            Debug.LogWarning("タスクデータベースが未設定、または空です");
            return;
        }

        currentTaskData = taskDatabase.tasks[Random.Range(0, taskDatabase.tasks.Length)];

        // UI表示用の説明文（見た目だけ）
        SetNewTask(currentTaskData.taskText);

        // ハイライトだけ反映
        boardController.HighlightDefaultPositionsOnly(
            currentTaskData.highlightText,
            new Color(1.0f, 0.713f, 0.757f)
        );
        boardController.lastHighlightedText = currentTaskData.highlightText;

        // あとで送信したい文字列だけ保存
        if (!string.IsNullOrEmpty(currentTaskData.inputText))
        {
            currentTask = currentTaskData.inputText;
        }
    }


    void UpdateTimeText()
    {
        if (timeText != null)
        {
            timeText.text = " " + Mathf.Max(0, Mathf.FloorToInt(currentTime)) + "";
        }
    }


    public void GenerateTextFromCurrentTask()
    {
        if (hiraganaGenerator != null && !string.IsNullOrEmpty(currentTask))
        {
            hiraganaGenerator.GenerateText(currentTask, null, true); // currentTaskはhighlightText
            Debug.Log("gameから呼び出し");
        }
    }
    public void SendCurrentInputText()
    {
        if (hiraganaGenerator != null && !string.IsNullOrEmpty(currentTask))
        {
            hiraganaGenerator.GenerateAndSendPlayerMessage(currentTask);
            Debug.Log("入力を送信しました: " + currentTask);
        }
    }
}
