using Cysharp.Threading.Tasks;
using Meta.Voice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;

// OpenAIでUnityとChatGPTとのやり取りをするソースコード
// 参考サイト「ChatGPT APIをUnityから動かす。」(https://note.com/negipoyoc/n/n88189e590ac3)

namespace AAA.OpenAI
{
    public class ChatGPTConnection
    {
        private readonly string _apiKey;

        // 会話履歴を保持するリスト
        public readonly List<ChatGPTMessageModel> _messageList = new();

        public ChatGPTConnection(string apiKey)
        {
            _apiKey = apiKey;
            
            // キャラクターの語尾やキャラクターの口調などを設定するソースコード
            // いわゆる最初のChatGPTに口調に関する言葉を投げかける部分
            //_messageList.Add(new ChatGPTMessageModel() { role = "system", content = "" });
        }

        public async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage)
        {
            // 文章生成AIのAPIのエンドポイントを設定する
            var apiUrl = "https://api.openai.com/v1/chat/completions";

            _messageList.Add(new ChatGPTMessageModel { role = "user", content = userMessage });

            // OpenAIのAPIリクエストに必要なヘッダー情報を設定
            var headers = new Dictionary<string, string>
            {
                {"Authorization","Bearer"+_apiKey},
                { "Content-Type","application/json"},
                {"X-Slack-No-Retry","1" }
            };

            // 文章生成で利用するモデルやトークン上限、プロンプトをオプションに設定
            var options = new ChatGPTCompletionRequestModel()
            {
                model = "gpt-3.5-turbo", //　gpt-3.5-turbo は モデル：gpt-3.5-turbo-0125を指している
                messages = _messageList
            };

            var jsonOptions=JsonUtility.ToJson(options);

            Debug.Log("自分：" + userMessage);

            // OpenAIの文章生成（Completion）にAPIリクエストを送り、結果を変数に格納
            using var request = new UnityWebRequest(apiUrl, "POST")
            {
                uploadHandler=new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
                downloadHandler=new DownloadHandlerBuffer()
            };

            foreach (var header in headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                throw new Exception();
            }
            else
            {
                var responseString = request.downloadHandler.text;
                var responseObject=JsonUtility.FromJson<ChatGPTResponseModel>(responseString);

                Debug.Log("ChatGPT : " + responseObject.choices[0].message.content);

                _messageList.Add(responseObject.choices[0].message);

                return responseObject;
            }
        }
    }
}

// クラスの定義
[Serializable]
public class ChatGPTMessageModel
{
    public string role;
    public string content;
}

// ChatGPT APIにRequestを送るためのJSON用クラス
[Serializable]
public class ChatGPTCompletionRequestModel
{
    public string model;
    public List<ChatGPTMessageModel> messages;
}

// ChatGPT APIからResponseを受け取るためのクラス
[Serializable]
public class ChatGPTResponseModel
{
    public string id;
    public string @object;
    public int created;
    public Choice[] choices;
    public Usage usage;

    [System.Serializable]
    public class Choice
    {
        public int index;
        public ChatGPTMessageModel message;
        public string finish_reason;
    }

    [System.Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }
}