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

        public MementoEventArgs(MementoAction mementoAction, IMementoSupport target)
        {
            MementoAction = mementoAction;
            Target = target;
        }

        public MementoEventArgs(MementoAction mementoAction, IMementoBatch targetBatch)
        {
            MementoAction = mementoAction;
            TargetBatch = targetBatch;
        }
        #endregion

        #region Properties
        public IMementoSupport Target { get; }

        public IMementoBatch TargetBatch { get; }

        public MementoAction MementoAction { get; }
        #endregion
    }
}
