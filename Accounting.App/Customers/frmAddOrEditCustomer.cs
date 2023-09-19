using Accounting.DataLayer.Context;
using Accounting.DataLayer;
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
using System.IO;

namespace Accounting.App.Customers
{
    public partial class frmAddOrEditCustomer : Form
    {

        public string path = Directory.GetParent(Application.StartupPath).Parent.FullName + @"\Images\";
        public int customerId = 0;
        UnitOfWork db = new UnitOfWork();
        public frmAddOrEditCustomer()
        {
            InitializeComponent();
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pcCustomer.ImageLocation = openFile.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(pcCustomer.ImageLocation);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                pcCustomer.Image.Save(path + imageName);
                DataLayer.Customers customer = new DataLayer.Customers()
                {
                    FullName = txtName.Text,
                    Mobile = txtMobile.Text,
                    Email = txtEmail.Text,
                    Address = txtAddress.Text,
                    CustomerImage = imageName
                };
                if (customerId != 0)
                {
                    customer.CustomerID = customerId;
                    bool isSuccess = db.CustomerRepository.UpdateCustomer(customer);
                    if (isSuccess == true)
                    {
                        RtlMessageBox.Show("ویرایش با موفقیت انجام شد", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        RtlMessageBox.Show("ویرایش با شکست مواجه شد", "شکست", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    db.CustomerRepository.InsertCustomer(customer);
                }
                db.Save();
                DialogResult = DialogResult.OK;
            }
        }

        private void frmAddOrEditCustomer_Load(object sender, EventArgs e)
        {
            if (customerId != 0)
            {

                Text = "ویرایش شخص";
                btnSave.Text = "ویرایش";
                var customer = db.CustomerRepository.GetCustomerById(customerId);
                if (customer!=null)
                {
                    txtName.Text = customer.FullName;
                    txtMobile.Text = customer.Mobile;
                    txtEmail.Text = customer.Email;
                    txtAddress.Text = customer.Address;
                    pcCustomer.ImageLocation = path + customer.CustomerImage;
                }
                else
                {
                    RtlMessageBox.Show("شخصی با این مشخصات یافت نشد احتمالا بانک اطلاعاتی شما هک شده است!", "احتمال هک", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }
    }
}
