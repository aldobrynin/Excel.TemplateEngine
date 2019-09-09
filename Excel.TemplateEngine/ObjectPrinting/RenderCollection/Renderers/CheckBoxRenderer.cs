﻿using SKBKontur.Catalogue.ExcelObjectPrinter.TableBuilder;

namespace SKBKontur.Catalogue.ExcelObjectPrinter.RenderCollection.Renderers
{
    public class CheckBoxRenderer : IFormControlRenderer
    {
        public void Render([NotNull] ITableBuilder tableBuilder, [NotNull] string name, [NotNull] object model)
        {
            if (!(model is bool boolToRender))
                throw new InvalidProgramStateException("model is not bool");

            tableBuilder.RenderCheckBoxValue(name, boolToRender);
        }
    }
}