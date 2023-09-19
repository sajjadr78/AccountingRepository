using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer.Context;
using Accounting.Utility.Converter;
using Accounting.ViewModels.Customers;

namespace Accounting.App.Accounting
{

    public partial class frmReport : Form
    {
        public int TypeId = 0;
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            if (TypeId == 1)
            {
                this.Text = "گزارش دریافتی ها";
            }
            if (TypeId == 2)
            {
                this.Text = "گزارش پرداختی ها";
            }
            using (UnitOfWork db = new UnitOfWork())
            {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(new ListCustomerViewModel()
                {
                    CustomerID = 0,
                    FullName = "لطفا انتخاب کنید"
                }
                );
                list.AddRange(db.CustomerRepository.GetNameCustomers());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerID";
            }
            Refresh();
        }

        void Filter()
        {
            dgvReport.Rows.Clear();
            dgvReport.AutoGenerateColumns = false;
            using (UnitOfWork db = new UnitOfWork())
            {
                DateTime? startDate;
                DateTime? endDate;
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();
                if ((int)cbCustomer.SelectedValue != 0)
                {
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeId && a.CustomerID == (int)cbCustomer.SelectedValue).ToList());
                }
                else
                {
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeId).ToList());
                }
                if (txtStartDate.Text != "    /  /")
                {
                    startDate = Convert.ToDateTime(txtStartDate.Text);
                    startDate = DateConverter.ToMiladi(startDate.Value);
                    result = result.Where(r => r.DateTime >= startDate.Value).ToList();
                }
                if (txtEndDate.Text != "    /  /")
                {
                    endDate = Convert.ToDateTime(txtEndDate.Text).AddDays(1);
                    endDate = DateConverter.ToMiladi(endDate.Value);
                    result = result.Where(r => r.DateTime <= endDate.Value).ToList();
                }
                foreach (DataLayer.Accounting item in result)
                {
                    string customerName = db.CustomerRepository.GetCustomerNameById(item.CustomerID);
                    string date = item.DateTime.ToShamsi().ToString("yyyy/MM/dd");
                    dgvReport.Rows.Add(item.ID, customerName, item.Amount, date, item.Description);
                }
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        private void Refresh()
        {
            cbCustomer.SelectedIndex = 0;
            txtEndDate.Text = "";
            txtStartDate.Text = "";
            Filter();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow != null)
            {
                if (RtlMessageBox.Show("آیا از حذف مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int Id = (int)dgvReport.CurrentRow.Cells[0].Value;
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.AccountingRepository.Delete(Id);
                        db.Save();
                        Refresh();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow != null)
            {
                int AccountID = (int)dgvReport.CurrentRow.Cells[0].Value;
                frmNewTransaction frmNewTransaction = new frmNewTransaction();
                frmNewTransaction.AccountID = AccountID;
                if (frmNewTransaction.ShowDialog() == DialogResult.OK)
                {
                    Refresh();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = new DataTable();
            dtPrint.Columns.Add("Customer");
            dtPrint.Columns.Add("Amount");
            dtPrint.Columns.Add("Date");
            dtPrint.Columns.Add("Description");
            foreach (DataGridViewRow item in dgvReport.Rows)
            {
                dtPrint.Rows.Add
                (
                item.Cells[1].Value.ToString(),
                item.Cells[2].Value.ToString(),
                item.Cells[3].Value.ToString(),
                item.Cells[4].Value.ToString()
                );
            }
            stiPrint.Load(Application.StartupPath + "/Report.mrt");
            stiPrint.RegData("DT", dtPrint);
            stiPrint.Show();
        }
    }
}
