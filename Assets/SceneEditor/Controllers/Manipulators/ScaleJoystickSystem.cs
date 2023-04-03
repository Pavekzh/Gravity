using System;
using UnityEngine;
using BasicTools;
using UIExtended;

namespace Assets.SceneEditor.Controllers
{
    public class ScaleJoystickSystem:ScalarJoystickSystem
    {
        [SerializeField] protected DragInputManipulator<Vector3> dragManipulator;

        public static string DefaultKey => "ScaleJoystickSystem";
        public override string ManipulatorKey => DefaultKey;

        public override DragInputManipulator<Vector3> DragManipulator { get => dragManipulator; }
    }
}
