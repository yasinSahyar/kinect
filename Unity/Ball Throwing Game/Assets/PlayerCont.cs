using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input System
using TMPro;                  // <-- TextMeshPro için gerekli kütüphane

public class PlayerCont : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10f;
    public float jumpForce = 5f; // BONUS: Zıplama gücü

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded = true;

    [Header("UI Elements (TextMeshPro)")]
    public TMP_Text countText; // CountText için TMP versiyonu
    public TMP_Text winText;   // WinText için TMP versiyonu

    private int count;
    private int totalPickUps;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        // Sahnedeki tüm PickUp objelerini say
        totalPickUps = GameObject.FindGameObjectsWithTag("PickUp").Length;

        SetCountText();

        // Eğer WinText'i kodla gizlemek istersen (isteğe bağlı)
        // winText.gameObject.SetActive(false);
        winText.text = "";
    }

    // Yeni Input System: Hareket için
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Yeni Input System: Zıplama için
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0.0f, moveInput.y);
        rb.AddForce(movement * speed);
    }

    // PickUp objesiyle çarpışma kontrolü
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Zeminle temas varsa tekrar zıplayabilir
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= totalPickUps)
        {
            // Eğer sahnede devre dışıysa aktif hale getir
            winText.gameObject.SetActive(true);
            winText.text = "You Win!";
        }
    }
}
