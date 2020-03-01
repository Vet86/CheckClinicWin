namespace CheckClinicUI.Base
{
    public class ViewModelBase<T> : NotifyPropertyChangedBase
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

    public class ViewModelBase : NotifyPropertyChangedBase
    {
        public ViewModelBase()
        {
        }
    }
}
