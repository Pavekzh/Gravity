using UnityEngine;
using System.Collections;

public class MoveSelectedPlanet : MonoBehaviour
{
    [SerializeField] bool isMovingEnabled = false;
    [SerializeField] new Camera camera;
    [SerializeField] CameraManager cameraManager;

    public void EnableMoving()
    {
        isMovingEnabled = true;
        cameraManager.ControlLocked = true;
    }
    public void DisableMoving()
    {
        isMovingEnabled = false;
        cameraManager.ControlLocked = false;
    }
    void Update()
    {
        
        if (isMovingEnabled)
        {
            if (Input.touchCount == 1 && SelectManager.Instance.SelectedObject != null)
            {
                Ray ray = camera.ScreenPointToRay(Input.GetTouch(0).position);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    SelectManager.Instance.SelectedObject.transform.position = ray.GetPoint(distance);
                }
            }

        }
    }
}
