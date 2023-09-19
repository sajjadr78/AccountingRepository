using Accounting.App.Customers;
using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.App
{
    public partial class frmCustomers : Form
    {
        public frmCustomers()
        {
            InitializeComponent();
        }

        private void frmAddOrEdit_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        void BindGrid()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgCustomers.AutoGenerateColumns = false;
                dgCustomers.DataSource = db.CustomerRepository.GetAllCustomers();
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgCustomers.DataSource = db.CustomerRepository.GetCustomersByFilter(txtSearch.Text);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgCustomers.CurrentRow != null)
            {
                int customerId = int.Parse(dgCustomers.CurrentRow.Cells[0].Value.ToString());
                string name = dgCustomers.CurrentRow.Cells[1].Value.ToString();
                if (RtlMessageBox.Show($"آیا از حذف {name} مطمئن هستید؟", "سوال", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        frmAddOrEditCustomer frmAddOrEditCustomer = new frmAddOrEditCustomer();
                        string customerImage = db.CustomerRepository.GetCustomerById(customerId).CustomerImage;
                        bool isSuccess = db.CustomerRepository.DeleteCustomer(customerId);
                        if (isSuccess == true)
                        {
                            File.Delete(frmAddOrEditCustomer.path + customerImage);
                        }
                        if (isSuccess == false)
                        {
                            RtlMessageBox.Show("شما نمی توانید این شخص را حذف کنید زیرا برای او تراکنش ایجاد کرده اید!", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        BindGrid();
                    }
                }
            }
            
            else
            {
                ErrorSelectRow();
            }
        }

        private static void ErrorSelectRow()
        {
            RtlMessageBox.Show("لطفا یک شخص را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnNewCustomer_Click(object sender, EventArgs e)
        {
            frmAddOrEditCustomer frmAddOrEdit = new frmAddOrEditCustomer();
            if (frmAddOrEdit.ShowDialog() == DialogResult.OK)
            {
                BindGrid();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgCustomers.CurrentRow != null)
            {
                frmAddOrEditCustomer frmAddOrEditCustomer = new frmAddOrEditCustomer();
                int customerId = int.Parse(dgCustomers.CurrentRow.Cells[0].Value.ToString());
                frmAddOrEditCustomer.customerId = customerId;
                if (frmAddOrEditCustomer.ShowDialog() == DialogResult.OK)
                {
                    BindGrid();
                }
            }
            else
            {
                ErrorSelectRow();
            }
        }
    }
}