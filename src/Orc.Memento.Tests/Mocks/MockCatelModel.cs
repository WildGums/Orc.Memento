namespace Orc.Memento.Tests.Mocks
{
    using Catel.Data;

    public class MockCatelModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string Value
        {
            get { return GetValue<string>(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Register the Value property so it is known in the class.
        /// </summary>
        public static readonly IPropertyData ValueProperty = RegisterProperty<string>("Value", "defaultValue");
    }
}
