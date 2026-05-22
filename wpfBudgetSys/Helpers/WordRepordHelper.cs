// Helpers/WordReportHelper.cs
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using wpfBudgetSys.Model;              // ✅ added — was missing
using wpfBudgetSys.MVVM;
// ✅ removed System.Transactions — this was causing the conflict

namespace wpfBudgetSys.Helpers
{
    public static class WordReportHelper
    {
        public static void GenerateTransactionReport(
            List<Transaction> transactions, string filePath)
        {
            using WordprocessingDocument doc = WordprocessingDocument
                .Create(filePath, WordprocessingDocumentType.Document);

            MainDocumentPart mainPart = doc.AddMainDocumentPart();
            mainPart.Document = new Document();
            Body body = mainPart.Document.AppendChild(new Body());

            // ── Title ─────────────────────────────────────────────
            body.AppendChild(CreateHeading("Transaction Report — Bawal Gastos"));
            body.AppendChild(CreateParagraph(
                $"Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}",
                fontSize: 20, bold: false));
            body.AppendChild(CreateParagraph(
                $"Account Owner: {SessionManager.CurrentUser?.Fullname}",
                fontSize: 20, bold: false));
            body.AppendChild(new Paragraph());

            // ── Table ─────────────────────────────────────────────
            body.AppendChild(CreateTransactionTable(transactions));

            // ── Summary ───────────────────────────────────────────
            body.AppendChild(new Paragraph());

            decimal totalCredits = transactions
                .Where(t => t.Type.Equals("credit", StringComparison.OrdinalIgnoreCase))
                .Sum(t => t.Amount);

            decimal totalDebits = transactions
                .Where(t => t.Type.Equals("debit", StringComparison.OrdinalIgnoreCase))
                .Sum(t => t.Amount);

            body.AppendChild(CreateParagraph(
                $"Total Credits: ₱{totalCredits:N2}", bold: true));
            body.AppendChild(CreateParagraph(
                $"Total Debits:  ₱{totalDebits:N2}", bold: true));

            mainPart.Document.Save();
        }

        private static Table CreateTransactionTable(List<Transaction> transactions)
        {
            Table table = new Table();

            TableProperties tblProps = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = BorderValues.Single, Size = 4 },
                    new BottomBorder { Val = BorderValues.Single, Size = 4 },
                    new LeftBorder { Val = BorderValues.Single, Size = 4 },
                    new RightBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                )
            );
            table.AppendChild(tblProps);

            // Header row
            table.AppendChild(CreateTableRow(
                new[] { "Date", "Reference No.", "Description",
                        "Type", "Category", "Amount" },
                isHeader: true
            ));

            // ✅ Fixed — removed extra })); that was causing syntax error
            foreach (Transaction t in transactions)
            {
                table.AppendChild(CreateTableRow(new string[]
                {
                    t.Date.ToString("MM/dd/yyyy"),
                    t.ReferenceNumber,
                    t.Description,
                    t.Type == "credit" ? "Credit" : "Debit",
                    t.CategoryName ?? "—",
                    $"₱{t.Amount:N2}"
                }));
            }

            return table;
        }

        private static TableRow CreateTableRow(string[] values, bool isHeader = false)
        {
            TableRow row = new TableRow();

            foreach (string value in values)
            {
                TableCell cell = new TableCell();

                TableCellProperties cellProps = new TableCellProperties(
                    new TableCellMargin(
                        new TopMargin { Width = "80", Type = TableWidthUnitValues.Dxa },
                        new BottomMargin { Width = "80", Type = TableWidthUnitValues.Dxa },
                        new LeftMargin { Width = "120", Type = TableWidthUnitValues.Dxa },
                        new RightMargin { Width = "120", Type = TableWidthUnitValues.Dxa }
                    )
                );

                if (isHeader)
                {
                    cellProps.AppendChild(new Shading
                    {
                        Fill = "003366",
                        Color = "FFFFFF",
                        Val = ShadingPatternValues.Clear
                    });
                }

                cell.AppendChild(cellProps);
                cell.AppendChild(new Paragraph(new Run(
                    new RunProperties(
                        new Bold { Val = OnOffValue.FromBoolean(isHeader) },
                        new Color { Val = isHeader ? "FFFFFF" : "000000" },
                        new FontSize { Val = "20" }
                    ),
                    new Text(value)
                )));

                row.AppendChild(cell);
            }

            return row;
        }

        private static Paragraph CreateParagraph(string text,
            int fontSize = 24, bool bold = false)
        {
            return new Paragraph(new Run(
                new RunProperties(
                    new Bold { Val = OnOffValue.FromBoolean(bold) },
                    new FontSize { Val = fontSize.ToString() }
                ),
                new Text(text)
            ));
        }

        private static Paragraph CreateHeading(string text)
        {
            return new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                ),
                new Run(
                    new RunProperties(
                        new Bold(),
                        new FontSize { Val = "36" },
                        new Color { Val = "003366" }
                    ),
                    new Text(text)
                )
            );
        }
    }
}