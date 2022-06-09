using System.ComponentModel.DataAnnotations;
namespace Projet_PFE.Models
{
    public class ChargeEncadrement
    {
        public long Id  { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public int? NbEncadrements { get; set; }
        public string CodeAnneeUniv  { get; set; }
        public long IdAUEnseignant { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire")]
        public long IdTypeEncadrement { get; set; }

        public TypeEncadrement TypeEncad { get; set; }
        public ChargeEncadrement()
        {}
    }
}