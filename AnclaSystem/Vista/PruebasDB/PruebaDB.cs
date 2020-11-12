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

namespace Vista.PruebasDB
{
    public partial class PruebaDB : Form
    {
        public PruebaDB()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Conexion cn = new Conexion();
            MessageBox.Show(cn.ProbarConexion().ToString());
        }
    }
}
