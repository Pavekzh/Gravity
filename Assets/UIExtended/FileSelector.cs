using UnityEngine;
using System.Collections;
using TMPro;
using BasicTools;
using UnityEngine.EventSystems;

namespace UIExtended
{
    public class FileSelector : StateChanger
    {
        [SerializeField] private StateChanger StateChanger;
        [SerializeField] private string filePath;
        [SerializeField] private Binding<string> pathBinding;

        public Binding<string> PathBinding
        {
            get => pathBinding;
            set
            {
                if (pathBinding != null)
                    pathBinding.ValueChanged -= SelectedFileChanged;

                pathBinding = value;
                if (pathBinding != null)
                    pathBinding.ValueChanged += SelectedFileChanged;
            }
        }
        public string FilePath { get => filePath; set => filePath = value; }
        public override State State
        {
            get { return state; }
            set
            {
                state = value;
                StateChanger.State = value;
                if (value == State.Changed)
                {
                    Select();
                }
            }
        }

        private void OnDestroy()
        {
            if(pathBinding != null)
                pathBinding.ValueChanged -= this.SelectedFileChanged;
        }

        private void Select()
        {
            PathBinding.ChangeValue(FilePath, this);         
        }

        public void SelectedFileChanged(string selectedPath, object sender)
        {
            if (sender != (object)this)
            {
                this.StateChanger.State = State.Default;
                this.state = State.Default;
            }
        }
    }
}