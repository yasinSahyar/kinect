using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 50f;
    public Transform orientation;
    private float xInput;
    private float yInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    public float jumpForce = 5f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Klavye girdilerini oku
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        // Zıplama
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // Hareket yönünü hesapla
        moveDirection = orientation.forward * yInput + orientation.right * xInput;
        rb.AddForce(moveDirection.normalized * speed, ForceMode.Force);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
