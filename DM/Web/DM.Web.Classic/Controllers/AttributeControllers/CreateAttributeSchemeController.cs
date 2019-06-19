using DM.Web.Classic.Views.ModuleAttributeScheme;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.AttributeControllers
{
    public class CreateAttributeSchemeController : DmControllerBase
    {
        private readonly IAttributeSpecificationMapper attributeSpecificationMapper;
        private readonly IAttributesService attributesService;
        private readonly IModuleService moduleService;

        public CreateAttributeSchemeController(
            IAttributeSpecificationMapper attributeSpecificationMapper,
            IAttributesService attributesService,
            IModuleService moduleService)
        {
            this.attributeSpecificationMapper = attributeSpecificationMapper;
            this.attributesService = attributesService;
            this.moduleService = moduleService;
        }

        [HttpGet]
        public ActionResult GetCreateForm()
        {
            return View("~/Views/ModuleAttributeScheme/CreateScheme.cshtml", CreateAttributeSchemeForm.Default);
        }

        [HttpPost]
        public ActionResult Create(CreateAttributeSchemeForm form)
        {
            var specifications = attributeSpecificationMapper.MapForCreate(form);
            var attributeScheme = attributesService.CreateScheme(form.SchemeTitle, specifications);
            
            return Json(new {value = attributeScheme.Id, text = form.SchemeTitle, order = -1});
        }

        [HttpPost]
        public void CreateAndApply(CreateAttributeSchemeForm form)
        {
            var specifications = attributeSpecificationMapper.MapForCreate(form);
            var attributeScheme = attributesService.CreateScheme(form.SchemeTitle, specifications);
            moduleService.UpdateScheme(form.ModuleId, attributeScheme.Id);
        }
    }
}