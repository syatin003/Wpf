using System;
using System.Diagnostics;
using System.Linq;
using EventManagementSystem.Core.Generics;
using EventManagementSystem.Core.Properties;

namespace EventManagementSystem.Core
{
    /// <summary>
    /// Argument validator class to help validating arguments that are passed into a method.
    /// </summary>
    public static class Argument
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified argument is not <c>null</c>.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="paramValue"/> is <c>null</c>.</exception>
        [DebuggerStepThrough]
        public static void IsNotNull(string paramName, object paramValue)
        {
            EnsureValidParamName(paramName);

            if (paramValue == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Determines whether the specified argument is not <c>null</c> or empty.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <exception cref="ArgumentException">If <paramref name="paramValue"/> is <c>null</c> or empty.</exception>
        [DebuggerStepThrough]
        public static void IsNotNullOrEmpty(string paramName, string paramValue)
        {
            EnsureValidParamName(paramName);

            if (string.IsNullOrEmpty(paramValue))
            {
                string error = string.Format(Exceptions.ArgumentNullOrEmpty, paramName);
                
                throw new ArgumentNullException(error, paramName);
            }
        }

        /// <summary>
        /// Determines whether the specified argument is not <c>null</c> or a whitespace.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <exception cref="ArgumentException">If <paramref name="paramValue"/> is <c>null</c> or a whitespace.</exception>
        [DebuggerStepThrough]
        public static void IsNotNullOrWhitespace(string paramName, string paramValue)
        {
            EnsureValidParamName(paramName);

            if (string.IsNullOrEmpty(paramValue) || (String.CompareOrdinal(paramValue.Trim(), string.Empty) == 0))
            {
                string error = string.Format(Exceptions.ArgumentNullOrWhitespace, paramName);

                throw new ArgumentException(error, paramName);
            }
        }

        /// <summary>
        /// Determines whether the specified argument is not <c>null</c> or an empty array (.Length == 0).
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <exception cref="ArgumentException">If <paramref name="paramValue"/> is <c>null</c> or an empty array.</exception>
        [DebuggerStepThrough]
        public static void IsNotNullOrEmptyArray(string paramName, Array paramValue)
        {
            EnsureValidParamName(paramName);

            if ((paramValue == null) || (paramValue.Length == 0))
            {
                string error = string.Format(Exceptions.ArgumentNullOrEmptyArray, paramName);

                throw new ArgumentException(error, paramName);
            }
        }

        /// <summary>
        /// Determines whether the specified argument is not out of range.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <param name="validation">The validation function to call for validation.</param>
        /// <param name="errorMessage">Custom Error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="paramValue"/> is out of range.</exception>
        [DebuggerStepThrough]
        public static void IsNotOutOfRange(string paramName, object paramValue, Func<object, bool> validation,
                                           string errorMessage = "")
        {
            EnsureValidParamName(paramName);

            IsNotNull("validation", validation);

            if (!validation(paramValue))
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    throw new ArgumentOutOfRangeException(paramName, errorMessage);
                }

                throw new ArgumentOutOfRangeException(paramName);
            }
        }

        /// <summary>
        /// Determines whether the specified argument is not out of range.
        /// </summary>
        /// <typeparam name="T">Type of the argument.</typeparam>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <param name="minimumValue">The minimum value.</param>
        /// <param name="maximumValue">The maximum value.</param>
        /// <param name="errorMessage">Custom Error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="paramValue"/> is out of range.</exception>
        [DebuggerStepThrough]
        public static void IsNotOutOfRange<T>(string paramName, T paramValue, T minimumValue, T maximumValue,
                                              string errorMessage = "")
        {
            EnsureValidParamName(paramName);

            if (Operator<T>.LessThan(paramValue, minimumValue) || Operator<T>.GreaterThan(paramValue, maximumValue))
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = string.Format(Exceptions.ArgumentNotInRange, paramName, minimumValue,
                                                 maximumValue);
                }

