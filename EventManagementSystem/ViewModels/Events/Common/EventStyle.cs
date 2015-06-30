using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Models;

namespace EventManagementSystem.ViewModels.Events.Common
{
    public class EventStyle : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is EventModel)
            {
                var model = item as EventModel;

                if (model.Changes == "Mod")
                    return ModEventStype;
                
                if (model.Changes == "New")
                    return NewEventStype;
            }

            return null;
        }

        public Style NewEventStype { get; set; }
        public Style ModEventStype { get; set; }
    }
}
