using System.Collections;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;

namespace Sealveru.Finances.Module.Controllers
{
    public partial class AccountCtrl : CustomViewCtrl<Account>
    {
        public AccountCtrl()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            PreFetchListView("SubAccounts");
        }
    }
}
