﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using EventManagementSystem.Services;

namespace EventManagementSystem.Converters
{
    public class EditPermissionToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = false;
            Guid g;
            var CanEditEveryonePermission = (Boolean)values[0];
            var CanEditOwnPermission = (Boolean)values[1];
            var loggedUserID = AccessService.Current.User.ID;

            if (values[2] is Guid)
            {
                var ItemOwner = (Guid)values[2];
                if (ItemOwner == loggedUserID)
                {
                    if (CanEditOwnPermission)
                    {
                        flag = true;
                    }
                }
                else
                {
                    flag = CanEditEveryonePermission;
                }
            }

            if (flag)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            //var back = ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
            //if (parameter != null)
            //{
            //    if ((bool)parameter)
            //    {
            //        back = !back;
            //    }
            //}
            return new object[1];
        }

    }
}
