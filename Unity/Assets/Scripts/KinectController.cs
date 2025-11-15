using UnityEngine;
using System;
using NativeWebSocket;

public class KinectController : MonoBehaviour
{
    WebSocket websocket;
    public GameObject leftHandObj;
    public GameObject rightHandObj;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8080/");

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            UpdateHands(message);
        };

        await websocket.Connect();
    }

    void UpdateHands(string message)
    {
        try
        {
            // Mesaj formatı: "Left:x,y,z;Right:x,y,z"
            string[] parts = message.Split(';');
            string[] leftCoords = parts[0].Replace("Left:", "").Split(',');
            string[] rightCoords = parts[1].Replace("Right:", "").Split(',');

            leftHandObj.transform.position = new Vector3(
                float.Parse(leftCoords[0]),
                float.Parse(leftCoords[1]),
                float.Parse(leftCoords[2])
            );

            rightHandObj.transform.position = new Vector3(
                float.Parse(rightCoords[0]),
                float.Parse(rightCoords[1]),
                float.Parse(rightCoords[2])
            );
        }
        catch (Exception e)
        {
            Debug.Log("Error parsing message: " + e);
        }
    }

    private async void Update()
    {
        if (websocket.State == WebSocketState.Open)
            await websocket.DispatchMessageQueue();
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
