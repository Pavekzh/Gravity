using BasicTools;

namespace Assets.SceneEditor.Models
{
    public class CommonPropertyViewData<T> : PropertyViewData
    {
        private ConvertibleBinding<T, string[]> binding;

        public CommonPropertyViewData(){}

        public CommonPropertyViewData(ConvertibleBinding<T, string[]> binding, string name, string[] components)
        {
            Binding = binding;
            Name = name;
            Components = components;
        }

        public ConvertibleBinding<T, string[]> Binding 
        {
            get => binding;
            set
            {
                binding = value;
            }
        }
        public override string Name { get; set; }
        public override string[] Components { get; set; }

        public override void ChangePresenter(string[] dataProvider, object source)
        {
            Binding.ChangePresenter(dataProvider, source);
        }

        public override event ValueChangedHandler<string[]> ValueChanged
        {
            add
            {
                Binding.PresenterChanged += value;
                Binding.ForceUpdate();
            }
            remove
            {
                Binding.PresenterChanged += value;
            }
        }
    }
}
