using BasicTools;
using UnityEngine;
using UnityEngine.UI;
using Assets.Services;

namespace Assets.Menu.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    class SettingsController:MonoBehaviour
    {
        [SerializeField] StateChanger opener;
        [SerializeField] new Services.AudioSettings audio;

        RectTransform rectTransform;
        RectTransform RectTransform
        {
            get 
            {
                if (rectTransform != null)
                    return rectTransform;
                else
                    return rectTransform = GetComponent<RectTransform>();
            }
        }

        public void Open()
        {
            opener.State = State.Changed;
        }

        public void Close()
        {
            opener.State = State.Default;
        }

        public void ChangeState()
        {
            if (opener.State == State.Default)
                Open();
            else if(opener.State == State.Changed)
                Close();
        }

        private void Update()
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId) && !RectTransformUtility.RectangleContainsScreenPoint(RectTransform, touch.position))
                {
                    this.Close();
                }
            }
        }
    }
}
