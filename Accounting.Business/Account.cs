using Accounting.ViewModels.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Context;
using Accounting.Utility.Converter;

namespace Accounting.Business
{
    public class Account
    {
        public static ReportViewModel ReportFormMain()
        {
            ReportViewModel rp = new ReportViewModel();
            using (UnitOfWork db = new UnitOfWork())
            {
                PersianDateTime now = PersianDateTime.Now;
                PersianDateTime startdate = new PersianDateTime(now.FirstDayOfMonth.ToDateTime());
                PersianDateTime enddate = new PersianDateTime(now.LastDayOfMonth.ToDateTime()).AddDays(1);
                var list = db.AccountingRepository.Get().Select(a => new ReportViewModel
                {
                    ShamsiDate = a.DateTime.ToShamsi(),
                    TypeID = a.TypeID,
                    Amount = a.Amount
                }).ToList();
                var receive = list.Where(a => a.TypeID == 1 && a.ShamsiDate >= startdate && a.ShamsiDate <= enddate).Select(a => a.Amount).ToList();
                var pay = list.Where(a => a.TypeID == 2 && a.ShamsiDate >= startdate && a.ShamsiDate <= enddate).Select(a => a.Amount).ToList();
                rp.Receive = (float)receive.Sum();
                rp.Pay = (float)pay.Sum();
                rp.AccountBalance = rp.Receive - rp.Pay;
                return rp;
            }
        }
    }
}
