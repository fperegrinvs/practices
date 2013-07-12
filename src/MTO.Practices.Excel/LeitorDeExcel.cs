namespace MTO.Practices.Common.Excel
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    /// <summary>
    /// Manipula arquivos do Excel.
    /// </summary>
    public class LeitorDeExcel
    {
        /// <summary>
        /// Delegate para uso interno. Usado para obter os dados das planilhas de forma asincrona.
        /// </summary>
        /// <param name="planilha">Representa uma planilha (Worksheet) do Excel.</param>
        /// <param name="primeiraLinhaContemTituloDeColuna">Determina se a primeira linha da planilha será utilizada para nomear as colunas do DataTable.</param>
        /// <returns>DataTable com os dados contidos na planilha.</returns>
        private delegate DataTable ObterDadosPlanilhaAsyncHandler(
            ExcelWorksheet planilha, bool primeiraLinhaContemTituloDeColuna);


        /// <summary>Obtem dados de uma pasta de trabalho do Excel (arquivo) específico.</summary>
        /// <param name="caminhoArquivo">
        /// Caminho completo do arquivo do Excel.
        /// </param>
        /// <param name="primeiraLinhaContemTituloDeColuna">Determina se a primeira linha das planilhas serão utilizadas para nomear as colunas do DataTable.</param>
        /// <returns>
        /// DataSet com dados do Excel onde cada DataTable representa uma planilha.
        /// </returns>
        public DataSet ObterDadosExcel(string caminhoArquivo, bool primeiraLinhaContemTituloDeColuna)
        {
            DataSet ds = null;
            var informacoesArquivo = new FileInfo(caminhoArquivo);
            using (var arquivo = new FileStream(informacoesArquivo.FullName, FileMode.Open, FileAccess.Read))
            {
                ds = this.ObterDadosExcel(informacoesArquivo.Name, arquivo, primeiraLinhaContemTituloDeColuna);
            }

            return ds;
        }

        public DataSet ObterDadosExcel(string nomeArquivo, Stream conteudo, bool primeiraLinhaContemTituloDeColuna)
        {
            var ds = new DataSet(nomeArquivo);

            using (var p = new ExcelPackage())
            {
                p.Load(conteudo);

                foreach (var planilha in p.Workbook.Worksheets)
                {
                    var dt = this.ObterDadosPlanilha(planilha, primeiraLinhaContemTituloDeColuna);
                    if (dt != null)
                    {
                        ds.Tables.Add(dt);
                    }
                }
            }

            return ds;
        }

        /// <summary>Obtem dados de uma pasta de trabalho do Excel (arquivo) específico.</summary>
        /// <param name="caminhoArquivo">
        /// Caminho completo do arquivo do Excel.
        /// </param>
        /// <param name="primeiraLinhaContemTituloDeColuna">Determina se a primeira linha das planilhas serão utilizadas para nomear as colunas do DataTable.</param>
        /// <returns>
        /// DataSet com dados do Excel onde cada DataTable representa uma planilha.
        /// </returns>
        public DataSet ObterDadosExcelAsync(string caminhoArquivo, bool primeiraLinhaContemTituloDeColuna)
        {
            DataSet ds = null;
            var informacoesArquivo = new FileInfo(caminhoArquivo);
            using (var arquivo = new FileStream(informacoesArquivo.FullName, FileMode.Open, FileAccess.Read))
            {
                ds = this.ObterDadosExcelAsync(informacoesArquivo.Name, arquivo, primeiraLinhaContemTituloDeColuna);
            }

            return ds;
        }

        public DataSet ObterDadosExcelAsync(string nomeArquivo, Stream conteudo, bool primeiraLinhaContemTituloDeColuna)
        {
            var ds = new DataSet(nomeArquivo);

            using (var p = new ExcelPackage())
            {
                p.Load(conteudo);

                var ars = new List<IAsyncResult>();
                ObterDadosPlanilhaAsyncHandler function = this.ObterDadosPlanilha;

                foreach (var planilha in p.Workbook.Worksheets)
                {
                    ars.Add(function.BeginInvoke(planilha, primeiraLinhaContemTituloDeColuna, null, function));
                }

                foreach (IAsyncResult result in ars)
                {
                    var dt = function.EndInvoke(result);
                    if (dt != null)
                    {
                        ds.Tables.Add(dt);
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// Obtem dados de uma planilha especifica dentro de uma pasta de trabalho do Excel.
        /// </summary>
        /// <param name="planilha">Representa uma planilha (Worksheet) do Excel.</param>
        /// <param name="primeiraLinhaContemTituloDeColuna">Determina se a primeira linha da planilha será utilizada para nomear as colunas do DataTable.</param>
        /// <returns>DataTable com os dados contidos na planilha.</returns>
        private DataTable ObterDadosPlanilha(ExcelWorksheet planilha, bool primeiraLinhaContemTituloDeColuna)
        {
            if (planilha.Dimension == null)
            {
                return null;
            }

            var dt = new DataTable(planilha.Name);

            int linhaInicial = 1;

            for (int i = 1; i <= planilha.Dimension.End.Column; i++)
            {
                if (primeiraLinhaContemTituloDeColuna)
                {
                    string nomeDataTable = planilha.GetValue(1, i) == null
                                               ? i.ToString()
                                               : planilha.GetValue(1, i).ToString();
                    dt.Columns.Add(nomeDataTable);
                    linhaInicial = 2;
                }
                else
                {
                    dt.Columns.Add(i.ToString());
                }
            }

            for (int i = linhaInicial; i <= planilha.Dimension.End.Row; i++)
            {
                var dr = dt.NewRow();
                for (int j = 1; j <= planilha.Dimension.End.Column; j++)
                {
                    int indiceDataTable = j - 1;
                    dr[indiceDataTable] = planilha.GetValue(i, j);
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public void SalvarPastaDeTrabalho(DataSet dadosExcel, string caminhoArquivo, bool usarNomeDasColunas)
        {
            var fileInfo = new FileInfo(caminhoArquivo);
            using (var p = new ExcelPackage(fileInfo))
            {
                this.GerarPlanilha(p, dadosExcel, usarNomeDasColunas);
                p.Save();
            }
        }

        public Stream GerarPastaDeTrabalho(DataSet dadosExcel, bool usarNomeDasColunas)
        {
            var stream = new MemoryStream();

            using (var p = new ExcelPackage(stream))
            {
                this.GerarPlanilha(p, dadosExcel, usarNomeDasColunas);
                p.Save();
            }

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        private void GerarPlanilha(ExcelPackage excelPackage, DataSet dadosExcel, bool usarNomeDasColunas)
        {
            foreach (DataTable tabela in dadosExcel.Tables)
            {
                var ws = excelPackage.Workbook.Worksheets.Add(tabela.TableName);

                for (int i = 0; i < tabela.Rows.Count; i++)
                {
                    for (int j = 0; j < tabela.Columns.Count; j++)
                    {
                        int linha = i + 1;
                        int coluna = j + 1;

                        if (usarNomeDasColunas && i.Equals(0))
                        {
                            ws.Cells[linha, coluna].Value = tabela.Columns[j].ColumnName;
                            ws.Cells[linha, coluna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[linha, coluna].Style.Fill.PatternColor.SetColor(Color.LightGray);
                            ws.Cells[linha, coluna].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            ws.Cells[linha, coluna].Style.Font.Bold = true;
                        }

                        if (usarNomeDasColunas)
                        {
                            linha++;
                        }

                        if (!tabela.Rows[i].IsNull(j))
                        {
                            ws.Cells[linha, coluna].Value = tabela.Rows[i][j];
                        }
                    }
                }

                ws.Cells.AutoFitColumns();
            }
        }
    }
}