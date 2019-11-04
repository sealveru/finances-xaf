using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Sealveru.Finances.Module
{
    public class CategoryAccount : BaseAccount
    {
        public CategoryAccount(Session session) : base(session) { }


        private KindAccount _kindAccount;
        [RuleRequiredField(DefaultContexts.Save)]
        public KindAccount KindAccount {
            get => _kindAccount;
            set => SetPropertyValue(nameof(KindAccount), ref _kindAccount, value);
        }

        [Association]
        public XPCollection<Account> Accounts 
            => GetCollection<Account>(nameof(Accounts));

    }
}
