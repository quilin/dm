using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class GameNotepadController : DmControllerBase
    {
        private readonly IModuleService moduleService;
        private readonly IBbParserProvider bbParserProvider;

        public GameNotepadController(
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