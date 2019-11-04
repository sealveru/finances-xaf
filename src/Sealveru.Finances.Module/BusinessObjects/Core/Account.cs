using System;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Sealveru.Finances.Module
{
    public class Account : BaseAccount, ITreeNode
    {
        public Account(Session session) : base(session) { }


        public void ChangeTotal(double value, KindCharacter direction)
        {
            var variation = value * (direction == Character ? 1 : -1);
            if (Total + variation < 0)
                throw new NegativeAccountException(Name);
            Total += variation;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (IsLoading) return;

            switch (propertyName)
            {
                case nameof(Category):
                    KindAccountChanged();
                    break;
                case nameof(Parent):
                    SetKind();
                    SetCode();
                    break;
            }
        }
        private void KindAccountChanged()
        {
            if (Category != null)
                Character = Category.Character;

            if (!SelectingKindAccount)
            {
                Parent = null;
                SetCode();
            }
        }
        private void SetCode()
        {
            var codeParent = Parent is null ? string.Empty : Parent.Code;
            var codeKind = Category is null ? string.Empty : Category.Code;
            Code = codeParent == string.Empty ? codeKind : codeParent;
        }
        private void SetKind()
        {
            if (!(Category is null & Parent != null)) return;

            SelectingKindAccount = true;
            Category = Parent.Category;
            SelectingKindAccount = false;
        }


        private double _total;
        [ModelDefault("DisplayFormat", BaseVariables.FormatDoubleDisplay)]
        [ModelDefault("EditMask", BaseVariables.FormatDoubleEditMask)]
        [ModelDefault("AllowEdit", "False")]
        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        public double Total {
            get => _total;
            set => SetPropertyValue(nameof(Total), ref _total, value);
        }

        private CategoryAccount _category;
        [RuleRequiredField(DefaultContexts.Save)]
        [Association]
        public CategoryAccount Category {
            get => _category;
            set => SetPropertyValue(nameof(Category), ref _category, value);
        }

        private Account _parent;
        [Association]
        [DataSourceProperty("Category.Accounts")]
        [DataSourceCriteria("Oid != '@This.Oid'")]
        public Account Parent {
            get => _parent;
            set => SetPropertyValue(nameof(Parent), ref _parent, value);
        }

        [PersistentAlias("[Total] + IsNull([SubAccounts].Sum([FullTotal]), 0)")]
        public double FullTotal
            => Convert.ToDouble(EvaluateAlias(nameof(FullTotal)));

        [Association, Aggregated]
        public XPCollection<Account> SubAccounts
            => GetCollection<Account>(nameof(SubAccounts));

        protected bool SelectingKindAccount;

        #region TreeNode

        string ITreeNode.Name => Name;
        ITreeNode ITreeNode.Parent => Parent;
        System.ComponentModel.IBindingList ITreeNode.Children => SubAccounts;

        #endregion

    }
}
