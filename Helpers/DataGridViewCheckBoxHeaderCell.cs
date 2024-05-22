using System.Drawing;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public delegate void CheckBoxClickedHandler(bool state);

    public class DataGridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        private Point checkBoxLocation;
        private Size checkBoxSize;
        private bool checkedState = false;
        private Point cellLocation = new Point();
        private System.Windows.Forms.VisualStyles.CheckBoxState cbState =
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;

        public event CheckBoxClickedHandler OnCheckBoxClicked;

        public DataGridViewCheckBoxHeaderCell()
        {
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                dataGridViewElementState, value,
                formattedValue, errorText, cellStyle,
                advancedBorderStyle, paintParts);

            Point p = new Point();
            Size s = CheckBoxRenderer.GetGlyphSize(graphics,
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);

            p.X = cellBounds.Location.X + (cellBounds.Width / 2) - (s.Width / 2) - 1;
            p.Y = cellBounds.Location.Y + (cellBounds.Height / 2) - (s.Height / 2) - 1;

            cellLocation = cellBounds.Location;
            checkBoxLocation = p;
            checkBoxSize = s;

            if (checkedState)
                cbState = System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;
            else
                cbState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;

            CheckBoxRenderer.DrawCheckBox(graphics, checkBoxLocation, cbState);
        }

        public void changeState()
        {
            checkedState = !checkedState;
            if (OnCheckBoxClicked != null)
            {
                OnCheckBoxClicked(checkedState); //-V3083
                DataGridView.InvalidateCell(this);
            }
        }

        public void setState(bool state)
        {
            checkedState = state;
            if (OnCheckBoxClicked != null)
            {
                OnCheckBoxClicked(checkedState);
                DataGridView.InvalidateCell(this);
            }
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + cellLocation.X, e.Y + cellLocation.Y);

            if (p.X >= checkBoxLocation.X && p.X <=
                checkBoxLocation.X + checkBoxSize.Width
            && p.Y >= checkBoxLocation.Y && p.Y <=
                checkBoxLocation.Y + checkBoxSize.Height)
            {
                changeState();
            }

            base.OnMouseClick(e);
        }
    }
}
