using System.ComponentModel.DataAnnotations;

namespace Projet_PFE.Models
{
    public class NomOption
    {
        public long Id { get; set; }
        public long IdModule { get; set; }
        public string ModuleIntuleFr { get; set; }
        public string CodeAnneeUniv { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Intitule { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]

        public string IntituleAbrg { get; set; }

        public NomOption()
        {

        }
        public NomOption(long pIdModule, string pCodeAnneeUniv, string pIntitule, string pIntituleAbrg)
        {
            this.IdModule = pIdModule;
            this.CodeAnneeUniv = pCodeAnneeUniv;
            this.Intitule = pIntitule;
            this.IntituleAbrg = pIntituleAbrg;
        }
    }
}
