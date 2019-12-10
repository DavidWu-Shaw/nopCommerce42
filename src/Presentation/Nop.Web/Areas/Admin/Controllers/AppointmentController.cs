using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Self;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Models.Self;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class AppointmentController : BaseAdminController
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IAppointmentService _appointmentService;
        private readonly IWorkContext _workContext;

        #endregion Fields

        #region Ctor

        public AppointmentController(CatalogSettings catalogSettings,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IAppointmentService appointmentService,
            IWorkContext workContext)
        {
            _catalogSettings = catalogSettings;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _appointmentService = appointmentService;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        [HttpPost]
        public virtual IActionResult EventsByResource(DateTime start, DateTime end, int resource)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedDataTablesJson();

            var events = _appointmentService.GetAppointmentsByResource(start, end, resource);

            var model = new List<AppointmentModel>();
            foreach (var appointment in events)
            {
                var item = AppointmentModelFactory.ConvertToModel(appointment);
                model.Add(item);
            }

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult Edit(ProductReviewModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedView();
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedView();

            return RedirectToAction("List");
        }

        #endregion
    }
}