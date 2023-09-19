using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Services;
using System.Windows.Forms;
using Accounting.DataLayer.Repositories;
using Accounting.DataLayer;
using Accounting.App.Accounting;
using Accounting.Utility.Converter;
using Accounting.ViewModels.Accounting;
using Accounting.Business;

namespace Accounting.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin frmLogin = new frmLogin();
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                this.Show();
                lblDate.Text = PersianDateTime.Now.ToString("yyyy/MM/dd");
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss.fff");
                Report();
            }
            else
            {
                Application.Exit();
            }
        }
        void Report()
        {
            ReportViewModel report = Account.ReportFormMain();
            lblReceive.Text = report.Receive.ToString("#,0") + "  تومان";
            lblPay.Text = report.Pay.ToString("#,0") + "  تومان";
            lblAccountingBalance.Text = report.AccountBalance.ToString("#,0") + "  تومان";
        }

        private void brnNewTransaction_Click(object sender, EventArgs e)
        {
            frmNewTransaction frmNewTransaction = new frmNewTransaction();
            frmNewTransaction.ShowDialog();
        }

        private void btnReportReceive_Click(object sender, EventArgs e)
        {
            frmReport frmReport = new frmReport();
            frmReport.TypeId = 1;
            frmReport.ShowDialog();
        }

        private void btnReportPay_Click(object sender, EventArgs e)
        {
            frmReport frmReport = new frmReport();
            frmReport.TypeId = 2;
            frmReport.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss.ff");
        }

        private void btnSigninSettings_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.isEdit = true;
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                Application.Restart();
            }
        }

        private void btnResfresh_Click(object sender, EventArgs e)
        {
            Report();
        }

        private void btnCustomers_Click_1(object sender, EventArgs e)
        {
            frmCustomers frmAddOrEdit = new frmCustomers();
            frmAddOrEdit.ShowDialog();
        }
    }
}
