using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using Febucci.UI;

public class ChatCommenter : MonoBehaviour
{
    [System.Serializable]
    public class MessageModel
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class CompletionRequestModel
    {
        public string model;
        public List<MessageModel> messages;
    }

    [System.Serializable]
    public class ChatGPTRecieveModel
    {
        public Choice[] choices;

        [System.Serializable]
        public class Choice
        {
            public MessageModel message;
        }
    }

    [System.Serializable]
    public class LabeledResponse
    {
        public string label; // "positive" / "negative" / "other"
        public string comment; // 表示する発言内容
    }

    [Header("API設定")]
    [SerializeField]
    private string apiKey =
        "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";

    [Header("参照設定")]
    public ChatManager chatManager;

    [Header("効果音設定")]
    public AudioClip positiveClip;
    public AudioClip negativeClip;
    public AudioClip neutralClip;
    public AudioSource audioSource;

    [Header("スコア設定")]
    public int score = 0;
    public int thresholdToNextScene = 10;

    private List<MessageModel> history = new();

    public void SendCommentRequest(string userText)
    {
        history = new List<MessageModel>
        {
            new MessageModel
            {
                role = "system",
                content =
                    "以下のユーザーの発言に対して、女子高生っぽく返事をしてください。"
                    + "返答は必ず次のJSON形式で出力してください："
                    + "{\"label\":\"positive または negative または other\", \"comment\":\"返答テキスト\"}。"
                    + "ラベルは必ずどれか一つを選んでください。あなたの名前は「あい」です。17歳の女子高生です。"
                    + "JSON以外の文字（説明や補足）を含めないでください。文脈的に質問をされてると思ったら質問に答えてください。文字数は多くても25字以内にして"
            },
            new MessageModel { role = "user", content = userText }
        };

        StartCoroutine(SendToGPT(userText));
    }

    private IEnumerator SendToGPT(string userText)
    {
        history.Add(new MessageModel { role = "user", content = userText });

        var requestData = new CompletionRequestModel
        {
            model = "gpt-3.5-turbo",
            messages = history
        };

        string jsonBody = JsonUtility.ToJson(requestData);
        var request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("GPT Error: " + request.error);
            yield break;
        }

        var json = request.downloadHandler.text;
        var response = JsonUtility.FromJson<ChatGPTRecieveModel>(json);
        string rawResponse = response.choices[0].message.content.Trim();

        LabeledResponse labeled;
        try
        {
            labeled = JsonUtility.FromJson<LabeledResponse>(rawResponse);
        }
        catch (Exception e)
        {
            Debug.LogError("返ってきたJSONを解析できませんでした: " + rawResponse);
            Debug.LogException(e);
            yield break;
        }

        GameObject bubble = chatManager.AddChatMessage(
            labeled.comment,
            chatManager.aiBubblePrefab,
            chatManager.aiStartPos,
            false
        );

        TypewriterByCharacter typewriter = bubble.GetComponentInChildren<TypewriterByCharacter>();
        if (typewriter != null)
        {
            typewriter.ShowText(labeled.comment);
        }

        HandleCommentResponse(labeled.label);
        history.Add(response.choices[0].message);
    }

    private void HandleCommentResponse(string label)
    {
        switch (label)
        {
            case "positive":
                PositiveReaction();
                break;
            case "negative":
                NegativeReaction();
                break;
            case "other":
                NeutralReaction();
                break;
            default:
                Debug.LogWarning("未知のラベル: " + label);
                break;
        }
    }

    private void PositiveReaction()
    {
        Debug.Log("🎉 Positiveリアクション：喜びの演出！");
        if (audioSource != null && positiveClip != null)
            audioSource.PlayOneShot(positiveClip);
        score += 9;

    }

    private void NegativeReaction()
    {
        Debug.Log("Negativeリアクション：怒りの演出！");
        if (audioSource != null && negativeClip != null)
            audioSource.PlayOneShot(negativeClip);
        score += 1;

    }

    private void NeutralReaction()
    {
        Debug.Log("Otherリアクション：通常の演出！");
        if (audioSource != null && neutralClip != null)
            audioSource.PlayOneShot(neutralClip);
        score += 2;

    }

    public void CheckSceneTransition()
    {

            string nextScene = "";

            if (score < 20)
                nextScene = "Goal1";
            else if (score < 30)
                nextScene = "Goal2";
            else
                nextScene = "Goal3";

            Debug.Log($"スコア {score} によりシーン遷移：{nextScene}");
            SceneManager.LoadScene(nextScene);
        
    }
  void Update()
  {
    Debug.Log(score);
  }
}
