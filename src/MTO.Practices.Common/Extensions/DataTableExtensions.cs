namespace MTO.Practices.Common.Extensions
{
    using System.Data;
    using System.Text;

    /// <summary>
    /// Extensores de datatable
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Transforma uma DataTable em html table numa string
        /// </summary>
        /// <param name="dt">A datatable</param>
        /// <returns>A string com a html table</returns>
        public static string ToHtml(this DataTable dt)
        {
            string tab = "\t";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(tab + tab + "<table border=\"1\">");

            // headers.
            sb.Append(tab + tab + tab + "<tr>");

            foreach (DataColumn dc in dt.Columns)
            {
                sb.AppendFormat("<th>{0}</th>", dc.ColumnName);
            }

            sb.AppendLine("</tr>");

            // data rows
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(tab + tab + tab + "<tr>");

                foreach (DataColumn dc in dt.Columns)
                {
                    string cellValue = dr[dc] != null ? dr[dc].ToString() : string.Empty;
                    sb.AppendFormat("<td>{0}</td>", cellValue);
                }

                sb.AppendLine("</tr>");
            }

            sb.AppendLine(tab + tab + "</table>");

            return sb.ToString();
        }

        /// <summary>
        /// Transforma um DataSet em html tables numa string
        /// </summary>
        /// <param name="ds">O DataSet</param>
        /// <returns>A string com uma html table pra cada DataTable do DataSet</returns>
        public static string ToHtml(this DataSet ds)
        {
            var sb = new StringBuilder();
            foreach (DataTable table in ds.Tables)
            {
                sb.Append("<br/><b>" + table.TableName + ":</b>");
                sb.AppendLine("<br/>");
                sb.Append(table.ToHtml());
            }

            return sb.ToString();
        }
    }
}
