using Datos;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.CorteDeCaja
{
    public partial class frmReporteCaja : Form
    {
        public frmReporteCaja()
        {
            InitializeComponent();
        }

        

        private void frmReporteCaja_Load(object sender, EventArgs e)
        {
            /*List<Ventas> pruebaVentas = new List<Ventas>();
            pruebaVentas.Add(new Ventas(1, 100, "2020-13-11", 1));
            pruebaVentas.Add(new Ventas(2, 200, "2020-13-11", 1));
            pruebaVentas.Add(new Ventas(3, 150, "2020-13-11", 1));
            pruebaVentas.Add(new Ventas(4, 300, "2020-13-11", 1));
            pruebaVentas.Add(new Ventas(5, 100, "2020-13-11", 1));*/
            SettingsBindingSource.DataSource = Properties.Settings.Default;
            this.reportViewer1.RefreshReport();
        }
    }
}
