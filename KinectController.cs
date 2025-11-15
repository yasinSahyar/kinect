using UnityEngine;
using NativeWebSocket;
using System.Text;
using System.Collections.Generic;

public class KinectController : MonoBehaviour
{
    WebSocket websocket;

    public Transform leftHandObj;  // Unity sahnesindeki sol el objesi
    public Transform rightHandObj; // Unity sahnesindeki sağ el objesi

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            ApplyKinectData(message);
        };

        await websocket.Connect();
    }

    void ApplyKinectData(string json)
    {
        // JSON parse et
        var data = JsonUtility.FromJson<HandData>(json);

        // Unity objelerini güncelle
        if (leftHandObj != null)
            leftHandObj.localPosition = new Vector3(data.leftHand.x, data.leftHand.y, data.leftHand.z);

        if (rightHandObj != null)
            rightHandObj.localPosition = new Vector3(data.rightHand.x, data.rightHand.y, data.rightHand.z);
    }

    [System.Serializable]
    public class HandData
    {
        public Vector3 leftHand;
        public Vector3 rightHand;
    }
}
