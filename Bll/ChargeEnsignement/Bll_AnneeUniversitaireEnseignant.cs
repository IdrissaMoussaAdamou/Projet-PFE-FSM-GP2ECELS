using System.Collections.Generic;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_AnneeUniversitaireEnseignant
    {
        public static void Add(AnneeUniversitaireEnseignant AnneeUnivE) =>  Dal_AnneeUniversitaireEnseignant.Add(AnneeUnivE);
           
        public static void Delete(long Id) => Dal_AnneeUniversitaireEnseignant.Delete(Id);
            
        public static void Update(AnneeUniversitaireEnseignant AnneeUnivE) => Dal_AnneeUniversitaireEnseignant.Update(AnneeUnivE);
            
        public static List<AnneeUniversitaireEnseignant> SelectAll(string CodeAnneeUniv) => Dal_AnneeUniversitaireEnseignant.SelectAll(CodeAnneeUniv);
        public static List<AnneeUniversitaireEnseignant> SelectAll(
            string FieldName,
            string FielValue,
            string CodeAnneeUniv) => Dal_AnneeUniversitaireEnseignant.SelectAll(FieldName, FielValue, CodeAnneeUniv);
        public static List<AnneeUniversitaireEnseignant> SelectAll(
            List<string> ListCIN,
            string CodeAnneeUniv) => Dal_AnneeUniversitaireEnseignant.SelectAll(ListCIN, CodeAnneeUniv);

        public static AnneeUniversitaireEnseignant SelectById(long Id) => Dal_AnneeUniversitaireEnseignant.SelectById(Id);
        public static AnneeUniversitaireEnseignant SelectByCIN(string CIN) => Dal_AnneeUniversitaireEnseignant.SelectByCIN(CIN);
        public static AnneeUniversitaireEnseignant SelectByAUCIN(
            string CodeAnneeUniv,
            string CIN) => Dal_AnneeUniversitaireEnseignant.SelectByAUCIN(CodeAnneeUniv, CIN);
        public static AnneeUniversitaireEnseignant SelectTheLast() => Dal_AnneeUniversitaireEnseignant.SelectTheLast();

        public static bool IsForeignKeyInTable(string TableName, long Id) => Dal_AnneeUniversitaireEnseignant.IsForeignKeyInTable(TableName, Id);      
        public static void  LockOrUnLoCkCharge(
            long IdTeacher,
            string KeyWord) => Dal_AnneeUniversitaireEnseignant.LockOrUnLockCharge(IdTeacher, KeyWord);
        public static void ChargeVerified(long IdTeacher) => Dal_AnneeUniversitaireEnseignant.ChargeVerified(IdTeacher);
        public static void  ValidateOrUnValidate(
            long IdTeacher,
            string LieuDEValidation,
            string KeyWord) => Dal_AnneeUniversitaireEnseignant.ValidateOrUnValidate(IdTeacher, LieuDEValidation, KeyWord);
    }
}