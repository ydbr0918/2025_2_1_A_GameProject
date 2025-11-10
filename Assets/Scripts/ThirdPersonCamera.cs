using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("타겟 설정")]
    public Transform target;

    [Header("카메라 거리 설정")]
    public float distance = 8.0f;
    public float height = 5.0f;

    [Header("마우스 설정")]
    public float mouseSensitivity = 2.0f;
    public float minVerticalAngle = -30.0f;
    public float maxVerticalAngle = 60f;

    [Header("부드러움 설정")]
    public float positionSmoothTime = 0.2f;     //위치 따라가기 부드러움
    public float rotationSmoothTime = 0.1f;     //회전 부드러움

    //회전 각도
    private float horizontalAngle = 0f;
    private float verticalAngle = 0f;

    private Vector3 currentVelocity;            //SmoothDamp 용 속도
    private Vector3 currentPosition;            //현재 위치
    private Quaternion currentRotation;         //현재 회전

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
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
        if (Input.GetKeyDown(KeyCode.F10)) { ToggleCursor(); }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        HandleMouseInput();
        UpdateCameraSmooth();
    }

    void HandleMouseInput()
    {
        if(Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        horizontalAngle += mouseX;
        verticalAngle -= mouseY;

        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);
    }

    void UpdateCameraSmooth()
    {
        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        Vector3 rotationOffset = rotation * new Vector3(0, height, -distance);
        Vector3 targetPosition = target.position + rotationOffset;

        Vector3 lookTarget = target.position + Vector3.up * height;
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - targetPosition);

        currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition,
            ref currentVelocity, positionSmoothTime);

        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime/rotationSmoothTime);

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
