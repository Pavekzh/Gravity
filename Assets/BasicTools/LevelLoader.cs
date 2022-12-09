using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BasicTools
{
    public class LevelLoader:MonoBehaviour
    {
        [SerializeField] protected LoadingProgressDisplay progressDisplay;

        public LoadingProgressDisplay ProgressDisplay { get => progressDisplay; set => progressDisplay = value; }

        public void LoadLevel(int sceneIndex)
        {
            Display();
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }

        IEnumerator LoadAsynchronously(int sceneIndex)
        {
            Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            operation.allowSceneActivation = false;
            while(operation.progress < 0.9f)
            {
                Display(operation.progress);
                yield return null;
            }
            Display(operation.progress);
            operation.allowSceneActivation = true;
        }

        void Display()
        {
            if (progressDisplay != null)
                progressDisplay.Display();
        }

        void Display(float progress)
        {
            if (progressDisplay != null)
                progressDisplay.Display(progress);
        }
    }
}
