// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Memento.Example.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Reflection;

    public class MainViewModel : CustomDragDropEnabledViewModel
    {
        public MainViewModel(IMementoService mementoService)
            : base(mementoService)
        {
            Title = "Orc.Memento example";

            // disable state tracking of memento service while viewmodel will be initialized
            _mementoService.IsEnabled = false;

            UndoRedoEvents = new ObservableCollection<string>();
            Model = new MyModel(_mementoService);

            // reactivate state tracking of memento service
            _mementoService.IsEnabled = true;

            Undo = new Command(OnUndoExecute, OnUndoCanExecute);
            Redo = new Command(OnRedoExecute, OnRedoCanExecute);
            Save = new Command(OnSaveExecute, OnSaveCanExecute);
            AddData = new Command(OnAddDataExecute, OnAddDataCanExecute);
            AddSpecialData = new Command<SpecialDataClass>(OnAddSpecialDataExecute, OnAddSpecialDataCanExecute);
            AddSpecialDataToRoot = new Command(OnAddSpecialDataToRootExecute, OnAddSpecialDataToRootCanExecute);
        }

        #region Properties
        public ObservableCollection<string> UndoRedoEvents { get; private set; }

        public MyModel Model { get; private set; }
        #endregion

        #region Commands
        public Command Undo { get; private set; }

        private bool OnUndoCanExecute()
        {
            return _mementoService.CanUndo;
        }

        private void OnUndoExecute()
        {
            _mementoService.Undo();
        }

        public Command Redo { get; private set; }

        private bool OnRedoCanExecute()
        {
            return _mementoService.CanRedo;
        }

        private void OnRedoExecute()
        {
            _mementoService.Redo();
        }

        public Command Save { get; private set; }

        private bool OnSaveCanExecute()
        {
            return true;
        }

        private void OnSaveExecute()
        {
            // --> without effect on Model.Data
            //_mementoService.Clear(Model);

            _mementoService.Clear();
        }

        public Command AddData { get; private set; }

        private bool OnAddDataCanExecute()
        {
            return true;
        }

        private void OnAddDataExecute()
        {
            var dateTimeString = DateTime.Now.ToString();

            var dataId = dateTimeString;

            _mementoService.BeginBatch("New data", dataId);

            Model.Data.Add(dateTimeString);

            _mementoService.EndBatch();
        }

        public Command<SpecialDataClass> AddSpecialData { get; private set; }

        private bool OnAddSpecialDataCanExecute(SpecialDataClass selectedItem)
        {
            return true;
        }

        private void OnAddSpecialDataExecute(SpecialDataClass selectedItem)
        {
            if (selectedItem == null)
            {
                return;
            }

            var dateTimeString = DateTime.Now.ToLongTimeString();
            var dataId = $"_1_{dateTimeString}";

            _mementoService.BeginBatch("New data collection", dataId);

            if (selectedItem.NestedData == null)
            {
                selectedItem.NestedData = new ObservableCollection<SpecialDataClass>();
            }

            selectedItem.NestedData.Add(
                new SpecialDataClass(_mementoService)
                {
                    Data1 = dataId,
                    NestedData = new ObservableCollection<SpecialDataClass>
                    {
                        new SpecialDataClass(_mementoService) { Data1 = "_1_1_" + dateTimeString},
                        new SpecialDataClass(_mementoService) { Data1 = "_1_2_" + dateTimeString},
                        new SpecialDataClass(_mementoService) { Data1 = "_1_3_" + dateTimeString}
                    }
                });
            _mementoService.EndBatch();
        }

        public Command AddSpecialDataToRoot { get; private set; }

        private bool OnAddSpecialDataToRootCanExecute()
        {
            return true;
        }

        private void OnAddSpecialDataToRootExecute()
        {
            var dateTimeString = DateTime.Now.ToLongTimeString();

            var dataId = $"root_1_{dateTimeString}";

            _mementoService.BeginBatch("TreeItem", dataId);

            Model.DataCollection.Add(
             new SpecialDataClass(_mementoService)
             {
                 Data1 = dataId,
                 NestedData = new System.Collections.ObjectModel.ObservableCollection<SpecialDataClass>
                 {
                    new SpecialDataClass(_mementoService) { Data1 = "root_1_1_" + dateTimeString },
                    new SpecialDataClass(_mementoService) { Data1 = "root_1_2_" + dateTimeString},
                    new SpecialDataClass(_mementoService) { Data1 = "root_1_3_" + dateTimeString}
                 }
             });
            _mementoService.EndBatch();
        }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _mementoService.Updated += OnMementoServiceUpdated;
        }

        protected override async Task CloseAsync()
        {
            _mementoService.Updated -= OnMementoServiceUpdated;

            await base.CloseAsync();
        }

        private void OnMementoServiceUpdated(object sender, MementoEventArgs e)
        {
            UndoRedoEvents.Clear();

            var allEvents = _mementoService.UndoBatches.Where(ev => !ev.IsEmptyBatch);
            foreach (var mementoEvent in allEvents.Reverse())
            {
                if (mementoEvent.IsSingleActionBatch)
                {
                    var propertyChangeAction = mementoEvent.Actions.First() as PropertyChangeUndo;
                    if (propertyChangeAction != null)
                    {
                        var message = $"changed {propertyChangeAction.Target}.{propertyChangeAction.PropertyName}";
                        UndoRedoEvents.Add(message);
                        continue;
                    }

                    var collectionChangedAction = mementoEvent.Actions.First() as CollectionChangeUndo;
                    if (collectionChangedAction != null)
                    {
                        var message = $"{collectionChangedAction.ChangeType} {mementoEvent.Title} {mementoEvent.Description}";
                        UndoRedoEvents.Add(message);
                        continue;
                    }

                    UndoRedoEvents.Add("unknown action type");
                }
                else
                {
                    var message = $"{mementoEvent.Title} {mementoEvent.Description}";
                    UndoRedoEvents.Add(message);
                }
            }

            ViewModelCommandManager.InvalidateCommands(true);
        }
        #endregion
    }
}
