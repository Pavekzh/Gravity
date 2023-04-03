using System;
using BasicTools;

namespace Assets.SceneEditor.Controllers
{
    public interface IManipulator
    {
        string ManipulatorKey { get; }

        void EnableManipulator(InputSystem inputSystem);
        void DisableManipulator();
    }
}
