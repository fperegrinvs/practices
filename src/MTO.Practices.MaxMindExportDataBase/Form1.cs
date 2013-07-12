using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MTO.Practices.MaxMindExportDataBase
{
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public const string ArquivoDAT = @"C:\GeoLiteCity.dat";

        private void btnExportar_Click(object sender, EventArgs e)
        {
            var service = new LookupService(ArquivoDAT);
            var location = service.getLocation("201.86.39.240");
        }
    }
}
