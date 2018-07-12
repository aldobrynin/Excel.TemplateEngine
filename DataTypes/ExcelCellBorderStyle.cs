﻿using System.Collections.Generic;

namespace SKBKontur.Catalogue.ExcelFileGenerator.DataTypes
{
    public class ExcelCellBorderStyle
    {
        public ExcelColor Color { get; set; }
        public ExcelBorderType BorderType { get; set; }

        public override string ToString()
        {
            var lines = new List<string>();
            if (Color != null)
                lines.Add(string.Format("Color = {0}", Color));
            lines.Add(string.Format("BorderType = {0}", BorderType));

            return "\n\t\t\t\t" + string.Join("\n\t\t\t\t", lines) + "\n\t\t\t";
        }
    }
}