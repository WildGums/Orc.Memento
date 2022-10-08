namespace Orc.Memento
{
    using System;

    public class MementoEventArgs : EventArgs
    {
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

        public IMementoSupport? Target { get; }

        public IMementoBatch? TargetBatch { get; }

        public MementoAction MementoAction { get; }
    }
}
