using System;

namespace EventManagementSystem.Core.Reflection.Exceptions
{
    /// <summary>
    /// Exception in case a property value cannot be get.
    /// </summary>
    public class CannotGetPropertyValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CannotGetPropertyValueException"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public CannotGetPropertyValueException(string propertyName)
            : base(string.Format(Properties.Exceptions.CannotGetPropertyValueException, propertyName))
        {
            PropertyName = propertyName;
        }

        #region Properties
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; private set; }
        #endregion
    }
}