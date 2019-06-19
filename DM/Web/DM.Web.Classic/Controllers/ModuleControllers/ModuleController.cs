using System;
using DM.Web.Classic.Views.ModuleInfo;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ModuleControllers
{
    public class ModuleController : DmControllerBase
    {
        private readonly IModuleViewModelBuilder moduleViewModelBuilder;
        private readonly IReaderService readerService;

        public ModuleController(
            IModuleViewModelBuilder moduleViewModelBuilder,
            IReaderService readerService
            )
        {
            this.moduleViewModelBuilder = moduleViewModelBuilder;
            this.readerService = readerService;
        }

        public IActionResult Index(Guid moduleId)
        {
            var moduleViewModel = moduleViewModelBuilder.Build(moduleId);
            return View("~/Views/ModuleInfo/Module.cshtml", moduleViewModel);
        }

        public IActionResult Observe(Guid moduleId)
        {
            readerService.Observe(moduleId);
            return RedirectToAction(Request.GetDisplayUrl());
        }

        public IActionResult StopObserving(Guid moduleId)
        {
            readerService.StopObserving(moduleId);
            return Redirect(Request.GetDisplayUrl());
        }
    }
}