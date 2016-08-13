// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MementoEventArgs.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Memento
{
    using System;

    public class MementoEventArgs : EventArgs
    {
        #region Constructors
        public MementoEventArgs(MementoAction mementoAction)
        {
            MementoAction = mementoAction;
        }
        #endregion

        #region Properties
        public MementoAction MementoAction { get; }
        #endregion
    }
}