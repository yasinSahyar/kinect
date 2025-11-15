// Assets/Scripts/KinectReceiver.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;

[Serializable]
public class JointAssignment {
    public string jointName;      // örn: "head", "handRight", "handLeft"
    public Transform targetTransform;
    public float positionScale = 1f;
    public Vector3 positionOffset = Vector3.zero;
    public float smoothing = 0.7f;
}

public class KinectReceiver : MonoBehaviour
{
    public string websocketUrl = "ws://localhost:8080";
    public JointAssignment[] jointAssignments;

    WebSocket ws;

    // Thread-safe latest positions (updated from ws thread, applied on main thread)
    private readonly object _locker = new object();
    private Dictionary<string, Vector3> _latestPositions = null;

    void Start()
    {
        Connect();
    }

    void Connect()
    {
        ws = new WebSocket(websocketUrl);

        ws.OnOpen += (s, e) => {
            Debug.Log("[KinectReceiver] WebSocket connected");
        };

        ws.OnMessage += (s, e) =>
        {
            // e.Data contains JSON bodyFrame
            try {
                var obj = JObject.Parse(e.Data);
                if ((string)obj["type"] != "bodyFrame") return;

                var bodies = obj["frame"]?["bodies"];
                if (bodies == null || !bodies.HasValues) return;

                // find first tracked body
                JObject tracked = null;
                foreach (var b in bodies)
                {
                    if ((bool?)b["tracked"] == true) { tracked = (JObject)b; break; }
                }
                if (tracked == null) return;

                var joints = tracked["joints"] as JObject;
                if (joints == null) return;

                var parsed = new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase);

                foreach (var kv in joints)
                {
                    var jointName = kv.Key; // e.g. "head", "handRight"
                    var j = kv.Value;
                    // Kinect2 node emits cameraX/Y/Z (örnek). adapt et.
                    float x = j["cameraX"] != null ? (float)j["cameraX"] : (float?)j["depthX"] ?? 0f;
                    float y = j["cameraY"] != null ? (float)j["cameraY"] : (float?)j["depthY"] ?? 0f;
                    float z = j["cameraZ"] != null ? (float)j["cameraZ"] : 0f;

                    // Convert to Unity coordinates: tweak -z if needed
                    var v = new Vector3(x, y, -z);
                    parsed[jointName] = v;
                }

                lock (_locker)
                {
                    _latestPositions = parsed;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[KinectReceiver] JSON parse error: " + ex.Message);
            }
        };

        ws.OnError += (s, e) =>
        {
            Debug.LogWarning("[KinectReceiver] WebSocket error: " + e.Message);
        };

        ws.OnClose += (s, e) =>
        {
            Debug.Log("[KinectReceiver] WebSocket closed: " + e.Reason);
        };

        ws.ConnectAsync();
    }

    void Update()
    {
        Dictionary<string, Vector3> positions = null;
        lock (_locker)
        {
            if (_latestPositions != null)
            {
                // copy reference (shallow) and clear so we don't reapply same frame many times
                positions = _latestPositions;
                _latestPositions = null;
            }
        }

        if (positions == null) return;

        // Apply positions to assigned transforms
        foreach (var ja in jointAssignments)
        {
            if (ja == null || ja.targetTransform == null) continue;
            if (!positions.TryGetValue(ja.jointName, out Vector3 pos)) continue;

            // apply scale & offset
            Vector3 worldPos = pos * ja.positionScale + ja.positionOffset;

            // smoothing
            ja.targetTransform.position = Vector3.Lerp(ja.targetTransform.position, worldPos, ja.smoothing * Time.deltaTime * 30f);
        }
    }

    void OnDestroy()
    {
        if (ws != null)
        {
            if (ws.IsAlive) ws.Close();
            ws = null;
        }
    }

    // Optional: editor helper to quickly add default joint names
#if UNITY_EDITOR
    [ContextMenu("Log assigned joints")]
    void LogAssigned()
    {
        foreach(var j in jointAssignments) Debug.Log($"{j.jointName} => {j.targetTransform}");
    }
#endif
}
