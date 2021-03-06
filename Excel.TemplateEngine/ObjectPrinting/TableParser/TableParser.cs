using System;
using System.Globalization;

using JetBrains.Annotations;

using SkbKontur.Excel.TemplateEngine.ObjectPrinting.ExcelDocumentPrimitives;
using SkbKontur.Excel.TemplateEngine.ObjectPrinting.NavigationPrimitives;
using SkbKontur.Excel.TemplateEngine.ObjectPrinting.TableNavigator;

namespace SkbKontur.Excel.TemplateEngine.ObjectPrinting.TableParser
{
    public class TableParser : ITableParser
    {
        public TableParser([NotNull] ITable target, [NotNull] ITableNavigator navigator)
        {
            this.target = target;
            this.navigator = navigator;
        }

        public bool TryParseAtomicValue(out string result)
        {
            result = target.GetCell(CurrentState.Cursor)?.StringValue;
            return true;
        }

        public bool TryParseAtomicValue(out int result)
        {
            var cellValue = target.GetCell(CurrentState.Cursor)?.StringValue;
            return int.TryParse(cellValue, out result);
        }

        public bool TryParseAtomicValue(out double result)
        {
            var cellValue = target.GetCell(CurrentState.Cursor)?.StringValue;
            return double.TryParse(cellValue, out result);
        }

        public bool TryParseAtomicValue(out decimal result)
        {
            var cellValue = target.GetCell(CurrentState.Cursor)?.StringValue;
            return decimal.TryParse(cellValue, numberStyles, russianCultureInfo, out result) || decimal.TryParse(cellValue, numberStyles, CultureInfo.InvariantCulture, out result);
        }

        public bool TryParseAtomicValue(out long result)
        {
            var cellValue = target.GetCell(CurrentState.Cursor)?.StringValue;
            return long.TryParse(cellValue, out result);
        }

        public bool TryParseAtomicValue(out int? result)
        {
            return TryParseNullableAtomicValue(() => (TryParseAtomicValue(out int res), res), out result);
        }

        public bool TryParseAtomicValue(out double? result)
        {
            return TryParseNullableAtomicValue(() => (TryParseAtomicValue(out double res), res), out result);
        }

        public bool TryParseAtomicValue(out decimal? result)
        {
            return TryParseNullableAtomicValue(() => (TryParseAtomicValue(out decimal res), res), out result);
        }

        public bool TryParseAtomicValue(out long? result)
        {
            return TryParseNullableAtomicValue(() => (TryParseAtomicValue(out long res), res), out result);
        }

        private bool TryParseNullableAtomicValue<T>(Func<(bool succeed, T result)> parser, out T? result)
            where T : struct
        {
            var cellValue = target.GetCell(CurrentState.Cursor)?.StringValue;
            if (string.IsNullOrEmpty(cellValue))
            {
                result = null;
                return true;
            }
            bool succeed;
            (succeed, result) = parser();
            return succeed;
        }

        public bool TryParseCheckBoxValue([NotNull] string name, out bool result)
        {
            var formControl = target.TryGetCheckBoxFormControl(name);
            if (formControl == null)
            {
                result = false;
                return false;
            }
            result = formControl.IsChecked;
            return true;
        }

        public bool TryParseDropDownValue([NotNull] string name, [CanBeNull] out string result)
        {
            var formControl = target.TryGetDropDownFormControl(name);
            if (formControl == null)
            {
                result = null;
                return false;
            }
            result = formControl.SelectedValue;
            return true;
        }

        public ITableParser PushState(ICellPosition newOrigin)
        {
            navigator.PushState(newOrigin);
            return this;
        }

        public ITableParser PushState()
        {
            navigator.PushState();
            return this;
        }

        public ITableParser PopState()
        {
            navigator.PopState();
            return this;
        }

        public ITableParser MoveToNextLayer()
        {
            navigator.MoveToNextLayer();
            return this;
        }

        public ITableParser MoveToNextColumn()
        {
            navigator.MoveToNextColumn();
            return this;
        }

        public TableNavigatorState CurrentState => navigator.CurrentState;

        private const NumberStyles numberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;

        private readonly ITable target;
        private readonly ITableNavigator navigator;
        private static readonly CultureInfo russianCultureInfo = CultureInfo.GetCultureInfo("ru-RU");
    }
}