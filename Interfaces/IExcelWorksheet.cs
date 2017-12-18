﻿using System;
using System.Collections.Generic;

using SKBKontur.Catalogue.ExcelFileGenerator.DataTypes;
using SKBKontur.Catalogue.ExcelFileGenerator.Implementation;

namespace SKBKontur.Catalogue.ExcelFileGenerator.Interfaces
{
    public interface IExcelWorksheet
    {
        void SetPrinterSettings(ExcelPrinterSettings excelPrinterSettings);
        IExcelCell InsertCell(ExcelCellIndex cellIndex);
        IExcelRow CreateRow(int rowIndex);
        void MergeCells(ExcelCellIndex upperLeft, ExcelCellIndex lowerRight);
        void ResizeColumn(int columnIndex, double width);
        IEnumerable<IExcelCell> GetSortedCellsInRange(ExcelCellIndex upperLeft, ExcelCellIndex lowerRight);
        IExcelCell GetCell(ExcelCellIndex position);
        IExcelFormControlInfo GetFormControlInfo(string name);
        IEnumerable<IExcelCell> SearchCellsByText(string text);
        IEnumerable<IExcelRow> Rows { get; }
        IEnumerable<IExcelColumn> Columns { get; }
        IEnumerable<Tuple<ExcelCellIndex, ExcelCellIndex>> MergedCells { get; }
        IExcelFormControlInfo[] GetFormControlInfosList();
        void AddFormControlInfos(IExcelFormControlInfo[] formControlInfos);
    }
}