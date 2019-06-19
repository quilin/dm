using DM.Web.Classic.Dto;
using DM.Web.Classic.Views.Dice;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.PostControllers
{
    public class DiceController : DmControllerBase
    {
        private readonly IDiceService diceService;
        private readonly ICreateDieResultModelConverter createDieResultModelConverter;
        private readonly IDieResultViewModelBuilder dieResultViewModelBuilder;

        public DiceController(
            IDiceService diceService,
            ICreateDieResultModelConverter createDieResultModelConverter,
            IDieResultViewModelBuilder dieResultViewModelBuilder
            )
        {
            this.diceService = diceService;
            this.createDieResultModelConverter = createDieResultModelConverter;
            this.dieResultViewModelBuilder = dieResultViewModelBuilder;
        }

        [HttpPost, ValidationRequired]
        public ActionResult Roll(DiceForm form)
        {
            var createDieResultModel = createDieResultModelConverter.Convert(form);
            var dieResult = diceService.Create(createDieResultModel);
            var dieResultViewModel = dieResultViewModelBuilder.Build(dieResult);

            return PartialView("ResultPreview", dieResultViewModel);
        }
    }
}