using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;

namespace Accounting.App
{
    public partial class frmNewTransaction : Form
    {
        private UnitOfWork db;
        public int AccountID = 0;
        public frmNewTransaction()
        {
            InitializeComponent();
        }
        private void frmNewTransaction_Load(object sender, EventArgs e)
        {
            db = new UnitOfWork();
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers();
            if (AccountID != 0)
            {
                var account = db.AccountingRepository.GetById(AccountID);
                txtAmount.Value = (int)account.Amount;
                txtDescription.Text = account.Description;
                txtName.Text = db.CustomerRepository.GetCustomerNameById(account.CustomerID);
                if (account.TypeID == 1)
                {
                    rbReceive.Checked = true;
                }
                if (account.TypeID == 2)
                {
                    rbPay.Checked = true;
                }
                this.Text = "ویرایش";
            }
            db.Dispose();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers(txtFilter.Text);
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                if (rbReceive.Checked || rbPay.Checked)
                {
                    db = new UnitOfWork();
                    DataLayer.Accounting accounting = new DataLayer.Accounting()
                    {
                        Amount = (int)txtAmount.Value,
                        CustomerID = db.CustomerRepository.GetCustomerIdByName(txtName.Text),
                        DateTime = DateTime.Now,
                        TypeID = (rbReceive.Checked) ? 1 : 2,
                        Description = txtDescription.Text,
                    };
                    bool isSuccess;
                    if (AccountID == 0)
                    {
                        isSuccess = db.AccountingRepository.Insert(accounting);
                    }
                    else
                    {
                        accounting.ID = AccountID;
                        isSuccess = db.AccountingRepository.Update(accounting);
                    }
                    if (isSuccess == true)
                    {
                        if (AccountID == 0)
                        {
                            RtlMessageBox.Show("افزودن با موفقیت انجام شد", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            RtlMessageBox.Show("ویرایش با موفقیت انجام شد", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (AccountID == 0)
                        {
                            RtlMessageBox.Show("افزودن با شکست مواجه شد", "شکست", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            RtlMessageBox.Show("ویرایش با شکست مواجه شد", "شکست", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    db.Save();
                    DialogResult = DialogResult.OK;
                    db.Dispose();
                }
                else
                {
                    RtlMessageBox.Show("لطفا نوع تراکنش را مشخص کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

       
    }
}
