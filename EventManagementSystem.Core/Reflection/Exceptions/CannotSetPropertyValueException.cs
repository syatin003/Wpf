using System;

namespace EventManagementSystem.Core.Reflection.Exceptions
{
    /// <summary>
    /// Exception in case a property value cannot be set.
    /// </summary>
    public class CannotSetPropertyValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CannotSetPropertyValueException"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public CannotSetPropertyValueException(string propertyName)
            : base(string.Format(Properties.Exceptions.CannotSetPropertyValueException, propertyName))
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