using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_Filiere
    {
        public static void Add(Filiere Flre) =>  Dal_Filiere.Add(Flre);
           
        public static void Delete(long Id) => Dal_Filiere.Delete(Id);

        public static void Update(Filiere Flre) => Dal_Filiere.Update(Flre);
       
        public static List<Filiere> SelectAll() => Dal_Filiere.SelectAll();
      
        public static List<Filiere> SelectAll(string CodeAnneeUniv) => Dal_Filiere.SelectAll(CodeAnneeUniv);
       
        public static List<Filiere> SelectAll(
            string CodeTypeDiplome,
            string CodeAnneeUniv) => Dal_Filiere.SelectAll(CodeTypeDiplome,CodeAnneeUniv);
        
        public static List<Filiere> GetAll(
            string CodeTypeDiplome,
            string CodeAnneeUniv) => Dal_Filiere.GetAll(CodeTypeDiplome, CodeAnneeUniv);
        
        public static List<Filiere> SelectAll(long TeacherId) => Dal_Filiere.SelectAll(TeacherId);
        public static Filiere SelectTheLast() => Dal_Filiere.SelectTheLast();

        public static Filiere SelectById(long Id) => Dal_Filiere.SelectById(Id);
       public static bool IsForeignKeyInTable(
           string TableName,
           long Id) => Dal_Filiere.IsForeignKeyInTable(TableName, Id);
    }
} 