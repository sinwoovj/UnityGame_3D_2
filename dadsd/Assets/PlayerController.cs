using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float lookSpeed;
    [SerializeField] private float camRotateLimit;
    private float currentCamRotateX = 0f;

    [SerializeField] Camera cam;

    private Animator anim;

    private CharacterController characterController;

    private bool canDJ;
    [SerializeField] private float JumpPower;
    [SerializeField] private float gravity = 1f;
    [SerializeField] private float gravImpulse;
    private Vector3 vec;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
        vec = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
        CharacterRotation();
        CameraRotate();
        CharacterAnimation();
        Jump();
    }
    private void Jump()
    {
        Debug.Log(characterController.isGrounded);
        if (IsCheckGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gravImpulse = JumpPower;
            }
        }
        else
        {
            gravImpulse -= gravity;
        }
        vec.y = gravImpulse;
        characterController.Move(vec * Time.deltaTime);
    }
    private void CharacterAnimation()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        anim.SetFloat("Ver", ver);
        anim.SetFloat("Hor", hor);
    }
    private void CharacterRotation()
    {
        float yR = Input.GetAxisRaw("Mouse X");
        Vector3 vec = new Vector3(0f, yR, 0f) * lookSpeed;
        transform.Rotate(vec);

    }
    private void CameraRotate()
    {
        float xR = Input.GetAxisRaw("Mouse Y");
        float camRX = xR * lookSpeed;
        currentCamRotateX -= camRX;
        currentCamRotateX = Mathf.Clamp(currentCamRotateX, -camRotateLimit, camRotateLimit);

        cam.transform.localEulerAngles = new Vector3(currentCamRotateX, 0f, 0f);
    }
    private bool IsCheckGrounded()
    {
        // CharacterController.IsGrounded�� true��� Raycast�� ������� �ʰ� ���� ����
        if (characterController.isGrounded) return true;
        // �߻��ϴ� ������ �ʱ� ��ġ�� ����
        // �ణ ��ü�� ���� �ִ� ��ġ�κ��� �߻����� ������ ����� ������ �� ���� ���� �ִ�.
        var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        // Ž�� �Ÿ�
        var maxDistance = 1.5f;
        // ���� ����� �뵵
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
        // Raycast�� hit ���η� ����
        // ���󿡸� �浹�� ���̾ ����
        return Physics.Raycast(ray, maxDistance, layerMask);
    }
}
