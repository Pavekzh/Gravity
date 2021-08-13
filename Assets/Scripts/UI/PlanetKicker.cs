using UnityEngine;
using System.Collections;
using Assets.Library;
using UnityEngine.EventSystems;
using BasicTools;

public class PlanetKicker : StateChanger
{
    [SerializeField] ManipulatorView manipulator;
    [SerializeField] bool isKickEnabled;
    [SerializeField] new Camera camera;
    [SerializeField] CameraModel cameraManager;
    [SerializeField] float vectorPower = 1.2f;
    [SerializeField] float kickForce = 1;

    public override State State
    {
        get
        {
            if (isKickEnabled)
                return State.Changed;
            else
                return State.Default;
        }
        set
        {
            if (value == State.Changed)
            {
                isKickEnabled = true;
                cameraManager.ControlLocked = true;
                manipulator.SetActive(true);
            }
            else
            {
                isKickEnabled = false;
                cameraManager.ControlLocked = false;
                manipulator.SetActive(false);

            }

        }
    }

    private bool controllLock;
    private void Start()
    {
        if (isKickEnabled)
            State = State.Changed;
        else
            State = State.Default;
    }
    private void Update()
    {
        if (isKickEnabled && SelectManager.Instance.SelectedObject != null)
        {
            if (Input.touchCount == 1)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    controllLock = true;
                    Debug.Log("Control locked");
                }
                if (!controllLock)
                {
                    Ray ray = camera.ScreenPointToRay(Input.GetTouch(0).position);
                    Plane plane = new Plane(Vector3.up, Vector3.zero);
                    float distance;
                    if (plane.Raycast(ray,out distance))
                    {
                        float zoomFactor = cameraManager.RotationRadius / CameraModel.DefaultRadius;

                        Vector3 touchPoint = ray.GetPoint(distance);
                        Vector3 objPoint = SelectManager.Instance.SelectedObject.GravityModule.Position;
                        Vector3 direct = objPoint - touchPoint;

                        direct = direct.normalized * Mathf.Pow(direct.magnitude, vectorPower);
                        direct = direct * zoomFactor * kickForce;
                        if (Input.GetTouch(0).phase == TouchPhase.Ended)
                        {
                            SelectManager.Instance.SelectedObject.GravityModule.Velocity = direct;
                        }
                        manipulator.SetManipulator(objPoint, objPoint + direct, touchPoint,zoomFactor);
                    }

                }
                else
                {
                    if(Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        controllLock = false;
                        Debug.Log("Control unlocked");
                    }
                }
            }
            else
            {

                float zoomFactor = cameraManager.RotationRadius / CameraModel.DefaultRadius;
                Vector3 obj = SelectManager.Instance.SelectedObject.GravityModule.Position;
                Vector3 direct = SelectManager.Instance.SelectedObject.GravityModule.Velocity;
                Vector3 touch = Vector3.zero;
                if (direct != Vector3.zero)
                {
                    touch = direct / zoomFactor / kickForce;
                    touch = touch.normalized * Mathf.Pow(touch.magnitude, 1 / vectorPower);
                    touch = obj - touch;

                }
                manipulator.SetManipulator(obj, obj + direct, touch, zoomFactor);
            }
        }
    }
}
