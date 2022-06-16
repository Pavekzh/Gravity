using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public abstract class EditorTool:MonoBehaviour
    {
        public abstract string ToolName { get; }
        //used to off input reaction
        public abstract void DisableTool();
        //used to subscribe methods to inputSystem
        public abstract void EnableTool(InputSystem inputSystem);
        //template method
        private void OnDestroy()
        {
            DisableTool();
        }
    }
}
