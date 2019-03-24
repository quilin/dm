using System.ComponentModel;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters
{
    public enum Alignment
    {
        [Description("Законопослушный добрый")]
        LawfulGood = 0,
        [Description("Нейтральный добрый")] NeutralGood,
        [Description("Хаотичный добрый")] ChaoticGood,

        [Description("Законопослушный нейтральный")]
        LawfulNeutral,
        [Description("Нейтральный")] TrueNeutral,
        [Description("Хаотичный нейтральный")] ChaoticNeutral,
        [Description("Законопослушный злой")] LawfulEvil,
        [Description("Нейтральный злой")] NeutralEvil,
        [Description("Хаотичный злой")] ChaoticEvil
    }
}