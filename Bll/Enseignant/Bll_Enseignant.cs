using System;
using System.Collections.Generic;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_Enseignant
    {
        public static void Add(Enseignant Enseignant) =>  Dal_Enseignant.Add(Enseignant);
           
        public static void Delete(long id) => Dal_Enseignant.Delete(id);
            
        public static void Update(
            string OldCIN,
            Enseignant enseignant) => Dal_Enseignant.Update(OldCIN,enseignant);
            
        public static List<Enseignant> SelectAll() => Dal_Enseignant.SelectAll();
        public static List<Enseignant> SelectAll(long ids) => Dal_Enseignant.SelectAll(ids);
        public static List<Enseignant> SelectAll(
            string FieldName,
            string FielValue,
            string CodeAnneeUniv) => Dal_Enseignant.SelectAll(FieldName, FielValue, CodeAnneeUniv);

        public static List<Enseignant> SelectAll(
            string FieldName,
            string FielValue,
            long ids) => Dal_Enseignant.SelectAll(FieldName, FielValue, ids);

        public static Enseignant SelectById(long Id) => Dal_Enseignant.SelectById(Id);
        public static Enseignant SelectByCIN(string CIN) => Dal_Enseignant.SelectByCIN(CIN);
        public static bool HasRowInTableInAnneeUnivEnseignant(string CIN) => Dal_Enseignant.HasRowInTableInAnneeUnivEnseignant(CIN); 
    }
}