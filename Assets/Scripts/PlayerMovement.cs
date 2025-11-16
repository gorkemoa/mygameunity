using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public Animator anim;              // zaten eklemiştik

    private CharacterController controller;
    private Camera mainCam;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0f, v).normalized;

        Vector3 camForward = mainCam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = mainCam.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = camForward * input.z + camRight * input.x;

        // ANİMASYON SPEED
        if (anim != null)
        {
            anim.SetFloat("Speed", moveDir.magnitude);
        }

        // 🔥 SALDIRI: Sol mouse veya Space’e basınca
        if (anim != null && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Attack");
        }

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = -9.81f;

        controller.Move(velocity * Time.deltaTime);

        if (moveDir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(moveDir),
                10f * Time.deltaTime
            );
        }
    }
}
