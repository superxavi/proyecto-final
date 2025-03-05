using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float movimientoSpeed = 10.0f;
    public float rotationSpeed = 150.0f;
    public float deadzone = 0.2f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask;

    private Animator animator;
    private Rigidbody rb;
    private float x, y;
    private bool isGrounded;
    private float lastJumpTime;
    private float jumpCooldown = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGround();
        HandleMovement();
        HandleActions();
        HandleAnimation();
    }

    void CheckGround()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, groundCheckDistance, groundMask);
    }

    private void HandleMovement()
    {
        // Movimiento adelante/atrás
        y = Input.GetAxis("Vertical");

        // Rotación
        float keyboardRotation = Input.GetAxis("Horizontal");
        float controllerRotation = Input.GetAxis("RightStickHorizontal");
        x = ApplyDeadzone(keyboardRotation + controllerRotation);

        transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);
        transform.Translate(0, 0, y * Time.deltaTime * movimientoSpeed);
    }

    private void HandleActions()
    {
        // Salto (Botón X del PS4)
        if (Input.GetButtonDown("Fire2") && isGrounded && Time.time > lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
            lastJumpTime = Time.time;
        }
        

        // Golpe (Botón Cuadrado del PS4)
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void HandleAnimation()
    {
        animator.SetFloat("velx", x);
        animator.SetFloat("velz", y);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private float ApplyDeadzone(float input)
    {
        if (Mathf.Abs(input) < deadzone) return 0f;
        return Mathf.Clamp(input, -1f, 1f);
    }
}