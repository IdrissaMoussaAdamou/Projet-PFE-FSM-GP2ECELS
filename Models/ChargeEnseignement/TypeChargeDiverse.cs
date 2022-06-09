using System.ComponentModel.DataAnnotations;
namespace Projet_PFE.Models
{
    public class TypeChargeDiverse
    {
        public long Id  { get; set; }
        [Required(ErrorMessage = "Champs Obligatoire non Saisi")]
        public string Libelle  { get; set; }
        public TypeChargeDiverse()
        {}
    }
}