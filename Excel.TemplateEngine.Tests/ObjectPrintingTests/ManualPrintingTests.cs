﻿using System.IO;
using System.Linq;

using NUnit.Framework;

namespace Excel.TemplateEngine.Tests.ExcelObjectPrinterTests
{
    [Ignore("These tests can be used only for manual testing")]
    public class ManualPrintingTests : FileBasedTestBase
    {
        [Test]
        public void TestPrintingDropDownFromTheOtherWorksheet()
        {
            using (var templateDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("printingDropDownFromTheOtherWorksheet.xlsx")), Log.DefaultLogger))
            using (var targetDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("empty.xlsx")), Log.DefaultLogger))
            {
                targetDocument.CopyVbaInfoFrom(templateDocument);

                foreach (var index in Enumerable.Range(1, templateDocument.GetWorksheetCount() - 1))
                {
                    var worksheet = templateDocument.GetWorksheet(index);
                    var name = templateDocument.GetWorksheetName(index);
                    var innerTemplateEngine = new TemplateEngine(new ExcelTable(worksheet), Log.DefaultLogger);
                    var targetWorksheet = targetDocument.AddWorksheet(name);
                    var innerTableBuilder = new TableBuilder(new ExcelTable(targetWorksheet), new TableNavigator(new CellPosition("A1"), Log.DefaultLogger));
                    innerTemplateEngine.Render(innerTableBuilder, new {});
                }

                var template = new ExcelTable(templateDocument.GetWorksheet(0));
                var templateEngine = new TemplateEngine(template, Log.DefaultLogger);

                var target = new ExcelTable(targetDocument.GetWorksheet(0));
                var tableNavigator = new TableNavigator(new CellPosition("A1"), Log.DefaultLogger);
                var tableBuilder = new TableBuilder(target, tableNavigator, new Style(template.GetCell(new CellPosition("A1"))));
                templateEngine.Render(tableBuilder, new {Type = "Значение 2"});

                var filename = "output.xlsx";
                File.WriteAllBytes(filename, targetDocument.CloseAndGetDocumentBytes());

                var path = "file:///" + Path.GetFullPath(filename).Replace("\\", "/");
                Assert.Fail($"Please manually open file '{path}' and check that dropdown on the first sheet has value 'Значение 2'");
            }
        }

        [Test]
        public void TestPrintingVbaMacros()
        {
            using (var templateDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("printingVbaMacros.xlsm")), Log.DefaultLogger))
            using (var targetDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("empty.xlsm")), Log.DefaultLogger))
            {
                targetDocument.CopyVbaInfoFrom(templateDocument);

                var template = new ExcelTable(templateDocument.GetWorksheet(0));
                var templateEngine = new TemplateEngine(template, Log.DefaultLogger);

                var target = new ExcelTable(targetDocument.GetWorksheet(0));
                var tableNavigator = new TableNavigator(new CellPosition("A1"), Log.DefaultLogger);
                var tableBuilder = new TableBuilder(target, tableNavigator, new Style(template.GetCell(new CellPosition("A1"))));
                templateEngine.Render(tableBuilder, new {Type = "123"});

                var filename = "output.xlsm";
                File.WriteAllBytes(filename, targetDocument.CloseAndGetDocumentBytes());

                var path = "file:///" + Path.GetFullPath(filename).Replace("\\", "/");
                Assert.Fail($"Please manually open file '{path}' and check that clicking on the right checkbox leads to changes in both checkbox");
            }
        }

        [Test]
        public void TestColumnsSwitching()
        {
            using (var templateDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("columnsSwitching.xlsx")), Log.DefaultLogger))
            using (var targetDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("empty.xlsx")), Log.DefaultLogger))
            {
                var template = new ExcelTable(templateDocument.GetWorksheet(0));
                var templateEngine = new TemplateEngine(template, Log.DefaultLogger);

                var target = new ExcelTable(targetDocument.GetWorksheet(0));
                var tableNavigator = new TableNavigator(new CellPosition("A1"), Log.DefaultLogger);
                var tableBuilder = new TableBuilder(target, tableNavigator, new Style(template.GetCell(new CellPosition("A1"))));
                templateEngine.Render(tableBuilder, new {A = "First", B = true, C = "Third", D = new[] {1, 2, 3}, E = "Fifth"});

                var filename = "output.xlsx";
                File.WriteAllBytes(filename, targetDocument.CloseAndGetDocumentBytes());

                var path = "file:///" + Path.GetFullPath(filename).Replace("\\", "/");
                Assert.Fail($"Please manually open file '{path}' and check values:\n" +
                            string.Join("\n", new[]
                                {
                                    "C5: First",
                                    "D5: <empty>",
                                    "E5: Third",
                                    "F5: 1",
                                    "F6: 2",
                                    "F7: 3",
                                    "G5: Fifth",
                                    "Флажок 1: checked",
                                }) + "\n");
            }
        }

        [Test]
        public void TestDataValidations()
        {
            using (var templateDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("dataValidations.xlsx")), Log.DefaultLogger))
            using (var targetDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("empty.xlsx")), Log.DefaultLogger))
            {
                var template = new ExcelTable(templateDocument.GetWorksheet(0));
                var templateEngine = new TemplateEngine(template, Log.DefaultLogger);

                var target = new ExcelTable(targetDocument.GetWorksheet(0));
                var tableNavigator = new TableNavigator(new CellPosition("A1"), Log.DefaultLogger);
                var tableBuilder = new TableBuilder(target, tableNavigator, new Style(template.GetCell(new CellPosition("A1"))));
                templateEngine.Render(tableBuilder, new {Test = "b"});

                var filename = "output.xlsx";
                File.WriteAllBytes(filename, targetDocument.CloseAndGetDocumentBytes());

                var path = "file:///" + Path.GetFullPath(filename).Replace("\\", "/");
                Assert.Fail($"Please manually open file '{path}' and check that:\n\n" +
                            "Cell C4 has validation with variants abc, cde and lalala\n" +
                            "Cell E6 has validation with variants a, b, c and value b\n");
            }
        }

        [Test]
        public void TestColors()
        {
            using (var templateDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("colors.xlsx")), Log.DefaultLogger))
            using (var targetDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("empty.xlsx")), Log.DefaultLogger))
            {
                var template = new ExcelTable(templateDocument.GetWorksheet(0));
                var templateEngine = new TemplateEngine(template, Log.DefaultLogger);

                var target = new ExcelTable(targetDocument.GetWorksheet(0));
                var tableNavigator = new TableNavigator(new CellPosition("A1"), Log.DefaultLogger);
                var tableBuilder = new TableBuilder(target, tableNavigator, new Style(template.GetCell(new CellPosition("A1"))));
                templateEngine.Render(tableBuilder, new {});

                var filename = "output.xlsx";
                File.WriteAllBytes(filename, targetDocument.CloseAndGetDocumentBytes());

                var path = "file:///" + Path.GetFullPath(filename).Replace("\\", "/");
                var templatePath = "file:///" + Path.GetFullPath("ExcelObjectPrinterTests/Files/colors.xlsx").Replace("\\", "/");
                Assert.Fail($"Please manually open file:\n{path}\nand check that cells has same colors as in\n{templatePath}\n");
            }
        }

        [Test]
        public void TestDataValidationsFromTheOtherWorksheet()
        {
            using (var templateDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("otherSheetDataValidations.xlsx")), Log.DefaultLogger))
            using (var targetDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("empty.xlsx")), Log.DefaultLogger))
            {
                targetDocument.CopyVbaInfoFrom(templateDocument);

                foreach (var index in Enumerable.Range(1, templateDocument.GetWorksheetCount() - 1))
                {
                    var worksheet = templateDocument.GetWorksheet(index);
                    var name = templateDocument.GetWorksheetName(index);
                    var innerTemplateEngine = new TemplateEngine(new ExcelTable(worksheet), Log.DefaultLogger);
                    var targetWorksheet = targetDocument.AddWorksheet(name);
                    var innerTableBuilder = new TableBuilder(new ExcelTable(targetWorksheet), new TableNavigator(new CellPosition("A1"), Log.DefaultLogger));
                    innerTemplateEngine.Render(innerTableBuilder, new {});
                }

                var template = new ExcelTable(templateDocument.GetWorksheet(0));
                var templateEngine = new TemplateEngine(template, Log.DefaultLogger);

                var target = new ExcelTable(targetDocument.GetWorksheet(0));
                var tableNavigator = new TableNavigator(new CellPosition("A1"), Log.DefaultLogger);
                var tableBuilder = new TableBuilder(target, tableNavigator, new Style(template.GetCell(new CellPosition("A1"))));
                templateEngine.Render(tableBuilder, new {});

                var filename = "output.xlsx";
                File.WriteAllBytes(filename, targetDocument.CloseAndGetDocumentBytes());

                var path = "file:///" + Path.GetFullPath(filename).Replace("\\", "/");
                Assert.Fail($"Please manually open file '{path}' and check that D4-D7 has data validation with values from the second worksheet and G4-G7 has data validation with values from K1:K6");
            }
        }

        [Test]
        public void TestPrintingComments()
        {
            using (var templateDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("comments.xlsx")), Log.DefaultLogger))
            using (var targetDocument = ExcelDocumentFactory.CreateFromTemplate(File.ReadAllBytes(GetFilePath("empty.xlsx")), Log.DefaultLogger))
            {
                var template = new ExcelTable(templateDocument.GetWorksheet(0));
                var templateEngine = new TemplateEngine(template, Log.DefaultLogger);

                var target = new ExcelTable(targetDocument.GetWorksheet(0));
                var tableNavigator = new TableNavigator(new CellPosition("A1"), Log.DefaultLogger);
                var tableBuilder = new TableBuilder(target, tableNavigator, new Style(template.GetCell(new CellPosition("A1"))));
                templateEngine.Render(tableBuilder, new {});

                var filename = "output.xlsx";
                File.WriteAllBytes(filename, targetDocument.CloseAndGetDocumentBytes());

                var path = "file:///" + Path.GetFullPath(filename).Replace("\\", "/");
                var templatePath = "file:///" + Path.GetFullPath("ExcelObjectPrinterTests/Files/comments.xlsx").Replace("\\", "/");
                Assert.Fail($"Please manually open file:\n{path}\nand check that cells has same comments as in\n{templatePath}\n");
            }
        }
    }
}