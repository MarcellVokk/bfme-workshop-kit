using BfmeWorkshopKit.Data;
using BfmeWorkshopKit.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkshopEditor
{
    public partial class ArenaLoginForm : Form
    {
        public ArenaLoginForm()
        {
            InitializeComponent();
        }

        public BfmeWorkshopAuthInfo AuthInfo { get; private set; }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                AuthInfo = await BfmeWorkshopAuthManager.Authenticate(txtEmail.Text, txtPassword.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
