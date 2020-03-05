using PropertyChanged;

namespace CheckClinicUI.Base
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModelBase<T>
    {
        public ViewModelBase()
        {
        }

        public ViewModelBase(T model)
        {
            Model = model;
        }

        public T Model { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class ViewModelBase
    {
        public ViewModelBase()
        {
        }
    }
}
