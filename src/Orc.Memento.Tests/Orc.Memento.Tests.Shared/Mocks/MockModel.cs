// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Memento.Tests.Mocks
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
                _value = value;
                RaisePropertyChanged(() => Value);
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