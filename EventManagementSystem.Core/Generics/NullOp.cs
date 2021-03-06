﻿namespace EventManagementSystem.Core.Generics
{
    /// <remarks>
    /// Code originally found at http://www.yoda.arachsys.com/csharp/miscutil/.
    /// </remarks>
    internal interface INullOp<T>
    {
        bool HasValue(T value);
        bool AddIfNotNull(ref T accumulator, T value);
    }

    /// <remarks>
    /// Code originally found at http://www.yoda.arachsys.com/csharp/miscutil/.
    /// </remarks>
    internal sealed class StructNullOp<T> : INullOp<T>, INullOp<T?>
        where T : struct
    {
        #region INullOp<T?> Members

        public bool HasValue(T? value)
        {
            return value.HasValue;
        }

        public bool AddIfNotNull(ref T? accumulator, T? value)
        {
            if (value.HasValue)
            {
                accumulator = accumulator.HasValue
                                  ? Operator<T>.Add(accumulator.GetValueOrDefault(), value.GetValueOrDefault())
                                  : value;
                return true;
            }

            return false;
        }

        #endregion

        #region INullOp<T> Members

        public bool HasValue(T value)
        {
            return true;
        }

        public bool AddIfNotNull(ref T accumulator, T value)
        {
            accumulator = Operator<T>.Add(accumulator, value);
            return true;
        }

        #endregion
    }

    /// <remarks>
    /// Code originally found at http://www.yoda.arachsys.com/csharp/miscutil/.
    /// </remarks>
    internal sealed class ClassNullOp<T> : INullOp<T>
        where T : class
    {
        #region INullOp<T> Members

        public bool HasValue(T value)
        {
            return value != null;
        }

        public bool AddIfNotNull(ref T accumulator, T value)
        {
            if (value != null)
            {
                accumulator = accumulator == null ? value : Operator<T>.Add(accumulator, value);
                return true;
            }
            return false;
        }

        #endregion
    }
}