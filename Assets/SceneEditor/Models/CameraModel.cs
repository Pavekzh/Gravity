using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;

namespace Assets.SceneEditor.Models
{
    public class CameraModel : MonoBehaviour
    {
        public delegate void ZoomChangedEventHandler(float value, object sender);
        public delegate void OriginMovedEventHandler(Vector3 value, object sender);
        public delegate void RotationChangedEventHandler(Vector2 value, object sender);
        public event ZoomChangedEventHandler ZoomChanged;
        public event OriginMovedEventHandler OriginMoved;
        public event RotationChangedEventHandler RotationChanged;

        public static string Key = "KEY_CameraModel";

        [Header("Limiters")]
        [SerializeField] private float maxRadius = 100;
        [SerializeField] private float minXAngle = 0;
        [SerializeField] private float maxXAngle = 89.99f;
        [SerializeField] private float minRotationRadius = 1;


        [Header("Start data")]
        [SerializeField] private float defaultRotationRadius = 10;
        [SerializeField] private Vector3 defaultOrigin;
        [SerializeField] private Vector2 defaultOrbitAngle = new Vector2(60, 0);
        [SerializeField] private Camera Camera;

        [SerializeField]private Vector3 origin;
        private Vector2 orbitAngle;
        private float rotationRadius;

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
                    if (value <= maxRadius || maxRadius == 0)
                    {
                        rotationRadius = value;
                        ZoomChanged?.Invoke(value, this);
                    }
                    else
                    {
                        rotationRadius = maxRadius;
                        ZoomChanged?.Invoke(value, this);
                    }
                }
                else
                {
                    rotationRadius = minRotationRadius;
                    ZoomChanged?.Invoke(value, this);
                }
            }
        }
        public Vector2 OrbitAngle
        {
            get { return orbitAngle; }
            private set
            {
                if (value.x <= maxXAngle && value.x >= minXAngle)
                {
                    orbitAngle = value;
                    RotationChanged?.Invoke(value, this);
                }

            }
        }
        public Vector3 Origin
        {
            get => origin;
            set
            {
                this.origin.x -= value.x * Mathf.Cos(Mathf.Deg2Rad * Camera.transform.rotation.eulerAngles.y);
                this.origin.z += value.x * Mathf.Sin(Mathf.Deg2Rad * Camera.transform.rotation.eulerAngles.y);

                this.origin.z += value.z * Mathf.Cos(Mathf.Deg2Rad * Camera.transform.rotation.eulerAngles.y);
                this.origin.x += value.z * Mathf.Sin(Mathf.Deg2Rad * Camera.transform.rotation.eulerAngles.y);

                this.origin.y += value.y;
                OriginMoved?.Invoke(origin, value);
            }
        }
        public bool ControlLocked { get; set; } = false;

        public Vector2 SpaceRectOfView
        {
            get
            {
                float height = 2 * RotationRadius * Mathf.Tan((Camera.fieldOfView * 0.5f) * Mathf.Deg2Rad);
                float width = height * Camera.aspect;

                return new Vector2(width, height);
            }
        }

        public float DefaultRadius { get => rotationRadius / (SpaceRectOfView.x / Screen.width); }
        public float ScaleFactor { get => rotationRadius / DefaultRadius; }

        private void Start()
        {

#if UNITY_EDITOR
            if (defaultOrbitAngle.x < minXAngle)
                Debug.LogError("Default orbit angle can`t be less then min angle");
            else if (defaultOrbitAngle.x > maxXAngle)
                Debug.LogError("Default orbit angle can`t be greater then max angle");
            if(defaultRotationRadius > maxRadius)
                Debug.LogError("Default rotation radius can`t be greater then max radius");
#endif

            this.origin = defaultOrigin;
            this.OrbitAngle = defaultOrbitAngle;
            this.RotationRadius = defaultRotationRadius;

            Debug.LogWarning("Adding to dataStorage: CameraModel.83");
            DataStorage.Instance.SaveData(Key, this);
            UpdateRotation();
        }

        public void Zoom(float DeltaDistance)
        {
            if (DeltaDistance != 0 && ControlLocked == false)
            {
                RotationRadius -= DeltaDistance * (rotationRadius / DefaultRadius);
                UpdateRotation();
            }
        }
        public void Moving(Vector3 Vector)
        {
            if (Vector != Vector3.zero && ControlLocked == false)
            {
                Vector = Vector * (rotationRadius / DefaultRadius);
                Origin = Vector;
                UpdateRotation();
            }
        }
        public void Rotation(Vector2 OrbitDeltaAngle)
        {
            if (OrbitDeltaAngle != Vector2.zero && ControlLocked == false)
            {
                OrbitAngle += OrbitDeltaAngle;

                float r = RotationRadius * Mathf.Cos(Mathf.Deg2Rad * OrbitAngle.x);
                float y = origin.y + (RotationRadius * Mathf.Sin(Mathf.Deg2Rad * OrbitAngle.x));

                //rotation origin arround Y axis
                Vector3 OriginY = new Vector3(origin.x, y, origin.z);

                float x = r * Mathf.Sin(Mathf.Deg2Rad * orbitAngle.y);
                float z = r * Mathf.Cos(Mathf.Deg2Rad * orbitAngle.y);

                Vector3 newPosition = new Vector3(OriginY.x - x, OriginY.y, OriginY.z - z);
                Camera.transform.position = newPosition;
                Camera.transform.LookAt(origin);
            }
        }

        public void ResetRotation()
        {
            this.OrbitAngle = defaultOrbitAngle;
            UpdateRotation();
        }
        public void ResetTransform()
        {
            this.origin = defaultOrigin;
            this.OrbitAngle = defaultOrbitAngle;
            this.RotationRadius = defaultRotationRadius;
            UpdateRotation();
        }

        private void TranslateY(float y)
        {

            float X = RotationRadius * Mathf.Cos(Mathf.Deg2Rad * OrbitAngle.x);
            float Y = (RotationRadius * Mathf.Sin(Mathf.Deg2Rad * OrbitAngle.x)) + y;
            float R = Mathf.Sqrt((Y * Y) + (X * X));
            float Angle = Mathf.Acos((X * R) / (R * R)) * Mathf.Rad2Deg;

            this.RotationRadius = R;
            this.OrbitAngle = new Vector2(Angle, OrbitAngle.y);
        }
        private void UpdateRotation()
        {
            float r = RotationRadius * Mathf.Cos(Mathf.Deg2Rad * OrbitAngle.x);
            float y = origin.y + (RotationRadius * Mathf.Sin(Mathf.Deg2Rad * OrbitAngle.x));

            //rotation origin arround Y axis
            Vector3 OriginY = new Vector3(origin.x, y, origin.z);

            float x = r * Mathf.Sin(Mathf.Deg2Rad * OrbitAngle.y);
            float z = r * Mathf.Cos(Mathf.Deg2Rad * OrbitAngle.y);

            Vector3 newPosition = new Vector3(OriginY.x - x, OriginY.y, OriginY.z - z);
            Camera.transform.position = newPosition;
            Camera.transform.LookAt(origin);
        }

    }

}

