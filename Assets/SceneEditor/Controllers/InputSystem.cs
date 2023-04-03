using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.SceneEditor.Controllers
{
    public class InputSystem : MonoBehaviour
    {
        public delegate void SeveralTouches(Touch[] touches);
        public delegate void SomeTouchChanged(Touch touch, Touch[] allTouches);
        public delegate void TouchChanged(Touch touch);
        public delegate void UITouched();

        public event UITouched OnUIRelease;
        public event UITouched OnUITouch;

        public event TouchChanged OnTouchDown;
        public event TouchChanged OnTouchContinues;
        public event TouchChanged OnTouchRelease;
        public event SeveralTouches OnTwoTouchesDown;
        public event SeveralTouches OnTwoTouchesContinue;
        public event SeveralTouches OnTwoTouchesRelease;

        public bool IsInputEnabled 
        {
            get => isInputEnabled;
            set
            {
                if (!isInputReadingLocked)
                {
                    isInputEnabled = value;
                }
            }
        }

        bool isInputEnabled = true;
        bool isInputReadingLocked;
        List<int> UITouches = new List<int>();

        //we need to detect touch down and touch release in Update but move or stationary in FixedUpdate
        private void Update()
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    //UI touch detection
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId) && !UITouches.Contains(touch.fingerId))
                    {
                        IsInputEnabled = false;
                        OnUITouch?.Invoke();
                        UITouches.Add(touch.fingerId);
                    }

                    
                    if (IsInputEnabled)
                    {
                        //messages one-per-one touch
                        if (touch.phase == TouchPhase.Began && Input.touchCount == 1)
                            this.OnTouchDown?.Invoke(touch);
                        if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && Input.touchCount == 1)
                            this.OnTouchRelease?.Invoke(touch);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        UITouches.Remove(touch.fingerId);
                        if (UITouches.Count == 0)
                        {
                            IsInputEnabled = true;
                            //All UI touches released
                            OnUIRelease?.Invoke();                            
                        }
                    }



                }

                //messages one-for-several touches
                if (IsInputEnabled)
                {
                    if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began))
                        OnTwoTouchesDown?.Invoke(new Touch[] { Input.GetTouch(0), Input.GetTouch(1) });
                    if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(1).phase == TouchPhase.Canceled))
                        OnTwoTouchesRelease?.Invoke(new Touch[] { Input.GetTouch(0), Input.GetTouch(1) });
                }
            }
        }

        private void FixedUpdate()
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);


                    if (IsInputEnabled)
                    {
                        //sending messages
                        if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && Input.touchCount == 1)
                            this.OnTouchContinues?.Invoke(touch);
                    }

                }
                if (IsInputEnabled)
                {
                    if (Input.touchCount == 2)
                        OnTwoTouchesContinue?.Invoke(new Touch[] { Input.GetTouch(0), Input.GetTouch(1) });
                }
            }
        }
    
        public void LockInputReading(bool isInputEnabled)
        {
            IsInputEnabled = isInputEnabled;
            isInputReadingLocked = true;
        }

        public void UnlockInputReading()
        {
            isInputReadingLocked = false;
        }
    }
}
