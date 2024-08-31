using System;

namespace Expenses_Management_System.Controllers
{
    internal class ExcelPackage : IDisposable
    {
        public object Workbook { get; internal set; }
    }
}