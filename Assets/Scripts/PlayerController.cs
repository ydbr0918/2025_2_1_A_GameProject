using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10;

    [Header("���� ����")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;                         //�߷� �ӵ� �߰�
    public float landingDuration = 0.3f;                   //���� �� ���� ���ӽð�


    [Header("���� ����")]
    public float attackDuration = 0.8f;                              //���� ���� �ð�
    public bool canMoveWhileAttacking = false;                       //������ �̵� ���� ����

    [Header("������Ʈ")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    //���� ����
    private float currentSpeed;
    private bool isAttacking = false;                                 //���������� üũ
    private bool isLanding = false;                                  //���� ������ Ȯ��
    private float landingTimer;                                      //���� Ÿ�̸�

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;                                        //���� �������� �� �̿�����
    private float attackTimer;

    private bool isUIMode = false;

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
            HandleLanding();
            HandleMovement();
            HandleJump();
            HandleAttack();
            UpdateAnimator();
        }
        
    }

    void CheckGrounded()
    {
        //���� ���� ����
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;                      //ĳ���� ��Ʈ�ѷ����� �޾ƿ´�

        if (!isGrounded && wasGrounded)                             //������ ����������(���� �������� ���� �ƴϰ�, ���� �������� ��)
        {
            Debug.Log("�������� ����");
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            //���� ��� Ʈ���� �� ���� ���� ����
            if (!wasGrounded && animator != null)
            {
                //animator.SetTrigger("landTrigger");
                isLanding = true;
                landingTimer = landingDuration;
                Debug.Log("����");
            }
        }
    }

    void HandleJump()
    {
        if (Input.GetButton("Jump") && isGrounded)       //�� ���� �������� ������ �� �� �ִ�
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                animator.SetTrigger("jumpTrigger");
            }
        }

        if (!isGrounded)                                    //���� ���� ���� ��� �߷� ����
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovement()              //�̵� �Լ� ����
    {
        if ((isAttacking && !canMoveWhileAttacking) || isLanding)
        {
            currentSpeed = 0;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float verical = Input.GetAxis("Vertical");

        if (horizontal != 0 || verical != 0)                //������ �ϳ��� �Է��� ���� ��
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * verical + cameraRight * horizontal;        //�̵� ���� ����

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);    //ĳ���� ��Ʈ�ѷ��� �̵� �Է�

            //�̵� ���� ���������� �ٶ󺸸鼭 �̵�
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;                                    //�̵��� �ƴ� ��� ���ǵ� 0
        }

    }

    void HandleLanding()
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
            attackTimer = attackDuration;

            if (animator != null)
            {
                animator.SetTrigger("attackTrigger");
            }
        }
    }

    void UpdateAnimator()
    {
        //��ü �ִ� �ӵ�(runSpeed) �������� 0~1 ���
        float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
        animator.SetFloat("Speed", animatorSpeed);
        animator.SetBool("isGrounded", isGrounded);

        bool isFalling = !isGrounded && velocity.y < -0.1f;             //ĳ������ Y�� �ӵ��� ������ �Ѿ�� �������� �ִٰ� �Ǵ�
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isLanding", isLanding);

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
