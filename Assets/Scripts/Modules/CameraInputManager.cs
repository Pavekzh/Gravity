using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;

public enum InputMode
{
    Moving,
    Rotation
}
class CameraInputManager:MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private bool isControlLocked;
    [SerializeField] private InputMode inputMode;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private Vector3 movingSpeed = new Vector3(0.05f,0.05f,0.05f);

    public InputMode InputMode { get => inputMode; set => inputMode = value; }
    public bool IsControlLocked { get => isControlLocked; set => isControlLocked = value; }

    private float DistanceBetweenTwoTouchPoints;

    private void Update()
    {
        if(isControlLocked == false)
        {
            if(InputMode == InputMode.Moving)
            {
                cameraManager.Moving(ReadMovingInput());
            }
            else if (InputMode == InputMode.Rotation)
            {
                cameraManager.Rotation(ReadRotationInput());
            }
            cameraManager.Zoom(ReadZoomInput());
        }
    }
    private float ReadZoomInput()
    {
        if (Input.touchCount >= 2)
        {
            float distance = 0;
            distance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                DistanceBetweenTwoTouchPoints = distance;
            }
            else
            {
                float deltaDistance = (distance - DistanceBetweenTwoTouchPoints) * zoomSpeed;
                DistanceBetweenTwoTouchPoints = distance;
                return deltaDistance;
            }
        }
        return 0;
    }
    private Vector3 ReadMovingInput()
    {
        if (Input.touchCount == 1 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
        {
            Vector3 vector = new Vector3(Input.GetTouch(0).deltaPosition.x * movingSpeed.x, 0, -(Input.GetTouch(0).deltaPosition.y * movingSpeed.z));
            return vector;
        }
        return Vector3.zero;
    }
    private Vector2 ReadRotationInput()
    {
        Vector2 OrbitAngle = new Vector2();
        if (Input.touchCount == 1 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
        {
            OrbitAngle.y = Input.GetTouch(0).deltaPosition.x * rotationSpeed;
            OrbitAngle.x = -(Input.GetTouch(0).deltaPosition.y * rotationSpeed);
            return OrbitAngle;
        }
        return Vector2.zero;
    }
    public void ChangeInputMode()
    {
        if (InputMode == InputMode.Moving)
        {
            this.InputMode = InputMode.Rotation;
        }
        else if (InputMode == InputMode.Rotation)
        {
            this.InputMode = InputMode.Moving;
        }
    }
    public void ChangeInputMode(int Mode)
    {
        if (Mode == 0)
            this.inputMode = InputMode.Moving;
        else if (Mode == 1)
            this.inputMode = InputMode.Rotation;
    }
}

