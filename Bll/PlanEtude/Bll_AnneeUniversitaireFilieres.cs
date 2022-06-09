using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_AnneeUniversitaireFilieres
    {
        public static void Add(
            long IdFiliere,
            string CodeAnneeUniv) =>  Dal_AnneeUniversitaireFilieres.Add(IdFiliere, CodeAnneeUniv);
        
        public static void Delete(
            long IdFiliere,
            string CodeAnneeUniv) => Dal_AnneeUniversitaireFilieres.Delete(IdFiliere, CodeAnneeUniv);
             
    }
}