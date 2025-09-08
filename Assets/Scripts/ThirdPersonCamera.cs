using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Ÿ�� ����")]
    public Transform target;

    [Header("ī�޶� �Ÿ� ����")]
    public float distance = 8.0f;
    public float height = 5.0f;

    [Header("���콺 ����")]
    public float mouseSensitivity = 2.0f;
    public float minVerticalAngle = -30.0f;
    public float maxVerticalAngle = 60.0f;

    [Header("�ε巯�� ����")]
    public float positionSmoothTime = 0.2f;                        //��ġ ���󰡱� �ε巯��
    public float rotationSmoothTime = 0.1f;                       //ȸ�� �ε巯��

    //ȸ�� ����
    private float horizontalAngle = 0f;
    private float verticalAngle = 0f;

    //�����ӿ� ����
    private Vector3 currentVelocity;                           //SmoothDamp �� �ӵ�
    private Vector3 currentPosition;                           //���� ��ġ(������ ���)
    private Quaternion currentRotation;                         //���� ȸ�� ( ������ ���)


    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
        currentPosition = transform.position;
        currentRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { ToggleCursor(); }
    }
    private void LateUpdate()
    {
        if (target == null) return;

        HandleMouseInput();
        UpdateCameraSmooth();

    }

    void HandleMouseInput()
    {
        //Ŀ���� ������� ���� ���콺 �Է� ó��
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        horizontalAngle += mouseX;
        verticalAngle -= mouseY;

        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);

    }
    void UpdateCameraSmooth()
    {
        //1.��ǥ ��ġ ���
        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        Vector3 rotatedOffset = rotation * new Vector3(0, height, -distance);
        Vector3 targetPosition = target.position + rotatedOffset;

        //2.��ǥ ȸ�� ���
        Vector3 lookTarget = target.position + Vector3.up * height;
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - targetPosition);

        //3.�ε巴�� �̵�
        currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref currentVelocity, positionSmoothTime);

        //4.�ε巴�� ȸ��
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime / rotationSmoothTime);

        //5.�� ���
        transform.position = currentPosition;
        transform.rotation = currentRotation;
    }

    void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
