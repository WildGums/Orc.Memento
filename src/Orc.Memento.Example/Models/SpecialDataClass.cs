namespace Orc.Memento.Example
{
    using Orc.Memento;
    using System;
    using System.Collections.ObjectModel;

    public class SpecialDataClass : MementoModelBase
    {
        public SpecialDataClass(IMementoService mementoService)
            : base(mementoService)
        {
        }

        public string Data1 { get; set; }

        public string Data3 { get; set; }

        public ObservableCollection<SpecialDataClass> NestedData { get; set; }
    }
}
