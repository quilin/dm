using DM.Web.Classic.Views.Dice;

namespace DM.Web.Classic.Dto
{
    public interface ICreateDieResultModelConverter
    {
        CreateDieResultModel Convert(DiceForm form);
    }
}