namespace Orc.Memento.Example;

using Memento;
using System.Collections.ObjectModel;

public class SpecialDataClass : MementoModelBase
{
    public SpecialDataClass(IMementoService mementoService)
        : base(mementoService)
    {
    }

    public string Data1 { get; set; }

    public string Data3 { get; set; }

    public ObservableCollection<SpecialDataClass>? NestedData { get; set; }
}
