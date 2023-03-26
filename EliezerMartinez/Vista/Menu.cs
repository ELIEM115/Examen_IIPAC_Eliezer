using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void TicketToolStripButton1_Click(object sender, EventArgs e)
        {
            Tickets tickets = new Tickets();
            tickets.MdiParent = this;
            tickets.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            UsuariosForm userForm = new UsuariosForm();
            userForm.MdiParent = this;
            userForm.Show();
        }
    }
}
