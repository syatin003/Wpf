using System.Windows.Controls;
using System.Windows.Media;

namespace EventManagementSystem.CommonObjects
{
    public class Tile
    {
        #region Properties

        public string Name { get; set; }
        public ContentControl Content { get; set; }
        public ImageSource Image { get; set; }

        #endregion
    }
}
