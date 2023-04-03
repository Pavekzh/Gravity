using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.SceneEditor.Controllers
{
    public class ManipulatorsController : MonoBehaviour
    {
        public Dictionary<string, IManipulator> Manipulators { get; private set; } = new Dictionary<string, IManipulator>();
        public InputSystem InputSystem { get; set; }

        private List<IManipulator> enabledManipulators = new List<IManipulator>();

        public T EnableManipulator<T>(string key) where T: class, IManipulator
        {
            T resultManipulator = GetManipulator<T>(key);

            if (this.enabledManipulators.Any())
            {
                foreach(IManipulator manipulator in enabledManipulators)
                {
                    manipulator.DisableManipulator();
                }
                enabledManipulators.Clear();
            }

            enabledManipulators.Add(resultManipulator);
            resultManipulator.EnableManipulator(InputSystem);
            return resultManipulator;
        }

        public T EnableAdditionaryManipulator<T>(string key) where T: class, IManipulator
        {
            T resultManipulator = GetManipulator<T>(key);
            enabledManipulators.Add(resultManipulator);
            resultManipulator.EnableManipulator(InputSystem);
            return resultManipulator;
        }

        private T GetManipulator<T>(string key) where T : class, IManipulator
        {
            IManipulator requestedManipulator;
            if (Manipulators.TryGetValue(key, out requestedManipulator))
            {
                T resultManipulator = requestedManipulator as T;
                if (resultManipulator != null)
                {
                    return resultManipulator;
                }
                else
                    BasicTools.MessagingSystem.Instance.ShowErrorMessage("Requested manipulator " + key + " was invalid type", this);
            }
            else
                BasicTools.MessagingSystem.Instance.ShowErrorMessage("Manipulator " + key + " was not found", this);
            return null;
        }
    }
}