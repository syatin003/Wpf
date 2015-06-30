using System.ComponentModel;

namespace EventManagementSystem.Core.Interfaces
{
    /// <summary>
    /// Interface defining advanced additional functionality for the <see cref="INotifyPropertyChanged"/> interface. This
    /// interface still supports the "old" way, so this can perfectly be used by any other class.
    /// </summary>
    public interface IAdvancedNotifyPropertyChanged : INotifyPropertyChanged
    {
    }
}