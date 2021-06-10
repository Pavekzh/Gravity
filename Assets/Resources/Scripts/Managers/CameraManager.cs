using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;

class CameraManager : Singleton<CameraManager>
{
    [Header("Limiters")]
    [SerializeField] private float maxHeight;
    [SerializeField] private float minXAngle;
    [SerializeField] private float maxXAngle;
    [SerializeField] private float minRotationRadius;


    [Header("Start data")]
    [SerializeField] private float rotationRadius = 500;
    [SerializeField] private Vector3 origin;
    [SerializeField] private Vector2 orbitAngle;
    //[SerializeField] private Camera Camera;
    public float RotationRadius
    {
        get
        {
            return rotationRadius;
        }
        set
        {
            if (value >= minRotationRadius)
            {
                if (value <= maxHeight || maxHeight == 0)
                {
                    rotationRadius = value;
                }
                else
                {
                    rotationRadius = maxHeight;
                }
            }
            else
            {
                rotationRadius = minRotationRadius;
            }
        }
    }
    public Vector2 OrbitAngle
    {
        get { return orbitAngle; }
        private set
        {
            if(value.x <= maxXAngle && value.x >= minXAngle)
            {
                orbitAngle = value;
            }

        }
    }
    public bool ControlLocked { get; set; } = false;

    private const float DefaultRadius = 300;

    private void Start()
    {
        ResetRotation();
    }

    public void Zoom(float DeltaDistance)
    {
        if(DeltaDistance != 0)
        {
            RotationRadius -= DeltaDistance * (rotationRadius / DefaultRadius);
            ResetRotation();
        }
    }
    public void Moving(Vector3 Vector)
    {
        if (Vector != Vector3.zero)
        {
            Vector = Vector * (rotationRadius / DefaultRadius);
            LocalOriginTranslate(Vector);
            ResetRotation();
        }
    }
    public void Rotation(Vector2 OrbitDeltaAngle)
    {
        if (OrbitDeltaAngle != Vector2.zero)
        {
            OrbitAngle += OrbitDeltaAngle;

            float r = RotationRadius * Mathf.Cos(Mathf.Deg2Rad * OrbitAngle.x);
            float y = origin.y + (RotationRadius * Mathf.Sin(Mathf.Deg2Rad * OrbitAngle.x));

            //rotation origin arround Y axis
            Vector3 OriginY = new Vector3(origin.x, y, origin.z);

            float x = r * Mathf.Sin(Mathf.Deg2Rad * orbitAngle.y);
            float z = r * Mathf.Cos(Mathf.Deg2Rad * orbitAngle.y);

            Vector3 newPosition = new Vector3(OriginY.x - x, OriginY.y, OriginY.z - z);
            transform.position = newPosition;
            transform.LookAt(origin);
        }
    }

    private void LocalOriginTranslate(Vector3 vector)
    {
        this.origin.x -= vector.x * Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        this.origin.z += vector.x * Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);

        this.origin.z += vector.z * Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        this.origin.x += vector.z * Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);

        this.origin.y += vector.y;
    }
    private void TranslateY(float y)
    {

        float X = RotationRadius * Mathf.Cos(Mathf.Deg2Rad * OrbitAngle.x);
        float Y = (RotationRadius * Mathf.Sin(Mathf.Deg2Rad * OrbitAngle.x)) + y;
        float R = Mathf.Sqrt((Y * Y) + (X * X));
        float Angle = Mathf.Acos((X * R) / (R * R)) * Mathf.Rad2Deg;

        this.RotationRadius = R;
        this.OrbitAngle = new Vector2(Angle,OrbitAngle.y);

    }
    private void ResetRotation()
    {
        float r = RotationRadius * Mathf.Cos(Mathf.Deg2Rad * OrbitAngle.x);
        float y = origin.y + (RotationRadius * Mathf.Sin(Mathf.Deg2Rad * OrbitAngle.x));

        //rotation origin arround Y axis
        Vector3 OriginY = new Vector3(origin.x, y, origin.z);

        float x = r * Mathf.Sin(Mathf.Deg2Rad * OrbitAngle.y);
        float z = r * Mathf.Cos(Mathf.Deg2Rad * OrbitAngle.y);

        Vector3 newPosition = new Vector3(OriginY.x - x, OriginY.y, OriginY.z - z);
        transform.position = newPosition;
        transform.LookAt(origin);
    }

}

