using System.Collections;
using UnityEngine;

namespace Assets.Controllers
{
    public abstract class PanelController : MonoBehaviour
    {
        public abstract void Open();
        public abstract void Close();
    }
}