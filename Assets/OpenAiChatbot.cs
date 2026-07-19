using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class OpenAIChatbot : MonoBehaviour
{
    private string apiKey = "not_a_real_key"; // Replace with your actual key
    private string apiEndpoint = "https://api.openai.com/v1/chat/completions";

    [System.Serializable]
    private class AIRequest
    {
        public string model = "gpt-3.5-turbo";
        public Message[] messages;
        public float temperature = 0.7f;
    }

    [System.Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    private class AIResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }

    public IEnumerator GetResponse(string characterPrompt, string userInput, System.Action<string> callback)
    {
        // Construct the full prompt
        string fullPrompt = $"{characterPrompt}\n\nUser says: {userInput}";

        // Create request data
        AIRequest requestData = new AIRequest()
        {
            messages = new Message[]
            {
                new Message()
                {
                    role = "system",
                    content = characterPrompt
                },
                new Message()
                {
                    role = "user",
                    content = userInput
                }
            }
        };

        string jsonData = JsonUtility.ToJson(requestData);

        // Create web request
        using (UnityWebRequest request = new UnityWebRequest(apiEndpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                AIResponse response = JsonUtility.FromJson<AIResponse>(request.downloadHandler.text);
                callback(response.choices[0].message.content);
            }
            else
            {
                Debug.LogError("API Error: " + request.error);
                callback("I'm having trouble responding right now.");
            }
        }
    }
}