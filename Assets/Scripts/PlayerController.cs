using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10;

    [Header("점프설정")]
    public float landingDuration = 0.3f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    [Header("공격 설정")]
    public float attackDruation = 0.8f;         //공격 지속 시간
    public bool canMoveWhileAttacking = false;  //공격중 이동 가능 여부

    [Header("컴포넌트")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    private float currentSpeed;
    private bool isAttacking = false;   //공격중인가
    private bool isLanding = false;
    private float landingTimer;

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private float attackTimer;

    private bool isUIMode = false;      //UI 모드 설정



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCursorLock();
        }

        if (!isUIMode)
        {
            CheckGrounded();
            HandleLandeing();
            HandleMovement();
            UpdateAnimator();
            HandleJump();
            HandleAttack();
            UpdateAnimator();
        }

        
    }

    void CheckGrounded()
    {
        //이전상태 저장
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        if (!isGrounded && wasGrounded)
        {
            Debug.Log("떨어지기 시작");
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            if (!wasGrounded && animator != null)
            {
                animator.SetTrigger("landTrigger");
                isLanding = true;
                landingTimer = landingDuration;
                Debug.Log("착지");
            }
        }
    }
    void HandleMovement()
    {
        if ((isAttacking && !canMoveWhileAttacking) || isLanding)
        {
            currentSpeed = 0;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;    //이동 방향 설정

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;

            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime); //캐릭터 컨트롤러의 이동 입력

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }
        else
        {
            currentSpeed = 0;
        }
    }

    void UpdateAnimator()
    {
        //전체 최대 속도 기준으로 0~1 계산
        float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
        animator.SetFloat("Speed", animatorSpeed);
        animator.SetBool("isGrounded", isGrounded);

        bool isFalling = !isGrounded && velocity.y < 0.1f;
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isLanding", isLanding);

    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                animator.SetTrigger("jumpTrigger");
            }
        }

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;

        }

        controller.Move(velocity *  Time.deltaTime);
    }

    void HandleLandeing()
    {
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;
            if (landingTimer <= 0)
            {
                isLanding = false;
            }
        }
    }

    void HandleAttack()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDruation;

            if (animator != null)
            {
                animator.SetTrigger("attackTrigger");
            }
        }
    }

    public void SetCursorLock(bool lockCursor)
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isUIMode = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isUIMode = true;
        }
    }

    public void ToggleCursorLock()
    {
        bool shouldLock = Cursor.lockState != CursorLockMode.Locked;
        SetCursorLock(shouldLock);
    }

    public void SetUIMode(bool uiMode)
    {
        SetCursorLock(!uiMode);
    }
}
