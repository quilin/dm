using DM.Web.Classic.Views.ModuleNotepad;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ModuleControllers
{
    public class ModuleNotepadController : DmControllerBase
    {
        private readonly IModuleService moduleService;
        private readonly IBbParserProvider bbParserProvider;

        public ModuleNotepadController(
            IModuleService moduleService,
            IBbParserProvider bbParserProvider)
        {
            this.moduleService = moduleService;
            this.bbParserProvider = bbParserProvider;
        }

        [HttpPost]
        public ActionResult Edit(ModuleNotepadForm form)
        {
            var module = moduleService.UpdateNotepad(form.ModuleId, form.Notepad);
            return Content(bbParserProvider.CurrentInfo.Parse(module.Notepad).ToHtml());
        }
    }
}