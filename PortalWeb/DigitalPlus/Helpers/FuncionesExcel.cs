using OfficeOpenXml;

namespace DigitalPlus.Helpers
{
    public class FuncionesExcel
    {
        public static ExcelPackage createExcelPackage()
        {
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Prueba del Titulo";
            package.Workbook.Properties.Author = "Digital One";
            package.Workbook.Properties.Subject = "Prueba del Subject";
            package.Workbook.Properties.Keywords = "Prueba de Keywords";

            var worksheet = package.Workbook.Worksheets.Add("Fichada");
            // Headers
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Nombre";
            worksheet.Cells[1, 3].Value = "Apellido";
            worksheet.Cells[1, 4].Value = "Fecha";
            worksheet.Cells[1, 5].Value = "Ingresos";


            //Valores
            var numberFormat = "#.##0";
            var dataCellStyleName = "TableNumber";
            var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            numStyle.Style.Numberformat.Format = numberFormat;

            //primer registro
            worksheet.Cells[2, 1].Value = 1;
            worksheet.Cells[2, 2].Value = "Gustavo";
            worksheet.Cells[2, 3].Value = "Assad";
            worksheet.Cells[2, 4].Value = DateTime.Today.ToShortDateString();
            worksheet.Cells[2, 5].Value = 5000000;
            worksheet.Cells[2, 5].Style.Numberformat.Format = numberFormat;

            //segundo registro
            worksheet.Cells[3, 1].Value = 2;
            worksheet.Cells[3, 2].Value = "Diego";
            worksheet.Cells[3, 3].Value = "Beltran";
            worksheet.Cells[3, 4].Value = DateTime.Today.ToShortDateString();
            worksheet.Cells[3, 5].Value = 3500000;
            worksheet.Cells[3, 5].Style.Numberformat.Format = numberFormat;

            //tercer registro
            worksheet.Cells[4, 1].Value = 2;
            worksheet.Cells[4, 2].Value = "Blanco";
            worksheet.Cells[4, 3].Value = "Encalada";
            worksheet.Cells[4, 4].Value = DateTime.Today.ToShortDateString();
            worksheet.Cells[4, 5].Value = 400000;
            worksheet.Cells[4, 5].Style.Numberformat.Format = numberFormat;


            //autofit
            worksheet.Cells[1, 1, 4, 4].AutoFitColumns();
            return package;

        }
    }
}
