using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Assets.UIExtended
{
    public class DoubleTapEventTrigger : MonoBehaviour,IPointerDownHandler
    {
        [SerializeField] UnityEvent onDoubleTap;
        [SerializeField] float maxTapDeltaTime = 0.5f;


        int tapCount = 0;

        public void OnPointerDown(PointerEventData eventData)
        {            
            tapCount++;

            if (tapCount == 1)
                StartCoroutine(Timer());

            if (tapCount == 2)
                onDoubleTap?.Invoke();
        }


        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(maxTapDeltaTime);
            tapCount = 0;
        }
    }
}