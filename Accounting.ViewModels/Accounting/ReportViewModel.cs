using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.ViewModels.Accounting
{
    public class ReportViewModel
    {

        public float Receive { get; set; }
        public float Pay { get; set; }
        public float AccountBalance { get; set; }

        #region Date
        public PersianDateTime ShamsiDate { get; set; }
        public int TypeID { get; set; }
        public double Amount { get; set; }

        #endregion
    }
}
