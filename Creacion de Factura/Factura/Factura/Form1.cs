using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Factura
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cod;
            int nom;
            float precio;
            cod = cmbProducto.SelectedIndex;
            nom = cmbProducto.SelectedIndex; //.ToString();
            precio = cmbProducto.SelectedIndex;
            switch (cod)
            {
                case 0:lblCodigo.Text = "00011";break;
                case 1:lblCodigo.Text = "00022"; break;
                case 2:lblCodigo.Text = "00033"; break;
                case 3:lblCodigo.Text = "00044"; break;
                default:lblCodigo.Text = "00055";break;
            }
            switch (nom)
            {
                case 0: lblNombre.Text = "POLERA"; break;
                case 1: lblNombre.Text = "CAMISA"; break;
                case 2:lblNombre.Text = "GORRA";break;
                case 3:lblNombre.Text = "PANTALON";break;
                default: lblNombre.Text = "ZAPATILLA"; break;

            }
            switch (precio)
            {
                case 0: lblPrecio.Text = "50"; break;
                case 1: lblPrecio.Text = "70"; break;
                case 2: lblPrecio.Text = "30"; break;
                case 3: lblPrecio.Text = "80"; break;
                default: lblPrecio.Text = "130"; break;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewRow file = new DataGridViewRow();
            file.CreateCells(dgvLista);
            file.Cells[0].Value = lblCodigo.Text;
            file.Cells[1].Value = lblNombre.Text;
            file.Cells[2].Value = lblPrecio.Text;
            file.Cells[3].Value = txtCantidad.Text;
            file.Cells[4].Value = (float.Parse(lblPrecio.Text) * float.Parse(txtCantidad.Text)).ToString();
            dgvLista.Rows.Add(file);
            ObtenerTotal();
        }
        public void ObtenerTotal()
        {
            float costot = 0;
            int contador = 0;
            contador = dgvLista.RowCount;
            for (int i = 0; i <contador; i++)
            {
                costot += float.Parse(dgvLista.Rows[i].Cells[4].Value.ToString());
            }
            lblTotalAPagar.Text = costot.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult rppta = MessageBox.Show("Desea eliminar producto?",
                    "Eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rppta == DialogResult.Yes)
                {
                    dgvLista.Rows.Remove(dgvLista.CurrentRow);
                }
            }
            catch { }
            ObtenerTotal();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblDevolucion.Text = (float.Parse(txtEfectivo.Text) - float.Parse(lblTotalAPagar.Text)).ToString();
            }
            catch { }
        }
        public static bool Isnumeric(String cad)
        {
            Boolean isnumeric= int.TryParse(cad,out _);

            return isnumeric;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (txtEfectivo.Text != "")
            {
                if (Isnumeric(txtEfectivo.Text) == true)
                {
                    if(Int32.Parse(txtEfectivo.Text) >= Int32.Parse(lblTotalAPagar.Text))
                    {
                        clsFactura.CreaTicket Ticket1 = new clsFactura.CreaTicket();

                        Ticket1.TextoCentro("Empresa RECM "); //imprime una linea de descripcion
                        Ticket1.TextoCentro("**********************************");
                        Ticket1.TextoCentro("Factura de Venta"); //imprime una linea de descripcion
                        Ticket1.TextoIzquierda("No Fac: 0120541");
                        Ticket1.TextoIzquierda("Fecha:" + DateTime.Now.ToShortDateString() + " Hora:" + DateTime.Now.ToShortTimeString());
                        Ticket1.TextoIzquierda("Le Atendio: Rene Chungara");
                        Ticket1.TextoIzquierda("");
                        clsFactura.CreaTicket.LineasGuion();

                        clsFactura.CreaTicket.EncabezadoVenta();
                        clsFactura.CreaTicket.LineasGuion();
                        foreach (DataGridViewRow r in dgvLista.Rows)
                        {
                            // PROD                     //PrECIO                                    CANT                         TOTAL
                            Ticket1.AgregaArticulo(r.Cells[1].Value.ToString(), double.Parse(r.Cells[2].Value.ToString()), int.Parse(r.Cells[3].Value.ToString()), double.Parse(r.Cells[4].Value.ToString())); //imprime una linea de descripcion
                        }


                        clsFactura.CreaTicket.LineasGuion();

                        Ticket1.AgregaTotales("Total", double.Parse(lblTotalAPagar.Text)); // imprime linea con total
                        Ticket1.TextoIzquierda(" ");
                        Ticket1.AgregaTotales("Efectivo Entregado:", double.Parse(txtEfectivo.Text));
                        Ticket1.AgregaTotales("Cambio:", double.Parse(lblDevolucion.Text));


                        // Ticket1.LineasTotales(); // imprime linea 

                        Ticket1.TextoIzquierda(" ");
                        Ticket1.TextoCentro("**********************************");
                        Ticket1.TextoCentro("*     Gracias por preferirnos    *");

                        Ticket1.TextoCentro("**********************************");
                        Ticket1.TextoIzquierda(" ");
                        string impresora = "Microsoft XPS Document Writer";
                        Ticket1.ImprimirTiket(impresora);
                        while (dgvLista.RowCount > 0)//limpia el dgv
                        { dgvLista.Rows.Remove(dgvLista.CurrentRow); }
                        //LBLIDnuevaFACTURA.Text = ClaseFunciones.ClsFunciones.IDNUEVAFACTURA().ToString();

                        lblCodigo.Text = lblNombre.Text = txtEfectivo.Text = "";
                        lblTotalAPagar.Text = lblDevolucion.Text = lblPrecio.Text = "0";
                        txtCantidad.Value = 1;
                        MessageBox.Show("Gracias por preferirnos");
                    }
                    else
                    {
                        MessageBox.Show("Efectivo Insuficiente");
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese solo numeros");
                }
            }
            else
            {
                MessageBox.Show("Ingrese la Cantidad de efectivo a pagar");

            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
