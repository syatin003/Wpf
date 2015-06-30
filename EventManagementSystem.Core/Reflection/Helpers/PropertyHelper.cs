using System;
using System.Collections.Generic;
using System.Reflection;
using EventManagementSystem.Core.Reflection.Exceptions;

namespace EventManagementSystem.Core.Reflection.Helpers
{
    /// <summary>
    /// Property helper class.
    /// </summary>
    public static class PropertyHelper
    {
        #region Constants

        private const BindingFlags PropertyBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        #endregion

        #region Variables

        /// <summary>
        /// Dictionary that serves as a cache for all properties.
        /// </summary>
        private static readonly Dictionary<string, PropertyInfo> _reflectionCache = new Dictionary<string, PropertyInfo>();
        #endregion

        /// <summary>
        /// Determines whether the specified property is available on the object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="property">The property.</param>
        /// <returns>
        /// 	<c>true</c> if the property exists on the object type; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="obj"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="property"/> is <c>null</c> or whitespace.</exception>
        public static bool IsPropertyAvailable(object obj, string property)
        {
            Argument.IsNotNull("obj", obj);
            Argument.IsNotNullOrWhitespace("property", property);

            return GetCachedPropertyInfo(obj, property) != null;
        }

        /// <summary>
        /// Tries to get the property value. If it fails, not exceptions will be thrown but the <paramref name="value"/> 
        /// is set to a default value and the method will return <c>false</c>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value as output parameter.</param>
        /// <returns><c>true</c> if the method succeeds; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="obj"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="property"/> is <c>null</c> or whitespace.</exception>
        public static bool TryGetPropertyValue(object obj, string property, out object value)
        {
            Argument.IsNotNull("obj", obj);
            Argument.IsNotNullOrWhitespace("property", property);

            try
            {
                value = GetPropertyValue(obj, property);
                return true;
            }
            catch (Exception)
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to get the property value. If it fails, not exceptions will be thrown but the <paramref name="value"/> 
        /// is set to a default value and the method will return <c>false</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value as output parameter.</param>
        /// <returns>
        /// 	<c>true</c> if the method succeeds; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="obj"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="property"/> is <c>null</c> or whitespace.</exception>
        public static bool TryGetPropertyValue<TValue>(object obj, string property, out TValue value)
        {
            Argument.IsNotNull("obj", obj);
            Argument.IsNotNullOrWhitespace("property", property);

            try
            {
                value = GetPropertyValue<TValue>(obj, property);
                return true;
            }
            catch (Exception)
            {
                value = default(TValue);
                return false;
            }
        }

        /// <summary>
        /// Gets the property value of a specific object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The property value or <c>null</c> if no property can be found.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="obj"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="property"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="PropertyNotFoundException">The <paramref name="obj"/> is not found or not publicly available.</exception>
        /// <exception cref="CannotGetPropertyValueException">The property value cannot be read.</exception>
        public static object GetPropertyValue(object obj, string property)
        {
            Argument.IsNotNull("obj", obj);
            Argument.IsNotNullOrWhitespace("property", property);

            var propertyInfo = GetCachedPropertyInfo(obj, property);
            if (propertyInfo == null)
            {
                throw new PropertyNotFoundException(property);
            }

            // Return property value if available
            if (!propertyInfo.CanRead)
            {
                throw new CannotGetPropertyValueException(property);
            }

            try
            {
                return propertyInfo.GetValue(obj, null);
            }
            catch (MethodAccessException)
            {
                throw new CannotGetPropertyValueException(property);
            }
        }

        /// <summary>
        /// Gets the property value of a specific object.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <returns>
        /// The property value or <c>null</c> if no property can be found.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="obj"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="property"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="PropertyNotFoundException">The <paramref name="obj"/> is not found or not publicly available.</exception>
        /// <exception cref="CannotGetPropertyValueException">The property value cannot be read.</exception>
        public static TValue GetPropertyValue<TValue>(object obj, string property)
        {
            return (TValue)GetPropertyValue(obj, property);
        }

        /// <summary>
        /// Tries to set the property value. If it fails, no exceptions will be thrown, but <c>false</c> will
        /// be returned.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the method succeeds; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="obj"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="property"/> is <c>null</c> or whitespace.</exception>
        public static bool TrySetPropertyValue(object obj, string property, object value)
        {
            Argument.IsNotNull("obj", obj);
            Argument.IsNotNullOrWhitespace("property", property);

            try
            {
                SetPropertyValue(obj, property, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the property value of a specific object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="obj"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="property"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="PropertyNotFoundException">The <paramref name="obj"/> is not found or not publicly available.</exception>
        /// <exception cref="CannotSetPropertyValueException">The the property value cannot be written.</exception>
        public static void SetPropertyValue(object obj, string property, object value)
        {
            Argument.IsNotNull("obj", obj);
            Argument.IsNotNullOrWhitespace("property", property);

            var propertyInfo = GetCachedPropertyInfo(obj, property);
            if (propertyInfo == null)
            {
                throw new PropertyNotFoundException(property);
            }

            if (!propertyInfo.CanWrite)
            {
                throw new CannotSetPropertyValueException(property);
            }

            try
            {
                propertyInfo.SetValue(obj, value, null);
            }
            catch (MethodAccessException)
            {
                throw new CannotSetPropertyValueException(property);
            }
        }

        /// <summary>
        /// Gets the cached property info. If the property is already cached, the result from the cache
        /// will be returned. Otherwise the property info will be added to the cache and then returned.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <returns>The <see cref="PropertyInfo"/> or <c>null</c> if the property does not exist.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="obj"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="property"/> is <c>null</c> or whitespace.</exception>
        private static PropertyInfo GetCachedPropertyInfo(object obj, string property)
        {
            Argument.IsNotNull("obj", obj);
            Argument.IsNotNullOrWhitespace("property", property);

            string key = string.Format("{0}|{1}", obj.GetType().AssemblyQualifiedName, property);

            lock (_reflectionCache)
            {
                if (!_reflectionCache.ContainsKey(key))
                {
                    var propertyInfo = obj.GetType().GetProperty(property, PropertyBindingFlags);
                    _reflectionCache.Add(key, propertyInfo);
                }

                return _reflectionCache[key];
            }
        }
    }
}
