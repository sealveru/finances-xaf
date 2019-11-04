using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Sealveru.Finances.Module
{
    [NonPersistent]
    [DefaultClassOptions()]
    [DefaultListViewOptions(false, NewItemRowPosition.Top)]
    [ModelDefault("DefaultListViewShowAutoFilterRow", "True")]
    [ModelDefault("IsVisibleInDashboards", "True")]
    [ModelDefault("IsVisibleInReports", "True")]
    public abstract class Root : BaseObject
    {
        protected Root(Session session) : base(session) { }

        protected UnitOfWork LocalUnitOfWork => (UnitOfWork)Session;
        protected virtual string VisibleName => ToString();

    }
}