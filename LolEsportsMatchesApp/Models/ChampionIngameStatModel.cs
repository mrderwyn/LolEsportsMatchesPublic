using System.ComponentModel.DataAnnotations;

namespace LolEsportsMatchesApp.Models
{
    public class ChampionIngameStatModel
    {
        public int Level { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public int Assists { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,k}")]
        public int Gold { get; set; }

        public string[] ItemsImage { get; set; } = null!;

        public string[] ItemsName { get; set; } = null!;

        public string Abilities { get; set; } = null!;

        public (string Name, string Image) MainPerk { get; set; }

        public (string Name, string Image) SubPerk { get; set; }
    }
}
