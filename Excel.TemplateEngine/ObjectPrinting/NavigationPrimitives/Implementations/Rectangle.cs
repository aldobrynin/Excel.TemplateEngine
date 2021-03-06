﻿using System;

namespace SkbKontur.Excel.TemplateEngine.ObjectPrinting.NavigationPrimitives.Implementations
{
    public class Rectangle : IRectangle
    {
        public Rectangle(ICellPosition upperLeft, IObjectSize size)
        {
            UpperLeft = upperLeft;
            Size = size;
            LowerRight = upperLeft.Add(Size.Subtract(new ObjectSize(1, 1)));
        }

        public Rectangle(ICellPosition upperLeft, ICellPosition lowerRight)
        {
            //Дерьмо ниже на случай, если кто-то перепутает позиции.
            UpperLeft = new CellPosition(Math.Min(upperLeft.RowIndex, lowerRight.RowIndex),
                                         Math.Min(upperLeft.ColumnIndex, lowerRight.ColumnIndex));
            LowerRight = new CellPosition(Math.Max(upperLeft.RowIndex, lowerRight.RowIndex),
                                          Math.Max(upperLeft.ColumnIndex, lowerRight.ColumnIndex));
            Size = lowerRight.Subtract(upperLeft).Add(new ObjectSize(1, 1));
        }

        public ICellPosition UpperLeft { get; private set; }
        public ICellPosition LowerRight { get; private set; }
        public IObjectSize Size { get; private set; }

        public bool Intersects(IRectangle rect)
        {
            if (rect.UpperLeft.ColumnIndex <= LowerRight.ColumnIndex &&
                UpperLeft.ColumnIndex <= rect.LowerRight.ColumnIndex &&
                rect.UpperLeft.RowIndex <= LowerRight.RowIndex)
                return UpperLeft.RowIndex <= rect.LowerRight.RowIndex;
            return false;
        }

        public bool Contains(ICellPosition position)
        {
            return position.RowIndex >= UpperLeft.RowIndex &&
                   position.RowIndex <= LowerRight.RowIndex &&
                   position.ColumnIndex >= UpperLeft.ColumnIndex &&
                   position.ColumnIndex <= LowerRight.ColumnIndex;
        }

        public bool Contains(IRectangle rect)
        {
            return Contains(rect.UpperLeft) &&
                   Contains(rect.LowerRight);
        }
    }
}