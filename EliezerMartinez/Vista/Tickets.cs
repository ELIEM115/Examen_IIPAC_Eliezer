using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vista
{
    public partial class Tickets : Form
    {
        public Tickets()
        {
            InitializeComponent();
        }
        Cliente miCliente = null;
        ClienteDB clienteDB = new ClienteDB();
        List<DetalleTicket> listaDetalles = new List<DetalleTicket>();
        TicketDB ticketDB = new TicketDB();
        decimal subTotal = 0;
        decimal isv = 0;
        decimal totalAPagar = 0;
        decimal descuento = 0;

        private void GuardarButton_Click(object sender, EventArgs e)
        {
            Ticket miTicket = new Ticket();
            miTicket.Fecha = FechaDateTimePicker.Value;
            miTicket.CodigoUsuario = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            miTicket.IdentidadCliente = miCliente.Identidad;
            miTicket.SubTotal = subTotal;
            miTicket.ISV = isv;
            miTicket.Descuento = descuento;
            miTicket.Total = totalAPagar;

            bool inserto = ticketDB.Guardar(miTicket, listaDetalles);

            if (inserto)
            {
                IdentidadTextBox.Focus();
                MessageBox.Show("Ticket registrado exitosamente");
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
                LimpiarControles();
            }
            else
                MessageBox.Show("No se pudo registrar el Ticket");
        }

        private void LimpiarControles()
        {
            miCliente = null;
            listaDetalles = null;
            FechaDateTimePicker.Value = DateTime.Now;
            IdentidadTextBox.Clear();
            NombreClienteTextBox.Clear();
            TipoSoporteComboBox.Text = "";
            DescripcionRespuestaTextBox.Clear();
            DescripcionSolicitudTextBox.Clear();
            DetalleDataGridView.DataSource = null;
            PrecioTextBox.Clear();
            subTotal = 0;
            SubTotalTextBox.Clear();
            isv = 0;
            ISVTextBox.Clear();
            descuento = 0;
            DescuentoTextBox.Clear();
            totalAPagar = 0;
            TotalTextBox.Clear();
        }

        private void DescuentoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(DescuentoTextBox.Text))
            {
                descuento = Convert.ToDecimal(DescuentoTextBox.Text);
                totalAPagar = totalAPagar - descuento;
                TotalTextBox.Text = totalAPagar.ToString();
            }
        }

        private void IdentidadTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(IdentidadTextBox.Text))
            {
                miCliente = new Cliente();
                miCliente = clienteDB.DevolverClientePorIdentidad(IdentidadTextBox.Text);
                NombreClienteTextBox.Text = miCliente.Nombre;
            }
            else
            {
                miCliente = null;
                NombreClienteTextBox.Clear();
            }
        }

        private void BuscarButton_Click(object sender, EventArgs e)
        {
            BuscarClienteForm form = new BuscarClienteForm();
            form.ShowDialog();
            miCliente = new Cliente();
            miCliente = form.cliente;
            IdentidadTextBox.Text = miCliente.Identidad;
            NombreClienteTextBox.Text = miCliente.Nombre;
        }

        private void Tickets_Load(object sender, EventArgs e)
        {
            UsuarioTextBox.Text = System.Threading.Thread.CurrentPrincipal.Identity.Name;
        }

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PrecioTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }


            if ((e.KeyChar == '.') && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }



            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(PrecioTextBox.Text))
            {

                DetalleTicket detalle = new DetalleTicket();
                detalle.TipoSoporte = TipoSoporteComboBox.Text;
                detalle.DescripcionSolicitud = DescripcionSolicitudTextBox.Text;
                detalle.DescripcionRespuesta = DescripcionRespuestaTextBox.Text;
                detalle.Precio = Convert.ToDecimal(PrecioTextBox.Text);
                detalle.Total = detalle.Precio;


                subTotal += detalle.Total;
                isv = subTotal * 0.15M;
                totalAPagar = subTotal + isv;

                listaDetalles.Add(detalle);

                DetalleDataGridView.DataSource = null;
                DetalleDataGridView.DataSource = listaDetalles;


                SubTotalTextBox.Text = subTotal.ToString("N2");
                ISVTextBox.Text = isv.ToString("N2");
                TotalTextBox.Text = totalAPagar.ToString("N2");


                TipoSoporteComboBox.Text = "";
                DescripcionSolicitudTextBox.Clear();
                DescripcionRespuestaTextBox.Clear();
                PrecioTextBox.Clear();

            }
        }
    }
}
