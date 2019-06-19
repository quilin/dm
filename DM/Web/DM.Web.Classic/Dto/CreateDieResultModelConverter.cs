using DM.Web.Classic.Views.Dice;

namespace DM.Web.Classic.Dto
{
    public class CreateDieResultModelConverter : ICreateDieResultModelConverter
    {
        public CreateDieResultModel Convert(DiceForm form)
        {
            return new CreateDieResultModel
            {
                IsHidden = form.IsHidden,
                IsFair = form.IsFair,

                ThrowsCount = form.RollsCount,
                EdgesCount = form.EdgesCount,
                BlastCount = form.BlastCount,
                Bonus = form.Bonus ?? 0,

                Commentary = form.Commentary
            };
        }
    }
}