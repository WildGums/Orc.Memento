namespace Orc.Memento.Example;

using Orc.Memento;
using System.Linq;
using System.Collections.ObjectModel;
using Catel.Data;

public class MyModel : MementoModelBase
{
    public MyModel(IMementoService mementoService)
        : base(mementoService)
    {
        Name = "John";
        LastName = "Doe";

        Data = new ObservableCollection<string>();
        DataCollection = new ObservableCollection<SpecialDataClass>
        {
            new SpecialDataClass(_mementoService)
            {
                Data1 = "string data 1",
                Data3 = "Teststring",
                NestedData = new ObservableCollection<SpecialDataClass>
                {
                    new SpecialDataClass(_mementoService) { Data1 = "string data 1_1", Data3 = "Teststring" },
                    new SpecialDataClass(_mementoService)
                    {
                        Data1 = "string data 1_2",
                        Data3 = "Teststring",
                        NestedData = new ObservableCollection<SpecialDataClass>
                        {
                            new SpecialDataClass(_mementoService) { Data1 = "1_2_1", Data3 = "Teststring" },
                            new SpecialDataClass(_mementoService) { Data1 = "1_2_3", Data3 = "Teststring" },
                            new SpecialDataClass(_mementoService) { Data1 = "1_2_3", Data3 = "Teststring" },
                        }
                    },
                    new SpecialDataClass(_mementoService) { Data1 = "string data 1_3", Data3 = "Teststring" },
                }
            },
            new SpecialDataClass(_mementoService)
            {
                Data1 = "string data 2",
                NestedData = new ObservableCollection<SpecialDataClass>
                {
                    new SpecialDataClass(_mementoService) { Data1 = "string data 2_1", Data3 = "Teststring" },
                    new SpecialDataClass(_mementoService) { Data1 = "string data 2_2", Data3 = "Teststring" },
                    new SpecialDataClass(_mementoService) { Data1 = "string data 2_3", Data3 = "Teststring" },
                }
            },
        };
    }

    public string Name { get; set; }

    public string LastName { get; set; }

    public ObservableCollection<string> Data { get; }

    public ObservableCollection<SpecialDataClass> DataCollection { get; }
}
