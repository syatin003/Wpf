using Telerik.Windows.Controls;

namespace EventManagementSystem.Helpers
{
    public class DisabledScheduleViewDragDropBehavior : ScheduleViewDragDropBehavior
    {
        public override bool CanStartDrag(DragDropState state)
        {
            return false;
        }

        public override bool CanStartResize(DragDropState state)
        {
            return false;
        }
    }
}
