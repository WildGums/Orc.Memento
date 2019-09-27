namespace Orc.Memento.Example
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Catel.Data;

    public class MementoModelBase : ModelBase, IDisposable
    {
        private const string COLLECTION_INDICATOR = "_Collection";
        private const string OBJECT_INDICATOR = "_Object";

        private readonly Dictionary<string, object> _mementoHashtable = new Dictionary<string, object>();
        protected readonly IMementoService _mementoService;

        public MementoModelBase(IMementoService mementoService)
        {
            _mementoService = mementoService;

            RegisterCallerForStateTracking(null);
        }

        protected virtual void UnLoad()
        {
            foreach (var kvp in _mementoHashtable)
            {
                if (kvp.Key.Contains(COLLECTION_INDICATOR))
                {
                    _mementoService.UnregisterCollection((INotifyCollectionChanged)kvp.Value);
                }
                else
                {
                    _mementoService.UnregisterObject((INotifyPropertyChanged)kvp.Value);
                }
            }

            _mementoHashtable.Clear();
        }

        protected override void RaisePropertyChanged(object sender, AdvancedPropertyChangedEventArgs e)
        {
            base.RaisePropertyChanged(sender, e);

            RegisterCallerForStateTracking(e.PropertyName);
        }

        private void RegisterCallerForStateTracking(string caller)
        {
            var instanceType = TypeDescriptor.GetReflectionType(this);
            var propertyInfo = caller != null ? instanceType.GetProperty(caller) : null;

            var dateTime = DateTime.Now;
            var infoString = $"{propertyInfo?.ReflectedType.Name}.{propertyInfo?.Name}";
            var key = $"{dateTime.ToLongTimeString()}.{dateTime.Millisecond} {(infoString.Length > 1 ? infoString : $"{dateTime.ToLongTimeString()}.{dateTime.Millisecond} {GetType().ToString()}")}";

            if (propertyInfo is null)
            {
                // in case of Collection.Add the constructor of added class should fire NotifyPropertyChanged 
                _mementoService.RegisterObject(this);
                _mementoHashtable.Add(key, this);
                return;
            }

            var propertyValueType = propertyInfo.GetValue(this);
            if (propertyValueType is INotifyCollectionChanged notifyCollectionChanged)
            {
                // double registration will be checked by MementoService it self
                _mementoService.RegisterCollection(notifyCollectionChanged);
                key += COLLECTION_INDICATOR;
            }
            else if (propertyValueType is INotifyPropertyChanged notifyPropertyChanged)
            {
                // double registration will be checked by MementoService it self
                _mementoService.RegisterObject(notifyPropertyChanged);
                key += OBJECT_INDICATOR;
            }

            _mementoHashtable.Add(key, propertyValueType);
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                UnLoad();
                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ModelBase() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
