using System;
using UnityEngine;

namespace BasicTools
{
    public abstract class LoadingProgressDisplay:MonoBehaviour
    {
        public abstract void Display();

        public abstract void Display(float progress);

        public abstract void Hide();
    }
}