                throw new ArgumentOutOfRangeException(paramName, errorMessage);
            }
        }

        /// <summary>
        /// Determines whether the specified argument has a minimum value.
        /// </summary>
        /// <typeparam name="T">Type of the argument.</typeparam>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <param name="minimumValue">The minimum value.</param>
        /// <param name="errorMessage">Custom Error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="paramValue"/> is out of range.</exception>
        [DebuggerStepThrough]
        public static void IsNotLessThan<T>(string paramName, T paramValue, T minimumValue, string errorMessage = "")
        {
            EnsureValidParamName(paramName);

            if (Operator<T>.LessThan(paramValue, minimumValue))
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = string.Format(Exceptions.ArgumentLessThanValue, paramName, minimumValue);
                }

                throw new ArgumentOutOfRangeException(paramName, errorMessage);
            }
        }

        /// <summary>
        /// Determines whether the specified argument has a maximum value.
        /// </summary>
        /// <typeparam name="T">Type of the argument.</typeparam>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <param name="maximumValue">The maximum value.</param>
        /// <param name="errorMessage">Custom Error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="paramValue"/> is out of range.</exception>
        [DebuggerStepThrough]
        public static void IsNotGreaterThan<T>(string paramName, T paramValue, T maximumValue, string errorMessage = "")
        {
            EnsureValidParamName(paramName);

            if (Operator<T>.GreaterThan(paramValue, maximumValue))
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = string.Format(Exceptions.ArgumentGreaterThanValue, paramName, maximumValue);
                }

                throw new ArgumentOutOfRangeException(paramName, errorMessage);
            }
        }

        /// <summary>
        /// Checks whether the specified <paramref name="instance"/> implements the specified <paramref name="interfaceType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="instance">The instance to check.</param>
        /// <param name="interfaceType">The type of the interface to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="instance"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="interfaceType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="instance"/> does not implement the <paramref name="interfaceType"/>.</exception>
        [DebuggerStepThrough]
        public static void ImplementsInterface(string paramName, object instance, Type interfaceType)
        {
            IsNotNull("instance", instance);

            ImplementsInterface(paramName, instance.GetType(), interfaceType);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="type"/> implements the specified <paramref name="interfaceType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="type">The type to check.</param>
        /// <param name="interfaceType">The type of the interface to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="interfaceType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="type"/> does not implement the <paramref name="interfaceType"/>.</exception>
        [DebuggerStepThrough]
        public static void ImplementsInterface(string paramName, Type type, Type interfaceType)
        {
            EnsureValidParamName(paramName);
            IsNotNull("type", type);
            IsNotNull("interfaceType", interfaceType);

            if (type.GetInterfaces().Any(iType => iType == interfaceType))
            {
                return;
            }

            string error = string.Format(Exceptions.ArgumentDoesNotImplementInterface, type.Name,
                                         interfaceType.Name);

            throw new ArgumentException(error, "type");
        }

        /// <summary>
        /// Checks whether the specified <paramref name="instance"/> is of the specified <paramref name="requiredType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="instance">The instance to check.</param>
        /// <param name="requiredType">The type to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="instance"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="requiredType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="instance"/> is not of type <paramref name="requiredType"/>.</exception>
        [DebuggerStepThrough]
        public static void IsOfType(string paramName, object instance, Type requiredType)
        {
            IsNotNull("instance", instance);

            IsOfType(paramName, instance.GetType(), requiredType);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="type"/> is of the specified <paramref name="requiredType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="type">The type to check.</param>
        /// <param name="requiredType">The type to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="requiredType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="type"/> is not of type <paramref name="requiredType"/>.</exception>
        [DebuggerStepThrough]
        public static void IsOfType(string paramName, Type type, Type requiredType)
        {
            EnsureValidParamName(paramName);
            IsNotNull("type", type);
            IsNotNull("requiredType", requiredType);

            if (requiredType.IsAssignableFrom(type))
            {
                return;
            }

            string error = string.Format(Exceptions.ArgumentIsNotOfType, type.Name, requiredType.Name);

            throw new ArgumentException(error, "type");
        }

        /// <summary>
        /// Checks whether the specified <paramref name="instance"/> is not of the specified <paramref name="notRequiredType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="instance">The instance to check.</param>
        /// <param name="notRequiredType">The type to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="instance"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="notRequiredType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="instance"/> is of type <paramref name="notRequiredType"/>.</exception>
        [DebuggerStepThrough]
        public static void IsNotOfType(string paramName, object instance, Type notRequiredType)
        {
            IsNotNull("instance", instance);

            IsNotOfType(paramName, instance.GetType(), notRequiredType);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="type"/> is not of the specified <paramref name="notRequiredType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="type">The type to check.</param>
        /// <param name="notRequiredType">The type to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="notRequiredType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="type"/> is of type <paramref name="notRequiredType"/>.</exception>
        [DebuggerStepThrough]
        public static void IsNotOfType(string paramName, Type type, Type notRequiredType)
        {
            EnsureValidParamName(paramName);
            IsNotNull("type", type);
            IsNotNull("notRequiredType", notRequiredType);

            if (!notRequiredType.IsAssignableFrom(type))
            {
                return;
            }

            string error = string.Format(Exceptions.ArgumentWrongType, type.Name,
                                         notRequiredType.Name);

            throw new ArgumentException(error, "type");
        }

        /// <summary>
        /// Checks whether the passed in boolean check is <c>true</c>. If not, this method will throw a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="isSupported">if set to <c>true</c>, the action is supported; otherwise <c>false</c>.</param>
        /// <param name="errorFormat">The error format.</param>
        /// <param name="args">The arguments for the string format.</param>
        /// <exception cref="NotSupportedException">The <paramref name="isSupported"/> is <c>false</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="errorFormat"/> is <c>null</c> or whitespace.</exception>
        public static void IsSupported(bool isSupported, string errorFormat, params object[] args)
        {
            IsNotNullOrWhitespace("errorFormat", errorFormat);

            if (!isSupported)
            {
                string error = string.Format(errorFormat, args);

                throw new NotSupportedException(error);
            }
        }

        /// <summary>
        /// Ensures that the name of the param is valid.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="ArgumentException">If <paramref name="paramName"/> is <c>null</c> or whitespace.</exception>
        [DebuggerStepThrough]
        private static void EnsureValidParamName(string paramName)
        {
            if ((paramName == null) || string.IsNullOrEmpty(paramName))
            {
                string error = string.Format(Exceptions.ArgumentNullOrWhitespace, "paramName");

                throw new ArgumentException(error, "paramName");
            }
        }

        #endregion
    }
}