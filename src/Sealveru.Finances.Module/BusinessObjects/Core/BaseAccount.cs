using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace Sealveru.Finances.Module
{
    [NonPersistent]
    public abstract class BaseAccount : Root
    {
        protected BaseAccount(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            CreatedDate = DateTime.Now;
        }


        private string _code;
        [Indexed(Unique = true)]
        public string Code {
            get => _code;
            set => SetPropertyValue<string>(nameof(Code), ref _code, value);
        }

        private string _name;
        [RuleRequiredField(DefaultContexts.Save)]
        public string Name {
            get => _name;
            set => SetPropertyValue<string>(nameof(Name), ref _name, value);
        }

        private string _description;
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get => _description;
            set => SetPropertyValue<string>(nameof(Description), ref _description, value);
        }

        private DateTime _createdDate;
        [ModelDefault("DisplayFormat", BaseVariables.FormatLongDateDisplay)]
        [ModelDefault("EditMask", BaseVariables.FormatLongDateEditMask)]
        public DateTime CreatedDate {
            get => _createdDate;
            set => SetPropertyValue<DateTime>(nameof(CreatedDate), ref _createdDate, value);
        }

        private KindCharacter _character;
        [RuleRequiredField(DefaultContexts.Save)]
        public KindCharacter Character {
            get => _character;
            set => SetPropertyValue(nameof(Character), ref _character, value);
        }

    }
}
