using System;
using UnityEngine;

namespace Assets.SceneEditor.Models
{
    [Serializable]
    public struct CameraStartupData
    {

        [Header("Constraints")]
        public bool UseConstraints;
        public float MaxRotationRadius;
        public float MinRotationRadius;
        public float MinXAngle;
        public float MaxXAngle;

        [Header("Default data")]
        public float RotationRadius;
        public Vector3 Origin;
        public Vector2 OrbitAngle;

        public CameraStartupData(bool useConstraints)
        {
            UseConstraints = useConstraints;
            MaxRotationRadius = 0;
            MinRotationRadius = 0;
            MinXAngle = -30;
            MaxXAngle = 89.9999f;
            RotationRadius = 1;
            Origin = Vector3.zero;
            OrbitAngle = new Vector2(60,0);
        }
    }
}
