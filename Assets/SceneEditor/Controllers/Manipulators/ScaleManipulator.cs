using System;
using UnityEngine;
using BasicTools;
using UIExtended;

namespace Assets.SceneEditor.Controllers
{
    public class ScaleManipulator : FloatJoystickSystem
    {
        [SerializeField] DragInputManipulator inputDetector;
        [SerializeField] float dragTouchDistance = 0;
        [SerializeField] bool useRelativeTouchScale;
        [SerializeField] float touchRelativeScale = 0.1f;


        public new static string DefaultKey => "ScaleManipulator";

        public event Action DragInputStarted
        {
            add
            {
                inputDetector.InputReadingStarted += value;
            }
            remove
            {
                inputDetector.InputReadingStarted -= value;
            }
        }
        public event Action DragInputEnded
        {
            add
            {
                inputDetector.InputReadingStoped += value;
            }
            remove
            {
                inputDetector.InputReadingStoped -= value;
            }
        }

        private Binding<Vector2> originBinding;
        public Binding<Vector2> OriginBiding 
        {
            get => originBinding;
            set
            {
                if(originBinding == null && value != null)
                {                
                    originBinding = value;
                    enableDragInput();
                }
                if(originBinding != null)
                {

                    if (value == null)
                        disableDragInput();
                    else
                        inputDetector.OriginBinding = value;                    
                    originBinding = value;
                }

            }
        }

        protected override void Start()
        {
            if (manipulatorName != "")
                EditorController.Instance.ManipulatorsController.Manipulators.Add(manipulatorName, this);
            else
                EditorController.Instance.ManipulatorsController.Manipulators.Add(ScaleManipulator.DefaultKey, this);

            EditorController.Instance.Camera.ZoomChanged += zoomChanged;
            this.InputBinding.ValueChanged += exteranlValueChanged;
        }

        private void exteranlValueChanged(float value, object source)
        {
            if(source != (System.Object)this && OriginBiding != null)
            {
                inputDetector.InputBinding.ChangeValue(new Vector3(0,0,value + dragTouchDistance), this);
            }
        }

        private void zoomChanged(float value, object sender)
        {
            if(isEnabled && InputBinding != null)
            {        
                inputDetector.ScaleFactor = EditorController.Instance.Camera.ScaleFactor;
            }
        }

        protected override void DoEnable()
        {
            base.DoEnable();
            enableDragInput();
        }
        protected override void DoDisable()
        {
            base.DoDisable();
            disableDragInput();
        }

        private void dragInput(Vector3 value, object source)
        {
            if(source != (System.Object)this)
                this.InputBinding.ChangeValue(value.magnitude - dragTouchDistance, this);
        }

        private void enableDragInput()
        {
            if(OriginBiding != null && isEnabled)
            {
                inputDetector.ScaleFactor = EditorController.Instance.Camera.ScaleFactor;
                inputDetector.Enable(OriginBiding);
                DragInputStarted += dragInputStarted;
                DragInputEnded += dragInputEnded;
                inputDetector.InputBinding.ValueChanged += dragInput;
            }

        }

        private void disableDragInput()
        {
            if(originBinding != null)
            {
                inputDetector.Disable();
                DragInputStarted -= dragInputStarted;
                DragInputEnded -= dragInputEnded;
                inputDetector.InputBinding.ValueChanged -= dragInput;
            }

        }        

        private void dragInputEnded()
        {
            straightJoystick.Enable(null);
        }

        private void dragInputStarted()
        {
            straightJoystick.Disable();     
        }
    }
}
