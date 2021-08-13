using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Controllers
{
    public abstract class EditorTool:MonoBehaviour
    {
        //used to off input reaction
        public abstract void DisableTool();
        //used to subscribe methods to inputSystem
        public abstract void EnableTool(InputController inputSystem);
    }
}
