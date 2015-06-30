using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Helpers
{
    public static class ForwardBookGridViewHelper
    {
        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.RegisterAttached("BindableColumns",
                                                typeof(ObservableCollection<GridViewDataColumn>),
                                                typeof(ForwardBookGridViewHelper),
                                                new UIPropertyMetadata(null, BindableColumnsPropertyChanged));

        private static void BindableColumnsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            RadGridView dataGrid = source as RadGridView;
            ObservableCollection<GridViewDataColumn> columns = e.NewValue as ObservableCollection<GridViewDataColumn>;
            dataGrid.Columns.Clear();
            if (columns == null)
            {
                return;
            }
            foreach (GridViewDataColumn column in columns)
            {
                dataGrid.Columns.Add(column);
            }
            columns.CollectionChanged += (sender, e2) =>
                {
                    NotifyCollectionChangedEventArgs ne = e2 as NotifyCollectionChangedEventArgs;
                    if (ne.Action == NotifyCollectionChangedAction.Reset)
                    {
                        dataGrid.Columns.Clear();
                        if (ne.NewItems != null)
                        {
                            foreach (GridViewDataColumn column in ne.NewItems)
                            {
                                dataGrid.Columns.Add(column);
                            }
                        }
                    }
                    else if (ne.Action == NotifyCollectionChangedAction.Add)
                    {
                        if (ne.NewItems != null)
                        {
                            foreach (GridViewDataColumn column in ne.NewItems)
                            {
                                dataGrid.Columns.Add(column);
                            }
                        }
                    }
                    else if (ne.Action == NotifyCollectionChangedAction.Move)
                    {
                        dataGrid.Columns.Move(ne.OldStartingIndex, ne.NewStartingIndex);
                    }
                    else if (ne.Action == NotifyCollectionChangedAction.Remove)
                    {
                        if (ne.OldItems != null)
                        {
                            foreach (GridViewDataColumn column in ne.OldItems)
                            {
                                dataGrid.Columns.Remove(column);
                            }
                        }
                    }
                    else if (ne.Action == NotifyCollectionChangedAction.Replace)
                    {
                        dataGrid.Columns[ne.NewStartingIndex] = ne.NewItems[0] as GridViewDataColumn;
                    }
                };
        }

        public static void SetBindableColumns(DependencyObject element, ObservableCollection<DataGridColumn> value)
        {
            element.SetValue(BindableColumnsProperty, value);
        }

        public static ObservableCollection<DataGridColumn> GetBindableColumns(DependencyObject element)
        {
            return (ObservableCollection<DataGridColumn>)element.GetValue(BindableColumnsProperty);
        }
    }
}