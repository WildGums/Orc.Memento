// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObserverBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Memento
{
    using Catel.IoC;

    /// <summary>
    /// Base class for all observer classes.
    /// </summary>
    public abstract class ObserverBase
    {
        #region Fields
        /// <summary>
        /// The memento service.
        /// </summary>
        private readonly IMementoService _mementoService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ObserverBase"/> class.
        /// </summary>
        /// <param name="tag">The tag, can be <c>null</c>.</param>
        /// <param name="mementoService">The memento service. If <c>null</c>, the service will be retrieved from the <see cref="IServiceLocator"/>.</param>
        protected ObserverBase(object tag, IMementoService mementoService = null)
        {
            Tag = tag;

            if (mementoService is null)
            {
                var dependencyResolver = IoCConfiguration.DefaultDependencyResolver;
                mementoService = dependencyResolver.Resolve<IMementoService>();
            }

            _mementoService = mementoService;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public object Tag { get; set; }

        /// <summary>
        /// Gets the memento service.
        /// </summary>
        protected IMementoService MementoService
        {
            get { return _mementoService; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Clears all the values and unsubscribes any existing change notifications.
        /// </summary>
        public abstract void CancelSubscription();
        #endregion
    }
}