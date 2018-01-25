﻿using System;
using System.Collections.Generic;
using System.Linq;

using SKBKontur.Catalogue.ExcelFileGenerator.Implementation;
using SKBKontur.Catalogue.ExcelFileGenerator.Interfaces;
using SKBKontur.Catalogue.ExcelObjectPrinter.DocumentPrimitivesInterfaces;
using SKBKontur.Catalogue.ExcelObjectPrinter.Helpers;
using SKBKontur.Catalogue.ExcelObjectPrinter.NavigationPrimitives;

namespace SKBKontur.Catalogue.ExcelObjectPrinter.ExcelDocumentPrimitivesImplementation
{
    public class ExcelTable : ITable
    {
        public ExcelTable(IExcelWorksheet excelWotksheet)
        {
            internalTable = excelWotksheet;
        }

        public ICell GetCell(ICellPosition position)
        {
            var internalCell = internalTable.GetCell(new ExcelCellIndex(position.CellReference));
            return internalCell == null ? null : new ExcelCell(internalCell);
        }

        public ICell InsertCell(ICellPosition position)
        {
            return new ExcelCell(internalTable.InsertCell(new ExcelCellIndex(position.CellReference)));
        }

        public IEnumerable<ICell> SearchCellByText(string text)
        {
            return internalTable.SearchCellsByText(text).Select(cell => new ExcelCell(cell));
        }

        public ITablePart GetTablePart(IRectangle rectangle)
        {
            var excelReferenceToCell = internalTable
                .GetSortedCellsInRange(new ExcelCellIndex(rectangle.UpperLeft.CellReference),
                                       new ExcelCellIndex(rectangle.LowerRight.CellReference))
                .Select(cell => new ExcelCell(cell))
                .ToDictionary(cell => cell.CellPosition.CellReference);

            var subTableSize = rectangle.Size;
            var subTable = JaggedArrayHelper.Instance.CreateJaggedArray<ICell[][]>(subTableSize.Height, subTableSize.Width);
            for(var x = rectangle.UpperLeft.ColumnIndex; x <= rectangle.LowerRight.ColumnIndex; ++x)
            {
                for(var y = rectangle.UpperLeft.RowIndex; y <= rectangle.LowerRight.RowIndex; ++y)
                {
                    ExcelCell cell;
                    if(!excelReferenceToCell.TryGetValue(new CellPosition(y, x).CellReference, out cell))
                        cell = new ExcelCell(internalTable.InsertCell(new ExcelCellIndex(y, x)));

                    subTable[y - rectangle.UpperLeft.RowIndex][x - rectangle.UpperLeft.ColumnIndex] = cell;
                }
            }

            return new ExcelTablePart(subTable);
        }

        public IEnumerable<IColumn> Columns { get { return internalTable.Columns.Select(c => new ExcelColumn(c)); } }

        public void MergeCells(IRectangle rectangle)
        {
            internalTable.MergeCells(new ExcelCellIndex(rectangle.UpperLeft.CellReference),
                                     new ExcelCellIndex(rectangle.LowerRight.CellReference));
        }

        public TExcelFormControlInfo TryGetFormControl<TExcelFormControlInfo>(string name)
            where TExcelFormControlInfo : class, IExcelFormControlInfo
        {
            return internalTable.GetFormControlInfo<TExcelFormControlInfo>(name);
        }
        
        public IFormControls GetFormControlsInfo()
        {
            return new ExcelFormControls(internalTable.GetFormControlsInfo());
        }

        public void AddFormControls(IFormControls formControls)
        {
            internalTable.AddFormControlInfos(formControls.ExcelFormControlsInfo);
        }

        public void ResizeColumn(int columnIndex, double width)
        {
            internalTable.ResizeColumn(columnIndex, width);
        }

        public IEnumerable<IRectangle> MergedCells
        {
            get
            {
                return internalTable.MergedCells
                                    .Select(tuple => new Rectangle(new CellPosition(tuple.Item1),
                                                                   new CellPosition(tuple.Item2)));
            }
        }

        private readonly IExcelWorksheet internalTable;
    }
}