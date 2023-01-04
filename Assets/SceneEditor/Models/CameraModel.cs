using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using UnityEngine;
using BasicTools;

namespace Assets.SceneEditor.Models
{
    [Serializable]
    public class CameraModel:ICloneable
    {
        public delegate void ZoomChangedEventHandler(float value, object sender);
        public delegate void OriginMovedEventHandler(Vector3 value, object sender);
        public delegate void RotationChangedEventHandler(Vector2 value, object sender);
        public event ZoomChangedEventHandler ZoomChanged;
        public event OriginMovedEventHandler OriginMoved;
        public event RotationChangedEventHandler RotationChanged;

        public static string Key = "KEY_CameraModel";

        [XmlIgnore]
        public CameraStartupData StartupData { get; set; } = new CameraStartupData(false);
        [XmlIgnore]
        public Camera Camera { get; set; }

        private Vector3 origin;
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
                if (StartupData.UseConstraints)
                {
                    if (value >= StartupData.MinRotationRadius)
                    {
                        if (value <= StartupData.MaxRotationRadius || StartupData.MaxRotationRadius == 0)
                        {
                            rotationRadius = value;
                            ZoomChanged?.Invoke(value, this);
                        }
                        else
                        {
                            rotationRadius = StartupData.MaxRotationRadius;
                            ZoomChanged?.Invoke(value, this);
                        }
                    }
                    else
                    {
                        rotationRadius = StartupData.MinRotationRadius;
                        ZoomChanged?.Invoke(value, this);
                    }
                }
                else
                {
                    rotationRadius = value;
                    ZoomChanged?.Invoke(value, this);
                }

                UpdateCamera();
            }
        }
        public Vector2 OrbitAngle
        {
            get { return orbitAngle; }
            set
            {
                if (StartupData.UseConstraints)
                {
                    if (value.x <= StartupData.MaxXAngle && value.x >= StartupData.MinXAngle)
                    {
                        orbitAngle = value;
                        RotationChanged?.Invoke(value, this);
                    }
                }
                else
                {
                    orbitAngle = value;
                    RotationChanged?.Invoke(value, this);
                }
                UpdateCamera();
            }
        }
        public Vector3 Origin
        {
            get => origin;
            set
            {
                origin = value;
                OriginMoved?.Invoke(origin, value);
                UpdateCamera();
            }
        }

        [XmlIgnore]
        public bool ControlLocked { get; set; }

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

        public CameraModel(Camera camera,CameraStartupData StartupData)
        {
            this.Camera = camera;
            this.StartupData = StartupData;

#if UNITY_EDITOR
            if (StartupData.OrbitAngle.x < StartupData.MinXAngle)
                Debug.LogError("Default orbit angle can`t be less then min angle");
            else if (StartupData.OrbitAngle.x > StartupData.MaxXAngle)
                Debug.LogError("Default orbit angle can`t be greater then max angle");
            if (StartupData.RotationRadius > StartupData.MaxRotationRadius)
                Debug.LogError("Default rotation radius can`t be greater then max radius");
#endif


            this.Origin = StartupData.Origin;
            this.OrbitAngle = StartupData.OrbitAngle;
            this.RotationRadius = StartupData.RotationRadius;

            Debug.LogWarning("Adding to dataStorage: CameraModel.122");
            DataStorage.Instance.SaveData(Key, this);
        }

        public CameraModel()
        {

        }

        public void Zoom(float DeltaDistance)
        {
            if (DeltaDistance != 0 && ControlLocked == false)
            {
                RotationRadius -= DeltaDistance * (rotationRadius / DefaultRadius);
                UpdateCamera();
            }
        }
        public void Moving(Vector3 Vector)
        {
            if (Vector != Vector3.zero && ControlLocked == false)
            {
                Vector = Vector * (rotationRadius / DefaultRadius);
                Quaternion rotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
                this.origin += rotationY * Vector;
                UpdateCamera();
            }
        }
        public void Rotation(Vector2 OrbitDeltaAngle)
        {
            if (OrbitDeltaAngle != Vector2.zero && ControlLocked == false)
            {
                OrbitAngle += OrbitDeltaAngle;
                UpdateCamera();
            }
        }
        public void UpdateCamera()
        {
            if(Camera != null)
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

        public void ResetRotation()
        {
            this.OrbitAngle = StartupData.OrbitAngle;
        }
        public void ResetTransform()
        {
            this.Origin = StartupData.Origin;
            this.OrbitAngle = StartupData.OrbitAngle;
            this.RotationRadius = StartupData.RotationRadius;
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

        public object Clone()
        {
            CameraModel cloned = new CameraModel();
            cloned.origin = origin;
            cloned.orbitAngle = orbitAngle;
            cloned.rotationRadius = rotationRadius;

            return cloned;
        }
    }

}

