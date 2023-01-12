﻿namespace Orc.Memento.Tests.Mocks
{
    using Catel.Data;

    public class MockModel : ObservableObject
    {
        #region Fields
        private string _value;
        #endregion

        #region Properties
        public static string Name { get; private set; }

        public string Value
        {
            get { return _value; }
            set
            {
                if (!string.Equals(_value, value))
                {
                    _value = value;
                    RaisePropertyChanged(nameof(Value));
                }
            }
        }
        #endregion

        #region Methods
        public static void Change(string value)
        {
            Name = value;
        }
        #endregion
    }
}