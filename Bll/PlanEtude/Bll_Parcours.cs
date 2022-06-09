using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_Parcours
    {
        public static void Add(Parcours Pars) => Dal_Parcours.Add(Pars);
            
        public static void Delete(long Id) => Dal_Parcours.Delete(Id);

        public static void Update(
            Parcours Pars) => Dal_Parcours.Update(Pars);
            
        public static List<Parcours> SelectAll() => Dal_Parcours.SelectAll();  

        public static List<Parcours> SelectFiliereParcours(long IdFiliere) => Dal_Parcours.SelectFiliereParcours(IdFiliere);

        public static List<Parcours> SelectAll(
            string CodeFiliere,
            string CodeAnneeUniv) => Dal_Parcours.SelectAll(CodeFiliere, CodeAnneeUniv);

        public static List<Parcours> GetAll(
            long IdFiliere,
            string CodeAnneeUniv) => Dal_Parcours.GetAll(IdFiliere, CodeAnneeUniv);

        public static Parcours SelectById(long Id) => Dal_Parcours.SelectById(Id);
        public static Parcours SelectTheLast() => Dal_Parcours.SelectTheLast();
        public static bool IsForeignKeyInTable(
            string TableName,
            long Id) => Dal_Parcours.IsForeignKeyInTable(TableName, Id);
    }
}