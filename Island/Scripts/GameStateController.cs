using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// Class to match the JSON structure of the response
[System.Serializable]
public class GameState
{
    public string world;
    public string task;
    public int state;
}

public class GameStateController : MonoBehaviour
{

    public IEnumerator SaveState(string world, string task, int state)
    {
        WWWForm form = new WWWForm();
        form.AddField("world", world);
        form.AddField("task", task);
        form.AddField("state", state);
        using (UnityWebRequest request = UnityWebRequest.Post("http://" + MainMenu.serverIp + ":5000/api/Users/SaveState", form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + Login.token);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.ConnectionError &&
             request.result != UnityWebRequest.Result.ProtocolError)
            {
                // print("Request success!");
            }
            else
            {
                // print("Request failed");
            }
        }
    }


    public IEnumerator GetState(System.Action<GameState> callback)
    {
        // Create a UnityWebRequest for the GET request
        using (UnityWebRequest request = UnityWebRequest.Get("http://" + MainMenu.serverIp + ":5000/api/Users/GetState"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + Login.token);

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // Parse the JSON response and create the GameState object
                GameState responseData = JsonUtility.FromJson<GameState>(request.downloadHandler.text);
                // Call the callback with the response data
                callback?.Invoke(responseData);
            }
        }
    }


}
