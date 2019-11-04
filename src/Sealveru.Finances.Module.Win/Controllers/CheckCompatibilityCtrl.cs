using DevExpress.ExpressApp.Win.SystemModule;

namespace Sealveru.Finances.Module.Win.Controllers
{
    public class CheckCompatibilityCtrl : VersionsCompatibilityController
    {
        public CheckCompatibilityCtrl()
        {
            Active["UpdateAllowed"] = false;
        }
    }
}
