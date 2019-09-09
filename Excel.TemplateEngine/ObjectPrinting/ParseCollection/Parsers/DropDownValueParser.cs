﻿using System;

using SKBKontur.Catalogue.ExcelObjectPrinter.TableParser;

namespace SKBKontur.Catalogue.ExcelObjectPrinter.ParseCollection.Parsers
{
    public class DropDownValueParser : IFormValueParser
    {
        public bool TryParse(ITableParser tableParser, string name, Type modelType, out object result)
        {
            if (!tableParser.TryParseDropDownValue(name, out var parseResult))
            {
                result = null;
                return false;
            }
            result = parseResult;
            return true;
        }

        public object ParseOrDefault(ITableParser tableParser, string name, Type modelType)
        {
            if (!TryParse(tableParser, name, modelType, out var result))
                result = null;
            return result;
        }
    }
}