using System;
using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Sealveru.Finances.Module.Controllers
{
    public partial class CustomViewCtrl<T> : ViewController
    {
        public CustomViewCtrl()
        {
            InitializeComponent();
            TargetObjectType = typeof(T);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            CurrentObjectChanged(null, null);
            View.CurrentObjectChanged += CurrentObjectChanged;
        }
        protected override void OnDeactivated()
        {
            View.CurrentObjectChanged -= CurrentObjectChanged;
            base.OnDeactivated();
        }


        protected UnitOfWork GetUnitOfWork()
        {
            return (UnitOfWork)((XPObjectSpace)ObjectSpace).Session;
        }
        protected void ExecuteAction(CallbackAction callback, bool isManaged, NotificationInfo notification)
        {
            if (callback == null)
                return;
            if (isManaged & _attempts == 0)
                ObjectSpace.CommitChanges();
            var rootObj = GetCurrentObject();

            try
            {
                callback.Invoke(rootObj);
                if (isManaged)
                    ObjectSpace.CommitChanges();
                ShowNotification(notification);
            }
            catch (UserFriendlyException ex) when (ex.Message.Contains("Please refresh data"))
            {
                Console.WriteLine(ex.GetType().ToString());
                if (isManaged)
                {
                    DoRollback();
                    _attempts += 1;
                    if (_attempts < 2)
                    {
                        ExecuteAction(callback, true, notification);
                        return;
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                if (isManaged)
                    DoRollback();
                if (!(ex is ValidationException))
                    throw;
            }
            finally
            {
                _attempts = 0;
            }
        }
        protected virtual void CurrentObjectChanged(object sender, EventArgs e) { }
        protected void PreFetchListView(params string[] propertyPath)
        {
            if (!(View is ListView listView)) return;

            var uow = GetUnitOfWork();
            var toPreFetch = (IEnumerable)listView.CollectionSource.Collection;
            var info = uow.GetClassInfo(View.ObjectTypeInfo.Type);
            if (toPreFetch != null)
                uow.PreFetch(info, toPreFetch, propertyPath);
        }
        protected T GetCurrentObject()
        {
            return CastObject<T>(View.CurrentObject);
        }
        protected T2 CastObject<T2>(object value)
        {
            if (value is XafDataViewRecord || value is XafInstantFeedbackRecord)
                return (T2)ObjectSpace.GetObject(value);
            return (T2)value;
        }
        protected void ShowNotification(NotificationInfo info)
        {
            if (info == null)
                return;

            var options = new MessageOptions
            {
                Duration = 2000,
                Message = info.Message,
                Type = info.Type,
                Win = {Caption = info.Caption, Type = WinMessageType.Alert}
            };
            Application.ShowViewStrategy.ShowMessage(options);
        }
        protected void DoRollback()
        {
            foreach (var i in ObjectSpace.ModifiedObjects)
            {
                if (ObjectSpace.IsNewObject(i))
                    ObjectSpace.Delete(i);
            }

            var uow = GetUnitOfWork();
            uow.ReloadChangedObjects();
            uow.RollbackTransaction();
            ObjectSpace.CommitChanges();
        }

        private int _attempts;
        protected delegate void CallbackAction(T obj);
    }
}
