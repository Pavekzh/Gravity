using UnityEngine;
using System.Collections;

namespace Assets.Library
{
    public abstract class FilePathProvider : MonoBehaviour
    {
        public delegate void PathChangedHandler(string path, object sender);
        public abstract event PathChangedHandler PathChanged;
        public abstract void Init(string sourceDirectory,string extension);
    }
}

