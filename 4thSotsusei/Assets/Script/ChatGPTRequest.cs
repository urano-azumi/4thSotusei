using AAA.OpenAI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTRequest : MonoBehaviour
{
    // OpenAI��API�L�[
    public string openapiKey;

    public TextMeshProUGUI inputField;
    private string inputText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InputText()
    {
        inputText = inputField.text;
    }

    public void PushRequest()
    {
        //Debug.Log("���͂��ꂽ�����F" + inputText);

        var chatGPTConnection = new ChatGPTConnection(openapiKey);

        chatGPTConnection.RequestAsync(inputText);
    }
}
