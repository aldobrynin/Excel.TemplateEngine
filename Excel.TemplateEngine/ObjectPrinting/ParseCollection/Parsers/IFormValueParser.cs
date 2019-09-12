using System;

using Excel.TemplateEngine.ObjectPrinting.TableParser;

using JetBrains.Annotations;

namespace Excel.TemplateEngine.ObjectPrinting.ParseCollection.Parsers
{
    internal interface IFormValueParser
    {
        object ParseOrDefault([NotNull] ITableParser tableParser, [NotNull] string name, [NotNull] Type modelType);
    }
}