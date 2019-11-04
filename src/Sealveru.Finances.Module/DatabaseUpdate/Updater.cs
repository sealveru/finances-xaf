using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace Sealveru.Finances.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDbVersion) :
            base(objectSpace, currentDbVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            var adminRole = CreateAdminRole();
            CreateUser(adminRole);
            CreateCategoriesAccounts();

            ObjectSpace.CommitChanges();
        }

        private PermissionPolicyRole CreateAdminRole()
        {
            const string name = "Administrators";
            var filter = new BinaryOperator("Name", name);
            var adminRole = ObjectSpace.FindObject<PermissionPolicyRole>(filter);

            if (adminRole is null)
            {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = name;
            }
            adminRole.IsAdministrative = true;

            return adminRole;
        }
        private PermissionPolicyUser CreateUser(PermissionPolicyRole role)
        {
            const string name = "Admin";
            var filter = new BinaryOperator("UserName", name);
            var userAdmin = ObjectSpace.FindObject<PermissionPolicyUser>(filter);

            if (userAdmin is null)
            {
                userAdmin = ObjectSpace.CreateObject<PermissionPolicyUser>();
                userAdmin.UserName = name;
                userAdmin.SetPassword(name);
                userAdmin.Roles.Add(role);
            }

            return userAdmin;
        }

        private void CreateCategoriesAccounts()
        {
            CreateCategoryAccount("Asset", KindAccount.Real, KindCharacter.Deudor, 1.ToString());
            CreateCategoryAccount("Liability", KindAccount.Real, KindCharacter.Acreedor, 2.ToString());
            CreateCategoryAccount("Capital", KindAccount.Real, KindCharacter.Acreedor, 3.ToString());
            CreateCategoryAccount("Revenue", KindAccount.Nominal, KindCharacter.Acreedor, 4.ToString());
            CreateCategoryAccount("Expense", KindAccount.Nominal, KindCharacter.Deudor, 5.ToString());
        }

        private void CreateCategoryAccount(string name, KindAccount kind, KindCharacter character, string code)
        {
            var filter = new BinaryOperator("Name", name);
            var category = ObjectSpace.FindObject<CategoryAccount>(filter);
            if (category is null)
            {
                category = ObjectSpace.CreateObject<CategoryAccount>();
                category.Name = name;
                category.KindAccount = kind;
                category.Character = character;
                category.Code = code;
            }
        }

    }
}
