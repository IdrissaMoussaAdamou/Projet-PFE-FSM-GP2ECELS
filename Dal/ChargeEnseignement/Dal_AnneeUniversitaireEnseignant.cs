using System;
using System.Collections.Generic;
using Projet_PFE.MyUtilities;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;

namespace Projet_PFE.Dal
{
    public class Dal_AnneeUniversitaireEnseignant
    {
        private static AnneeUniversitaireEnseignant GetEntityFromDataRow(DataRow dr)
        {
            AnneeUniversitaireEnseignant AnneUnivE = new AnneeUniversitaireEnseignant();
            AnneUnivE.Id = (Int64)dr["Id"];
            AnneUnivE.CIN = (string)dr["CIN"];
            AnneUnivE.Nom = (string)dr["Nom"];
            AnneUnivE.Prenom = (string)dr["Prenom"];
            AnneUnivE.Grade = (string)(dr["Grade"]);
            AnneUnivE.Statut = (string)(dr["Statut"]);
            AnneUnivE.ValidationChargeDepartement = (string)dr["ValidationChargeDepartement"] ;
            AnneUnivE.DateValidationChargeDepartement = dr["DateValidationChargeDepartement"] == System.DBNull.Value?   (DateTime?)null : (DateTime)dr["DateValidationChargeDepartement"];
            AnneUnivE.ValidationChargeAdministration = (string)dr["ValidationChargeAdministration"];
            AnneUnivE.DateValidationChargeAdminsitration = dr["DateValidationChargeAdminsitration"] == System.DBNull.Value? (DateTime?)null : (DateTime)dr["DateValidationChargeAdminsitration"];
            AnneUnivE.EtatSaisie = (string)dr["EtatSaisie"];

            AnneUnivE.CodeDepartement = (string)dr["CodeDepartement"];
            AnneUnivE.IntituleFrDepartement = Dal_Departement.SelectByCode(AnneUnivE.CodeDepartement).IntituleFr;
            AnneUnivE.CodeAnneeUniv = (string)dr["CodeAnneeUniv"];

            return AnneUnivE;
        }

        private static List<AnneeUniversitaireEnseignant> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<AnneeUniversitaireEnseignant> L = new List<AnneeUniversitaireEnseignant>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_AnneeUniversitaireEnseignant.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(AnneeUniversitaireEnseignant AnneUnivE)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [AnneeUniversitaireEnseignant](CIN, Nom, Prenom, Grade, Statut, CodeDepartement, ";
                Command.CommandText += "CodeAnneeUniv) values(@CIN, @Nom, @Prenom, @Grade, @Statut, @CodeDepartement, @CodeAnneeUniv)";
                
                Command.Parameters.AddWithValue("@CIN", AnneUnivE.CIN);
                Command.Parameters.AddWithValue("@Nom", AnneUnivE.Nom);
                Command.Parameters.AddWithValue("@Prenom", AnneUnivE.Prenom);
                Command.Parameters.AddWithValue("@Grade", AnneUnivE.Grade);
                Command.Parameters.AddWithValue("@Statut", AnneUnivE.Statut);
                Command.Parameters.AddWithValue("@CodeDepartement", AnneUnivE.CodeDepartement);
                Command.Parameters.AddWithValue("@CodeAnneeUniv", AnneUnivE.CodeAnneeUniv);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            
            }
        }

