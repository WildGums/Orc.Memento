namespace Orc.Memento.Tests.Mocks
{
    public class MockUndo : IMementoSupport
    {
        #region Fields
        public bool UndoCalled, RedoCalled;
        #endregion

        #region Constructors
        public MockUndo(bool canRedo = false, object target = null)
        {
            Target = target;
            CanRedo = canRedo;
        }
        #endregion

        #region Properties
        public object Target { get; private set; }

        /// <summary>
        /// Gets or sets the description, which is option and can be used to display a text to the end-user.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        public object Tag { get; set; }

        public bool CanRedo { get; private set; }

        public int Value { get; set; }
        #endregion

        #region IMementoSupport Members
        public void Undo()
        {
            UndoCalled = true;
        }

        public void Redo()
        {
            RedoCalled = true;
        }
        #endregion
    }
}