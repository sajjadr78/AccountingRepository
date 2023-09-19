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
    public partial class frmLogin : Form
    {
        public bool isEdit = false;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {

                if (BaseValidator.IsFormValid(this.components))
                {
                    if (isEdit == true)
                    {
                        var edit = db.LoginRepository.Get().First();
                        edit.UserName = txtUserName.Text;
                        edit.Password = txtPassword.Text;
                        db.LoginRepository.Update(edit);
                        db.Save();
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        if (db.LoginRepository.Get(l => l.Password == txtPassword.Text && l.UserName == txtUserName.Text).Any())
                        {
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("کاربری یافت نشد");
                        }
                    }
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (isEdit == true)
            {
                this.Text = "تنظیمات ورود";
                btnSignIn.Text = "ذخیره";
                using (UnitOfWork db = new UnitOfWork())
                {
                    var edit = db.LoginRepository.Get().First();
                    txtPassword.Text = edit.Password;
                    txtUserName.Text = edit.UserName;
                }
        }
    }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (BaseValidator.IsFormValid(this.components))
                {
                    if (isEdit == false)
                    {
                        if (db.LoginRepository.Get(l => l.Password == txtPassword.Text && l.UserName == txtUserName.Text).Any())
                        {
                            DialogResult = DialogResult.OK;
                        }
                    }
                }
            }
        }
    }
}
