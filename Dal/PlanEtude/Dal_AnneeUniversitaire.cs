using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;
using Projet_PFE.MyUtilities;

namespace Projet_PFE.Dal
{
    public class Dal_AnneeUniversitaire
    {
        private static AnneeUniversitaire GetEntityFromDataRow(DataRow dr)
        {
            AnneeUniversitaire AnneeUniv = new AnneeUniversitaire();
            AnneeUniv.Code = (string)dr["Code"];
            AnneeUniv.DateDebut = (DateTime)dr["DateDebut"];
            AnneeUniv.DateFin = (DateTime)dr["DateFin"];
            AnneeUniv.EtatPlanEtudes = (string)dr["EtatPlanEtudes"];
            AnneeUniv.EtatCharges = (string)dr["EtatCharges"];

            return AnneeUniv;
        }

        private static List<AnneeUniversitaire> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<AnneeUniversitaire> L = new List<AnneeUniversitaire>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_AnneeUniversitaire.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(AnneeUniversitaire AnneeUniv)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if(DataBaseAccessUtilities.CheckKeyUnicity("AnneeUniversitaire","Code",SqlDbType.VarChar,AnneeUniv.Code) == true )
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "INSERT INTO [AnneeUniversitaire](Code,DateDebut,DateFin) VALUES(@Code,@DateDebut,@DateFin)";
                    Command.Parameters.AddWithValue("@Code",AnneeUniv.Code);
                    Command.Parameters.AddWithValue("@DateDebut",AnneeUniv.DateDebut);
                    Command.Parameters.AddWithValue("@DateFin",AnneeUniv.DateFin);

                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
                else
                {
                    throw new MyException("Erreur dans l'ajout d'une AnneeUniversitaire", "Cette année Universitaire déjà utilisé", "DAL");
                }
            }
        }

        public static  void Delete(string Code)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
             {
                string StrRequest = "DELETE FROM [AnneeUniversitaire] WHERE [Code]=@Code";
                var Command = new SqlCommand(StrRequest,Cnn);
                Command.Parameters.AddWithValue("@Code",Code);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update( string OldCode, AnneeUniversitaire AnneeUniv)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if ((OldCode != AnneeUniv.Code) && DataBaseAccessUtilities.CheckKeyUnicity("AnneeUniversitaire","Code",SqlDbType.VarChar,AnneeUniv.Code) == false)
                {
                    throw new MyException("Erreur dans la modification d' une AnneeUniversitaire", "Le nouveau Code est déjà utilisé", "DAL");
                }
                else
                {   
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "UPDATE [AnneeUniversitaire] SET [Code]=@Code, [DateDebut]=@DateDebut, [DateFin]=@DateFin WHERE Code=@OldCode";
                    Command.Parameters.Add(new SqlParameter("@Code",AnneeUniv.Code));
                    Command.Parameters.Add(new SqlParameter("@DateDebut",AnneeUniv.DateDebut));
                    Command.Parameters.Add(new SqlParameter("@DateFin",AnneeUniv.DateFin));
                    Command.Parameters.Add(new SqlParameter("@OldCode",OldCode));
                    
                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }    
            
            }
        }

        public static void ArchiveAnneeUniversitaire(string Code)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "UPDATE [AnneeUniversitaire] SET [EtatPlanEtudes]= 'Cloturée' WHERE Code=@Code";
                    Command.Parameters.Add(new SqlParameter("@Code",Code));
                    
                    DataBaseAccessUtilities.NonQueryRequest(Command);
            
            }
        }

        public static void ArchiverChargeEnseignement(string Code)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "UPDATE [AnneeUniversitaire] SET [EtatCharges]= 'Cloturée' WHERE Code=@Code";
                    Command.Parameters.Add(new SqlParameter("@Code",Code));
                    
                    DataBaseAccessUtilities.NonQueryRequest(Command);
            
            }
        }
        public static List<AnneeUniversitaire> SelectAll()
        {
            List<AnneeUniversitaire> ListeAnneeUniversitaires = new List<AnneeUniversitaire>();
            AnneeUniversitaire AnneeUniv;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    string StrSQL = "SELECT * FROM [AnneeUniversitaire] ";
                    var Command = new SqlCommand(StrSQL, Cnn);
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            AnneeUniv = new AnneeUniversitaire();                                               
                            AnneeUniv.Code = dr.GetString(0);
                            AnneeUniv.DateDebut = dr.GetDateTime(1);
                            AnneeUniv.DateFin = dr.GetDateTime(2);
                            AnneeUniv.EtatPlanEtudes = dr.GetString(3);
                            AnneeUniv.EtatCharges = dr.GetString(4);
                            ListeAnneeUniversitaires.Add(AnneeUniv);
                        }
                    }
                    return ListeAnneeUniversitaires;
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
        public static AnneeUniversitaire SelectByCode(string Code)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [AnneeUniversitaire] WHERE Code = @Code";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Code", Code);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_AnneeUniversitaire.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static AnneeUniversitaire SelectTNArchivedAnneeUniv()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [AnneeUniversitaire] WHERE [EtatPlanEtudes] = 'En Cours'";
                var Command = new SqlCommand(StrSQL, Cnn);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_AnneeUniversitaire.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
   }
}