        public static  void Delete(long Id)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
             {
                string StrRequest = "DELETE FROM [AnneeUniversitaireEnseignant] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest ,Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update(AnneeUniversitaireEnseignant AnneUnivE)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
               
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE [AnneeUniversitaireEnseignant] SET  [Nom] = @Nom, [Prenom] = @Prenom, [Grade] = @Grade, [Statut] = @Statut, ";
                Command.CommandText += "[CodeDepartement] = @CodeDepartement WHERE [Id] = @Id";
                
                Command.Parameters.AddWithValue("@Nom", AnneUnivE.Nom);
                Command.Parameters.AddWithValue("@Prenom", AnneUnivE.Prenom);
                Command.Parameters.AddWithValue("@Grade", AnneUnivE.Grade);
                Command.Parameters.AddWithValue("@Statut", AnneUnivE.Statut);
                Command.Parameters.AddWithValue("@CodeDepartement", AnneUnivE.CodeDepartement);
                Command.Parameters.AddWithValue("@CodeAnneeUniv", AnneUnivE.CodeAnneeUniv);
                Command.Parameters.AddWithValue("@Id", AnneUnivE.Id);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static List<AnneeUniversitaireEnseignant> SelectAll(SqlCommand Command)
        {
            List<AnneeUniversitaireEnseignant> ListeAnneeUniversitaireEnseignants = new List<AnneeUniversitaireEnseignant>();
            AnneeUniversitaireEnseignant AnneUnivE;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    Command.Connection = Cnn;
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            AnneUnivE = new AnneeUniversitaireEnseignant();                                               
                            AnneUnivE.Id = dr.GetInt64(0);
                            AnneUnivE.CIN = dr.GetString(1);
                            AnneUnivE.Nom = dr.GetString(2);
                            AnneUnivE.Prenom = dr.GetString(3);
                            AnneUnivE.Grade = dr.GetString(4);
                            AnneUnivE.Statut = dr.GetString(5);
                            AnneUnivE.ValidationChargeDepartement = (string)dr["ValidationChargeDepartement"] ;
                            AnneUnivE.DateValidationChargeDepartement = dr["DateValidationChargeDepartement"] == System.DBNull.Value?   (DateTime?)null : (DateTime)dr["DateValidationChargeDepartement"];
                            AnneUnivE.ValidationChargeAdministration = (string)dr["ValidationChargeAdministration"];
                            AnneUnivE.DateValidationChargeAdminsitration = dr["DateValidationChargeAdminsitration"] == System.DBNull.Value? (DateTime?)null : (DateTime)dr["DateValidationChargeAdminsitration"];
                            AnneUnivE.EtatSaisie  = dr.GetString(12); 

                            AnneUnivE.CodeDepartement = dr.GetString(6);
                            AnneUnivE.IntituleFrDepartement = Dal_Departement.SelectByCode(AnneUnivE.CodeDepartement).IntituleFr;
                            AnneUnivE.CodeAnneeUniv = dr.GetString(7);

                            ListeAnneeUniversitaireEnseignants.Add(AnneUnivE);
                        }
                    }
                    return ListeAnneeUniversitaireEnseignants;
                }
                catch (SqlException e)
                {
                    //throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");
                    throw new MyException(e, "DataBase Errors", e.Message, "DAL");
                }
                finally
                {
                    Cnn.Close();
                }

            }

        }
        public static AnneeUniversitaireEnseignant SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE Id = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_AnneeUniversitaireEnseignant.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static AnneeUniversitaireEnseignant SelectByCIN(string CIN)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE [CIN] = @CIN";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@CIN", CIN);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_AnneeUniversitaireEnseignant.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

        public static AnneeUniversitaireEnseignant SelectTheLast()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE Id = (SELECT MAX(Id) FROM [AnneeUniversitaireEnseignant] )";
                var Command = new SqlCommand(StrSQL, Cnn);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_AnneeUniversitaireEnseignant.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

        public static AnneeUniversitaireEnseignant SelectByAUCIN(string CodeAnneeUniv, string CIN)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE [CIN] = @CIN AND [CodeAnneeUniv] = @CodeAnneeUniv" ;
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@CIN", CIN);
                Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_AnneeUniversitaireEnseignant.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static List<AnneeUniversitaireEnseignant> SelectAll(string CodeAnneeUniv)
        {
            string StrSQL = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE [CodeAnneeUniv] = @CodeAnneeUniv";
            var Command = new SqlCommand(StrSQL);
            Command.Parameters.AddWithValue("@CodeAnneeUniv",CodeAnneeUniv);
            return Dal_AnneeUniversitaireEnseignant.SelectAll(Command);
        }
        public static List<AnneeUniversitaireEnseignant> SelectAll(List<string> ListCIN, string CodeAnneeUniv)
        {

            var Command = new SqlCommand();
            var StrListCIN = String.Join(",", ListCIN.ToArray());

            Command.CommandText = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE [CIN] IN "+"("+StrListCIN+")"+" AND [CodeAnneeUniv] = @CodeAnneeUniv";
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);

            return Dal_AnneeUniversitaireEnseignant.SelectAll(Command);
        }
        public static List<AnneeUniversitaireEnseignant> SelectAll(string FieldName, string FielValue,string CodeAnneeUniv)
        {
            var Command = new SqlCommand();
            if(FieldName == "Departement")
            {
                Command.CommandText  = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE [CodeDepartement] = ";
                Command.CommandText  += "(SELECT [Code] FROM [Departement] WHERE [IntituleFr] = @FielValue) AND [CodeAnneeUniv] = @CodeAnneeUniv";
            }
            else if(FieldName == "Grade")
                Command.CommandText  = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE [Grade] = @FielValue AND [CodeAnneeUniv] = @CodeAnneeUniv";
            
            else if(FieldName == "Statut")
                Command.CommandText  = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE [Statut] = @FielValue AND [CodeAnneeUniv] = @CodeAnneeUniv";

            else if(FieldName == "Nom")
            {
                FielValue = FielValue.TrimEnd(' ').TrimStart(' ').Replace(' ','%');
                Command.CommandText  = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE ([Nom] + Space(1) + [Prenom] LIKE '%"+FielValue+"%' ";
                Command.CommandText +=  "OR [Prenom] + Space(1) + [Nom] LIKE '%"+FielValue+"%') AND [CodeAnneeUniv] = @CodeAnneeUniv";
            }
            else if(FieldName == "Tous")
                return Dal_AnneeUniversitaireEnseignant.SelectAll(CodeAnneeUniv);
            else
                Command.CommandText  = "SELECT * FROM [AnneeUniversitaireEnseignant] WHERE [CIN] = @FielValue AND [CodeAnneeUniv] = @CodeAnneeUniv";

            Command.Parameters.AddWithValue("@FielValue", FielValue);
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);

            return Dal_AnneeUniversitaireEnseignant.SelectAll(Command);            
        }
        public static bool IsForeignKeyInTable(string TableName, long Id)
        {
            int NbOccs = 0;
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [IdAUEnseignant] = @Id";
                Command.Parameters.AddWithValue("@Id", Id); 
                Command.Connection = Cnn;
                NbOccs = (int)DataBaseAccessUtilities.ScalarRequest(Command);
            }

            if (NbOccs == 0)
                return false;
            else
                return true;
        }
        public static void LockOrUnLockCharge(long IdTeacher, string KeyWord)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;

                if( KeyWord == "lock")
                {
                    Command.CommandText = "Update [AnneeUniversitaireEnseignant] set [EtatSaisie] = 'Verrouillee' WHERE [Id] = @IdTeacher";
                }
                else
                {
                    Command.CommandText = "Update [AnneeUniversitaireEnseignant] set [EtatSaisie] = 'En Cours' WHERE [Id] = @IdTeacher";
                }
                Command.Parameters.AddWithValue("@IdTeacher", IdTeacher);
                
                DataBaseAccessUtilities.NonQueryRequest(Command);
            }    
        }
        public static void ValidateOrUnValidate(long IdTeacher, string LieuDEValidation, string KeyWord)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;

               if(LieuDEValidation == "Departement")
               {
                    if( KeyWord == "validate")
                    {
                        Command.CommandText = "Update [AnneeUniversitaireEnseignant] set [ValidationChargeDepartement] = 'Validee', ";
                        Command.CommandText += "[DateValidationChargeDepartement] = @Date WHERE [Id] = @IdTeacher";
                    }
                    else
                    {
                        Command.CommandText = "Update [AnneeUniversitaireEnseignant] set [ValidationChargeDepartement] = 'Non Validee', ";
                        Command.CommandText += "[DateValidationChargeDepartement] = @Date, [EtatSaisie] = 'Verrouillee'  WHERE [Id] = @IdTeacher";
                    }
               }
               else
               {
                   if( KeyWord == "validate")
                    {
                        Command.CommandText = "Update [AnneeUniversitaireEnseignant] set [ValidationChargeAdministration] = 'Validee', ";
                        Command.CommandText += "[DateValidationChargeAdminsitration] = @Date WHERE [Id] = @IdTeacher";
                    }
                    else
                    {
                        Command.CommandText = "Update [AnneeUniversitaireEnseignant] set [ValidationChargeAdministration] = 'Non Validee', ";
                        Command.CommandText += "[DateValidationChargeAdminsitration] = @Date WHERE [Id] = @IdTeacher";
                    }
               }
                Command.Parameters.AddWithValue("@IdTeacher", IdTeacher);
                Command.Parameters.AddWithValue("@Date", DateTime.Now);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }    
        }

        public static void ChargeVerified(long IdTeacher)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;

                Command.CommandText = "Update [AnneeUniversitaireEnseignant] set [EtatSaisie] = 'Verifiee' WHERE [Id] = @IdTeacher";                
                Command.Parameters.AddWithValue("@IdTeacher", IdTeacher);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

    }
}