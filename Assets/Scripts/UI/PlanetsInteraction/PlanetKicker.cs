using UnityEngine;
using System.Collections;
using Assets.Library;
using UnityEngine.EventSystems;

public class PlanetKicker : StateChanger
{
    [SerializeField] bool isKickEnabled;
    [SerializeField] new Camera camera;
    [SerializeField] CameraManager cameraManager;
    [SerializeField] float vectorPower;
    [SerializeField] [Range(0.000001f, 1)] float kickForce;

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
            }
            else
            {
                isKickEnabled = false;
                cameraManager.ControlLocked = false;
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
    private Vector3 VectorPow(Vector3 vector,float power)
    {
        return new Vector3(Mathf.Pow(vector.x,power), Mathf.Pow(vector.y, power), Mathf.Pow(vector.z, power));
    }
    private void Update()
    {
        if (isKickEnabled)
        {
            if (Input.touchCount == 1 && SelectManager.Instance.SelectedObject != null)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    controllLock = true;
                    Debug.Log("Controll locked");
                }
                if (!controllLock)
                {
                    Vector3 directVector = Vector3.zero;
                    Vector3 objPosition = camera.WorldToScreenPoint(SelectManager.Instance.SelectedObject.GravityModule.Position);

                    Vector3 touchPosition = Input.GetTouch(0).position;
                    Vector3 directPoint = objPosition - touchPosition;
                    directPoint = directPoint.normalized * Mathf.Pow(directPoint.magnitude, vectorPower);
                    directPoint = new Vector3(directPoint.x, 0, directPoint.y);
                    directVector = directPoint + SelectManager.Instance.SelectedObject.GravityModule.Position;

                    float pxForStandardForce = Screen.width * kickForce;
                    directVector = (directVector / pxForStandardForce) * (cameraManager.RotationRadius / CameraManager.DefaultRadius);
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        SelectManager.Instance.SelectedObject.GravityModule.AddVelocity(directVector);
                    }
                    Debug.DrawLine(SelectManager.Instance.SelectedObject.GravityModule.Position, SelectManager.Instance.SelectedObject.GravityModule.Position + directVector,Color.red);
                }
                else
                {
                    if(Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        controllLock = false;
                        Debug.Log("Controll unlocked");
                    }
                }
            }
        }
    }
}
