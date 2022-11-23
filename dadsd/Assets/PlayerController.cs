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

    private float jumpPower;
    private float gravity;
    private Vector3 moveDir;

    private int canJumpCount;
    // Start is called before the first frame update
    void Start()
    {
        canJumpCount = 2;
        gravity= -9.8f;
        moveDir = Vector3.zero;
        jumpPower = 7.0f;
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(moveDir.y);
        CharacterRotation();
        CameraRotate();
        CharacterAnimation();
        Jump();
        characterController.Move(moveDir * Time.deltaTime);
    }
    private void Jump()
    {
        if (IsCheckGrounded()) canJumpCount = 2;
        if (IsCheckGrounded() && Input.GetButtonDown("Jump"))
        {
            canJumpCount--;
            moveDir.y = jumpPower;

        }
        else if(!IsCheckGrounded() && Input.GetButtonDown("Jump") && canJumpCount == 1)
        {
            canJumpCount--;
            moveDir.y = 10f;
        }
        else
        {
            moveDir.y += gravity * Time.deltaTime;
        }

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
        
        // CharacterController.IsGrounded가 true라면 Raycast를 사용하지 않고 판정 종료
        if (characterController.isGrounded) return true;
        // 발사하는 광선의 초기 위치와 방향
        // 약간 신체에 박혀 있는 위치로부터 발사하지 않으면 제대로 판정할 수 없을 때가 있다.
        var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        // 탐색 거리
        var maxDistance = 1.5f;
        // 광선 디버그 용도
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
        // Raycast의 hit 여부로 판정
        // 지상에만 충돌로 레이어를 지정
        return Physics.Raycast(ray, maxDistance, layerMask);
    }
}
