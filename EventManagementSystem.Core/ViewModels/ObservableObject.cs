using System;
using System.ComponentModel;
using System.Linq.Expressions;
using EventManagementSystem.Core.EventArgs;
using EventManagementSystem.Core.Extensions;
using EventManagementSystem.Core.Interfaces;
using EventManagementSystem.Core.Linq;
using EventManagementSystem.Core.Properties;

namespace EventManagementSystem.Core.ViewModels
{
    /// <summary>
    /// Very basic class implementing the <see cref="INotifyPropertyChanging"/> and <see cref="INotifyPropertyChanged"/> interfaces.
    /// </summary>
    [Serializable]
    public class ObservableObject : INotifyPropertyChanged, IAdvancedNotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property of this object is changing.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Occurs when a property of this object has changed.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #region Methods

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event.
        /// </summary>
        /// <typeparam name="TProperty">The type of the object holding the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="propertyExpression"/> is <c>null</c>.</exception>
        /// <example>
        /// <![CDATA[
        ///     RaisePropertyChanging(() => IsDirty);
        /// ]]>
        /// </example>
        protected void RaisePropertyChanging<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            Argument.IsNotNull("propertyExpression", propertyExpression);

            object sender = ExpressionUtil.GetOwner(propertyExpression) ?? this;
            string propertyName = ExpressionUtil.GetPropertyName(propertyExpression);

            RaisePropertyChanging(sender, propertyName);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected void RaisePropertyChanging(string propertyName)
        {
            RaisePropertyChanging(this, propertyName);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanging(object sender, string propertyName)
        {
            RaisePropertyChanging(sender, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event.
        /// <para />
        /// This is the one and only method that actually raises the <see cref="PropertyChanging"/> event. All other
        /// methods are (and should be) just overloads that eventually call this method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangingEventArgs"/> instance containing the event data.</param>
        protected virtual void RaisePropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            var handler = PropertyChanging;
            if (handler != null)
            {
                handler(sender, e);
            }

            if (ReferenceEquals(this, sender))
            {
                OnPropertyChanging(e);
            }
        }

        /// <summary>
        /// Called when the <see cref="PropertyChanging"/> event occurs.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangingEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="TProperty">The type of the object holding the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="propertyExpression"/> is <c>null</c>.</exception>
        /// <example>
        /// <![CDATA[
        ///     RaisePropertyChanged(() => IsDirty);
        /// ]]>
        /// </example>
        protected void RaisePropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            Argument.IsNotNull("propertyExpression", propertyExpression);

            object sender = ExpressionUtil.GetOwner(propertyExpression) ?? this;
            string propertyName = ExpressionUtil.GetPropertyName(propertyExpression);

            RaisePropertyChanged(sender, propertyName);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="TProperty">The type of the object holding the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="newValue">The new value.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="propertyExpression"/> is <c>null</c>.</exception>
        /// <example>
        /// 	<![CDATA[
        /// RaisePropertyChanged(() => IsDirty, true);
        /// ]]>
        /// </example>
        protected void RaisePropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpression, object newValue)
        {
            Argument.IsNotNull("propertyExpression", propertyExpression);

            object sender = ExpressionUtil.GetOwner(propertyExpression) ?? this;
            string propertyName = ExpressionUtil.GetPropertyName(propertyExpression);

            RaisePropertyChanged(sender, propertyName, newValue);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="TProperty">The type of the object holding the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="propertyExpression"/> is <c>null</c>.</exception>
        /// <example>
        /// 	<![CDATA[
        /// RaisePropertyChanged(() => IsDirty, false, true);
        /// ]]>
        /// </example>
        protected void RaisePropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpression, object oldValue, object newValue)
        {
            Argument.IsNotNull("propertyExpression", propertyExpression);

            object sender = ExpressionUtil.GetOwner(propertyExpression) ?? this;
            string propertyName = ExpressionUtil.GetPropertyName(propertyExpression);

            RaisePropertyChanged(sender, propertyName, oldValue, newValue);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            RaisePropertyChanged(this, propertyName);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="newValue">The new value.</param>
        protected void RaisePropertyChanged(string propertyName, object newValue)
        {
            RaisePropertyChanged(this, propertyName, newValue);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected void RaisePropertyChanged(string propertyName, object oldValue, object newValue)
        {
            RaisePropertyChanged(this, propertyName, oldValue, newValue);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(object sender, string propertyName)
        {
            // This is 1 of the 3 places where the AdvancedPropertyChangedEventArgs are created
            var eventArgs = new AdvancedPropertyChangedEventArgs(sender, propertyName);

            RaisePropertyChanged(sender, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="newValue">The new value.</param>
        protected void RaisePropertyChanged(object sender, string propertyName, object newValue)
        {
            // This is 1 of the 3 places where the AdvancedPropertyChangedEventArgs are created
            var eventArgs = new AdvancedPropertyChangedEventArgs(sender, propertyName, newValue);

            RaisePropertyChanged(sender, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected void RaisePropertyChanged(object sender, string propertyName, object oldValue, object newValue)
        {
            // This is 1 of the 3 places where the AdvancedPropertyChangedEventArgs are created
            var eventArgs = new AdvancedPropertyChangedEventArgs(sender, propertyName, oldValue, newValue);

            RaisePropertyChanged(sender, eventArgs);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// <para />
        /// This is the one and only method that actually raises the <see cref="PropertyChanged"/> event. All other
        /// methods are (and should be) just overloads that eventually call this method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void RaisePropertyChanged(object sender, AdvancedPropertyChangedEventArgs e)
        {
            PropertyChanged.SafeInvoke(sender, e);

            if (ReferenceEquals(this, sender))
            {
                OnPropertyChanged(e);
            }
        }

        /// <summary>
        /// Called when the <see cref="PropertyChanged"/> event occurs.
        /// </summary>
        /// <param name="e">The <see cref="AdvancedPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
        }

        #endregion

    }
}
