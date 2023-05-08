using System.Collections;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public class ClearSceneController : MonoBehaviour
    {
        private Services.SceneInstance sceneInstance;

        [Zenject.Inject]
        private void Construct(Services.SceneInstance sceneInstance)
        {
            this.sceneInstance = sceneInstance;
        }

        public void ClearScene()
        {
            sceneInstance.ResetScene();
        }
    }
}