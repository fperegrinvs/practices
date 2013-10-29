using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MTO.Practices.Common.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTO.Practices.Common.Tests
{
    [TestClass]
    public class LeitorDeExcelTests
    {
        public LeitorDeExcelTests()
        {
            this.excel = new LeitorDeExcel();
            this.ds = new DataSet();
            this.MontarDadosFake();
        }

        private readonly DataSet ds;
        private readonly LeitorDeExcel excel;

        [TestMethod]
        [TestCategory("Manual")]
        public void SalvarPlanilhaSemNomeDeCampos()
        {
            excel.SalvarPastaDeTrabalho(ds, @"C:\temp\testeExcelSemNomeDeCampos.xlsx", false);
            Assert.AreEqual(true, System.IO.File.Exists(@"C:\temp\testeExcelSemNomeDeCampos.xlsx"));
        }

        [TestMethod]
        [TestCategory("Manual")]
        public void SalvarPlanilhaComNomeDeCampos()
        {
            excel.SalvarPastaDeTrabalho(ds, @"C:\temp\testeExcelComNomeDeCampos.xlsx", true);
            Assert.AreEqual(true, System.IO.File.Exists(@"C:\temp\testeExcelComNomeDeCampos.xlsx"));
        }

        private void MontarDadosFake()
        {
            var tabela1 = new DataTable("Talela1");
            tabela1.Columns.Add("col_a");
            tabela1.Columns.Add("col_b");
            tabela1.Columns.Add("col_c");
            this.ds.Tables.Add(tabela1);

            var tabela2 = new DataTable("Talela2");
            tabela2.Columns.Add("col_1");
            tabela2.Columns.Add("col_2");
            tabela2.Columns.Add("col_3");
            this.ds.Tables.Add(tabela2);

            var linha1 = tabela1.NewRow();
            tabela1.Rows.Add(linha1);
            linha1["col_a"] = "Edson";
            linha1["col_b"] = "Parizoti";
            linha1["col_c"] = "Junior";

            var linha1T2 = tabela2.NewRow();
            tabela2.Rows.Add(linha1T2);
            linha1T2["col_1"] = "C#";
            linha1T2["col_2"] = 50M;
            linha1T2["col_3"] = DateTime.Today;

            var linha2T2 = tabela2.NewRow();
            tabela2.Rows.Add(linha2T2);
            linha2T2["col_1"] = "Java";
            linha2T2["col_2"] = 50M;
            linha2T2["col_3"] = DateTime.Today.AddDays(5);
        }
    }
}
