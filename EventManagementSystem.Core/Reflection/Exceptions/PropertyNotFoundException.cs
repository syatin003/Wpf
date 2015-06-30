using System;

namespace EventManagementSystem.Core.Reflection.Exceptions
{
    /// <summary>
    /// Exception for in case a property is not found.
    /// </summary>
    public class PropertyNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyNotFoundException"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyNotFoundException(string propertyName)
            : base(string.Format(Properties.Exceptions.PropertyNotFound, propertyName))
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