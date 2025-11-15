using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraRayController : MonoBehaviour
{
    public Text infoText;
    public Slider delaySlider;
    public float teleportDelay = 2f;

    private float delayTimer = 0f;

    void Start()
    {
        if (infoText != null) infoText.enabled = false;
        if (delaySlider != null) delaySlider.gameObject.SetActive(false);
        if (delaySlider != null) delaySlider.maxValue = teleportDelay;
    }

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 dir = transform.forward;
        Debug.DrawRay(origin, dir * 50f, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, 50f))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj.CompareTag("InfoSpot"))
            {
                ShowInfo(hitObj);
            }
            else if (hitObj.CompareTag("TeleportSpot"))
            {
                HandleTeleport(hitObj);
            }
            else
            {
                ResetHit();
            }
        }
        else
        {
            ResetHit();
        }
    }

    void ShowInfo(GameObject obj)
    {
        if (infoText == null) return;
        infoText.enabled = true;
        // Objeye göre mesaj özelleştir
        if (obj.name == "InfoCapsule")
            infoText.text = "Bu bir kapsüldür. (Örnek bilgi.)";
        else
            infoText.text = "Bu nesne hakkında bilgi yok.";
    }

    void HandleTeleport(GameObject obj)
    {
        if (delaySlider == null) return;
        delayTimer += Time.deltaTime;
        delaySlider.gameObject.SetActive(true);
        delaySlider.value = delayTimer;
        // obj adını hedef sahne ismi yap (örnek: Scene2)
        string targetScene = obj.name; // objenin ismini hedef sahne adıyla eşle
        if (delayTimer >= teleportDelay)
        {
            delayTimer = 0f;
            SceneManager.LoadScene(targetScene);
        }
    }

    void ResetHit()
    {
        if (infoText != null) infoText.enabled = false;
        if (delaySlider != null) delaySlider.gameObject.SetActive(false);
        delayTimer = 0f;
    }
}
