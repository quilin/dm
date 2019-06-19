using System;
using System.Net;
using DM.Web.Classic.Dto;
using DM.Web.Classic.Views.Shared.Warnings.CreateWarning;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.WarningsControllers
{
    public class CreateWarningController : DmControllerBase
    {
        private readonly ICreateWarningFormBuilder formBuilder;
        private readonly IWarningService warningService;
        private readonly ICreateWarningModelConverter modelConverter;


        public CreateWarningController(
            ICreateWarningFormBuilder formBuilder,
            IWarningService warningService,
            ICreateWarningModelConverter modelConverter)
        {
            this.formBuilder = formBuilder;
            this.warningService = warningService;
            this.modelConverter = modelConverter;
        }

        [HttpGet]
        public IActionResult Create(string userName, Guid? entityId)
        {
            var createWarningForm = formBuilder.Build(userName, entityId);
            return PartialView("~/Views/Shared/Warnings/CreateWarning/CreateWarning.cshtml", createWarningForm);
        }

        [HttpPost, ValidationRequired]
        public IActionResult Create(CreateWarningForm createWarningForm)
        {
            var createModel = modelConverter.Convert(createWarningForm);
            try
            {
                warningService.Create(createModel);
                return null;
            }
            catch (WarningPointsException warningPointsException)
            {
                Console.WriteLine(warningPointsException);
                return AjaxError(HttpStatusCode.BadRequest, warningPointsException.Message);
            }
        }
    }
}