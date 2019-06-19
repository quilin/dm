using DM.Web.Classic.Views.ModuleAttributeScheme;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.AttributeControllers
{
    public class EditAttributesSchemeController : DmControllerBase
    {
        private readonly IAttributeSpecificationMapper attributeSpecificationMapper;
        private readonly IAttributesService attributesService;

        public EditAttributesSchemeController(
            IAttributeSpecificationMapper attributeSpecificationMapper,
            IAttributesService attributesService)
        {
            this.attributeSpecificationMapper = attributeSpecificationMapper;
            this.attributesService = attributesService;
        }

        [HttpPost]
        public ActionResult Edit(CreateAttributeSchemeForm form)
        {
            var specifications = attributeSpecificationMapper.MapForEdit(form);
            attributesService.UpdateScheme(form.SchemeId, form.SchemeTitle, specifications);
            return new EmptyResult();
        }
    }
}