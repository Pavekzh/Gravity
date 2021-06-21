﻿using UnityEngine;
using System.Collections;

namespace Assets.Library
{
    public abstract class FilePresenter : MonoBehaviour
    {
        public abstract RectTransform GetFileView(string path,Vector2 cellSize);

    }
}