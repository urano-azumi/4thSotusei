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

// OpenAI��Unity��ChatGPT�Ƃ̂���������\�[�X�R�[�h
// �Q�l�T�C�g�uChatGPT API��Unity���瓮�����B�v(https://note.com/negipoyoc/n/n88189e590ac3)

namespace AAA.OpenAI
{
    public class ChatGPTConnection
    {
        private readonly string _apiKey;

        // ��b������ێ����郊�X�g
        public readonly List<ChatGPTMessageModel> _messageList = new();

        public ChatGPTConnection(string apiKey)
        {
            _apiKey = apiKey;
            
            // �L�����N�^�[�̌����L�����N�^�[�̌����Ȃǂ�ݒ肷��\�[�X�R�[�h
            // ������ŏ���ChatGPT�Ɍ����Ɋւ��錾�t�𓊂������镔��
            //_messageList.Add(new ChatGPTMessageModel() { role = "system", content = "" });
        }

        public async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage)
        {
            // ���͐���AI��API�̃G���h�|�C���g��ݒ肷��
            var apiUrl = "https://api.openai.com/v1/chat/completions";

            _messageList.Add(new ChatGPTMessageModel { role = "user", content = userMessage });

            // OpenAI��API���N�G�X�g�ɕK�v�ȃw�b�_�[����ݒ�
            var headers = new Dictionary<string, string>
            {
                {"Authorization","Bearer"+_apiKey},
                { "Content-Type","application/json"},
                {"X-Slack-No-Retry","1" }
            };

            // ���͐����ŗ��p���郂�f����g�[�N������A�v�����v�g���I�v�V�����ɐݒ�
            var options = new ChatGPTCompletionRequestModel()
            {
                model = "gpt-3.5-turbo", //�@gpt-3.5-turbo �� ���f���Fgpt-3.5-turbo-0125���w���Ă���
                messages = _messageList
            };

            var jsonOptions=JsonUtility.ToJson(options);

            Debug.Log("�����F" + userMessage);

            // OpenAI�̕��͐����iCompletion�j��API���N�G�X�g�𑗂�A���ʂ�ϐ��Ɋi�[
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

// �N���X�̒�`
[Serializable]
public class ChatGPTMessageModel
{
    public string role;
    public string content;
}

// ChatGPT API��Request�𑗂邽�߂�JSON�p�N���X
[Serializable]
public class ChatGPTCompletionRequestModel
{
    public string model;
    public List<ChatGPTMessageModel> messages;
}

// ChatGPT API����Response���󂯎�邽�߂̃N���X
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