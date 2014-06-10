﻿namespace SKBKontur.Catalogue.ExcelFileGenerator
{
    internal static class IndexHelpers
    {
        public static string ToCellName(int rowIndex, int columnIndex)
        {
            return string.Format("{0}{1}", ToColumnName(columnIndex), rowIndex);
        }

        public static string ToColumnName(int columnIndex)
        {
            columnIndex -= 1;
            var prefixIndex = columnIndex / 26;
            var tail = ((char)(columnIndex % 26 + 'A')).ToString();
            return prefixIndex > 0 ? ToColumnName(prefixIndex) + tail : tail;
        }
    }
}