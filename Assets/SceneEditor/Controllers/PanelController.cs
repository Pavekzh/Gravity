using System.Collections;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public abstract class PanelController : MonoBehaviour
    {
        public abstract void Open();
        public abstract void Close();
    }
}