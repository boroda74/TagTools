using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public delegate void CheckBoxClickedHandler(bool state);

    public class DataGridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        private Point checkBoxLocation;
        private Size checkBoxSize;
        private bool checkedState;
        private Point cellLocation;
        private System.Windows.Forms.VisualStyles.CheckBoxState cbState =
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;

        public event CheckBoxClickedHandler OnCheckBoxClicked;

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
            if (rowIndex != -2)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    dataGridViewElementState, value,
                    formattedValue, errorText, cellStyle,
                    advancedBorderStyle, paintParts);
            }

            var p = new Point();
            var s = CheckBoxRenderer.GetGlyphSize(graphics,
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
                OnCheckBoxClicked(checkedState); //-V3083 //-V5605
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
            var p = new Point(e.X + cellLocation.X, e.Y + cellLocation.Y);

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



    internal class DataGridViewBoundColumns
    {
        public List<object> Columns { get; set; }

        public DataGridViewBoundColumns()
        {
            Columns = new List<object>();
        }

        public DataGridViewBoundColumns(int columnCount)
        {
            Columns = new List<object>();
            for (int i = 0; i < columnCount; i++)
                Columns.Add(null);
        }

        public DataGridViewBoundColumns CreateColumns(int columnCount)
        {
            Columns = new List<object>();
            this.AddColumns(columnCount);
            return this;
        }

        public DataGridViewBoundColumns AddColumns(int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
                Columns.Add(null);

            return this;
        }

        public DataGridViewBoundColumns AddColumns(IEnumerable<object> columns)
        {
            Columns.AddRange(columns);
            return this;
        }
    }

    internal class DataGridViewBoundColumnList<T> : DataGridViewBoundList<T>
        where T : DataGridViewBoundColumns, new()
    {
        public DataGridViewBoundColumnList(List<string> columnNames) : base("Columns", columnNames)
        {
            //Nothing new...
        }

        public void Sort(Plugin.DataGridViewBoundColumnsComparer comparer)
        {
            Sort((x, y) => comparer.Compare(x, y));
        }

        internal DataGridViewBoundColumnList<T> AddColumnNames(List<string> columnNames)
        {
            this.ColumnNames.AddRange(columnNames);

            return this;
        }

        internal DataGridViewBoundColumnList<T> CreateRows(int columnCount, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
                base.Add(new T().CreateColumns(columnCount) as T);

            return this;
        }

        internal DataGridViewBoundColumnList<T> AddRows(int columnCount, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
                base.Add(new T().AddColumns(columnCount) as T);

            return this;
        }

        internal DataGridViewBoundColumnList<T> AddRow(IEnumerable<object> tags)
        {
            base.Add(new T().AddColumns(tags) as T);

            return this;
        }
    }

    internal class DataGridViewBoundList<T> : List<T>, ITypedList
    {
        public List<string> ColumnNames;
        private string listPropertyName;

        public DataGridViewBoundList(string listPropertyName, List<string> columnNames)
        {
            if (listPropertyName == null)
                throw new Exception("List property name must not be null!");
            
            //if (columnNames != null && columnNames.Count == 0)
            //    throw new Exception("Column names count must be positive number!");

            this.listPropertyName = listPropertyName;

            if (columnNames != null)
                this.ColumnNames = columnNames;
            else
                this.ColumnNames = new List<string>();
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            var origProps = TypeDescriptor.GetProperties(typeof(T));
            List<PropertyDescriptor> newProps = new List<PropertyDescriptor>();
            PropertyDescriptor listProp = null;
            Type propType = typeof(object);

            foreach (PropertyDescriptor prop in origProps)
            {
                if (prop.Name == listPropertyName)
                    listProp = prop;
                else 
                    newProps.Add(prop);
            }

            if (listProp != null)
            {
                for (int i = 0; i < ColumnNames.Count; i++)
                    newProps.Add(new ListItemDescriptor(listProp, ColumnNames[i], i, propType));
            }

            return new PropertyDescriptorCollection(newProps.ToArray());
        }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return listPropertyName;
        }
    }


    internal class ListItemDescriptor : PropertyDescriptor
    {
        private static readonly Attribute[] nix = new Attribute[0];
        private readonly PropertyDescriptor tail;
        private readonly Type type;
        private readonly int index;

        public ListItemDescriptor(PropertyDescriptor tail, string name, int index, Type type) : base(name == null ? index.ToString() : name, nix)
        {
            this.tail = tail;
            this.type = type;
            this.index = index;
        }

        public override object GetValue(object component)
        {
            IList list = tail.GetValue(component) as IList;
            return (list == null || list.Count <= index) ? null : list[index];
        }

        public override Type PropertyType
        {
            get { return type; }
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override void SetValue(object component, object value)
        {
            throw new NotSupportedException();
        }

        public override void ResetValue(object component)
        {
            throw new NotSupportedException();
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return tail.ComponentType; }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
