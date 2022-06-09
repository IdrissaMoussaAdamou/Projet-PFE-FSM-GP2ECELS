using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Projet_PFE.Models;
using Projet_PFE.MyUtilities;

namespace Projet_PFE.Dal
{
    public class Dal_Filiere
    {
        private static Filiere GetEntityFromDataRow(DataRow dr)
        {
            var Flre = new Filiere();

            Flre.CodeTypeDiplome = (string)dr["CodeTypeDiplome"];
            Flre.CodeDepartement = (string)dr["CodeDepartement"];
            Flre.CodeTypePeriode = (string)dr["CodeTypePeriode"];

            Flre.Id =  (Int64)dr["Id"]; 
            Flre.Code = (string)dr["Code"];
            Flre.IntituleFr = dr["IntituleFr"] == System.DBNull.Value ? "" : (string)dr["IntituleFr"];
            Flre.IntituleAr = dr["IntituleAr"] == System.DBNull.Value ? "" : (string)dr["IntituleAr"];
            Flre.IntituleAbrg = dr["IntituleAbrg"] == System.DBNull.Value ? "" : (string)dr["IntituleAbrg"];
            Flre.Domaine = dr["Domaine"] == System.DBNull.Value ? "" : (string)dr["Domaine"];
            Flre.Mention = dr["Mention"] == System.DBNull.Value ? "" : (string)dr["Mention"];
            Flre.PeriodeHabilitation = dr["PeriodeHabilitation"] == System.DBNull.Value ? "" : (string)dr["PeriodeHabilitation"];
            Flre.NbPeriodes = (int)dr["NbPeriodes"];

            Flre.IntituleFrDepartement = Dal_Departement.SelectByCode(Flre.CodeDepartement).IntituleFr;
            Flre.IntituleFrTypeDiplome = Dal_TypeDiplome.SelectByCode(Flre.CodeTypeDiplome).IntituleFr;
            Flre.TypePeriode = Dal_TypePeriode.SelectByCode(Flre.CodeTypePeriode).Type;
            
            return Flre;
        }

        private static List<Filiere> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<Filiere> L = new List<Filiere>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_Filiere.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }

        public static void Add(Filiere Flre)
        {
           using (var Cnn = new SqlConnection(Config.GetConnectionString()))
           {
               
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [Filiere](CodeTypeDiplome,CodeDepartement,Code,IntituleFr,IntituleAr,IntituleAbrg,Domaine,Mention,PeriodeHabilitation,NbPeriodes,CodeTypePeriode)";
                Command.CommandText += "VALUES(@CodeTypeDiplome,@CodeDepartement,@Code,@IntituleFr,@IntituleAr,@IntituleAbrg,@Domaine,@Mention,@PeriodeHabilitation,@NbPeriodes,@CodeTypePeriode)";

                Command.Parameters.AddWithValue("@CodeTypeDiplome", Flre.CodeTypeDiplome);
                Command.Parameters.AddWithValue("@CodeDepartement", Flre.CodeDepartement);
                Command.Parameters.AddWithValue("@Code", Flre.Code);
                Command.Parameters.AddWithValue("@IntituleFr", Flre.IntituleFr);
                Command.Parameters.AddWithValue("@IntituleAr", Flre.IntituleAr);
                Command.Parameters.AddWithValue("@IntituleAbrg", Flre.IntituleAbrg);
                Command.Parameters.AddWithValue("@Domaine", Flre.Domaine);
                Command.Parameters.AddWithValue("@Mention", Flre.Mention);
                Command.Parameters.AddWithValue("@PeriodeHabilitation", Flre.PeriodeHabilitation);
                Command.Parameters.AddWithValue("@NbPeriodes", Flre.NbPeriodes);
                Command.Parameters.AddWithValue("@CodeTypePeriode", Flre.CodeTypePeriode);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            
           }
        }

        public static void Delete(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrRequest = "DELETE FROM [Filiere] WHERE [Id] = @Id";
                var Command = new SqlCommand(StrRequest, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                
                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }
        
        public static void Update(Filiere Flre )
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
              
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE [Filiere] SET [CodeTypeDiplome] = @CodeTypeDiplome, [CodeDepartement] = @CodeDepartement, [Code] = @Code, [IntituleFr] = @IntituleFr, ";
                Command.CommandText += "[IntituleAr] = @IntituleAr ,[IntituleAbrg] = @IntituleAbrg, [Domaine] = @Domaine, [Mention] = @Mention, [PeriodeHabilitation] = @PeriodeHabilitation, ";
                Command.CommandText += "[NbPeriodes]= @NbPeriodes ,[CodeTypePeriode] = @CodeTypePeriode WHERE [Id]= @Id";

                Command.Parameters.AddWithValue("@CodeTypeDiplome", Flre.CodeTypeDiplome);
                Command.Parameters.AddWithValue("@CodeDepartement", Flre.CodeDepartement);
                Command.Parameters.AddWithValue("@Code", Flre.Code);
                Command.Parameters.AddWithValue("@IntituleFr", Flre.IntituleFr);
                Command.Parameters.AddWithValue("@IntituleAr", Flre.IntituleAr);
                Command.Parameters.AddWithValue("@IntituleAbrg", Flre.IntituleAbrg);
                Command.Parameters.AddWithValue("@Domaine", Flre.Domaine);
                Command.Parameters.AddWithValue("@Mention", Flre.Mention);
                Command.Parameters.AddWithValue("@PeriodeHabilitation", Flre.PeriodeHabilitation);
                Command.Parameters.AddWithValue("@NbPeriodes", Flre.NbPeriodes);
                Command.Parameters.AddWithValue("@CodeTypePeriode", Flre.CodeTypePeriode);
                Command.Parameters.AddWithValue("@Id", Flre.Id);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            
            }
        }

        public static List<Filiere> SelectAll()
        {
            string StrSQL = "SELECT * FROM [Filiere] ";
            var Command = new SqlCommand(StrSQL);
            return Dal_Filiere.SelectAll(Command);
        }
        public static List<Filiere> SelectAll(string CodeAnneeUniv)
        {
            string StrSQL = "SELECT * FROM [Filiere] WHERE [Id] In (SELECT [IdFiliere] FROM [AnneeUniversitaireFilieres] WHERE [CodeAnneeUniv] = @CodeAnneeUniv) ";
            var Command = new SqlCommand(StrSQL);
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);
            return Dal_Filiere.SelectAll(Command);
        }
        public static List<Filiere> SelectAll(SqlCommand Command)
        {
            List<Filiere> ListeFilieres = new List<Filiere>();
            Filiere Flre;

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
                            Flre = new Filiere();
                            Flre.Code = dr.GetString(0);
                            Flre.IntituleFr =  dr.GetString(1);
                            Flre.IntituleAr =  dr.GetString(2);
                            Flre.IntituleAbrg =  dr.GetString(3);
                            Flre.Domaine =  dr.GetString(4);
                            Flre.Mention = dr.GetString(5);
                            Flre.CodeTypeDiplome = dr.GetString(6); 
                            Flre.CodeDepartement = dr.GetString(7); 
                            Flre.PeriodeHabilitation = dr.GetString(8);
                            Flre.NbPeriodes = dr.GetInt32(9);
                            Flre.CodeTypePeriode = dr.GetString(10);
                            Flre.Id = dr.GetInt64(11);

                            Flre.IntituleFrDepartement = Dal_Departement.SelectByCode(Flre.CodeDepartement).IntituleFr;
                            Flre.IntituleFrTypeDiplome = Dal_TypeDiplome.SelectByCode(Flre.CodeTypeDiplome).IntituleFr;
                            Flre.TypePeriode = Dal_TypePeriode.SelectByCode(Flre.CodeTypePeriode).Type;
                            
                            ListeFilieres.Add(Flre);
                        }
                    }
                    return ListeFilieres;
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

        public static List<Filiere> SelectAll(string CodeTypeDiplome, string CodeAnneeUniv)
        {
            var Command = new SqlCommand();
            Command.CommandText = "SELECT * FROM [Filiere] WHERE [CodeTypeDiplome] = @CodeTypeDiplome  AND [Id] NOT In (SELECT [IdFiliere] FROM [AnneeUniversitaireFilieres] WHERE [CodeAnneeUniv] = @CodeAnneeUniv) ";
            Command.Parameters.AddWithValue("@CodeTypeDiplome", CodeTypeDiplome);
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);

            return Dal_Filiere.SelectAll(Command);
        }

        public static List<Filiere> SelectAll(long IdTeacher)
        {
            var Command = new SqlCommand();
            Command.CommandText = "SELECT * FROM [Filiere] WHERE [CodeDepartement] = (SELECT [CodeDepartement] FROM [AnneeUniversitaireEnseignant] WHERE [Id] = @IdTeacher)";
            
            Command.Parameters.AddWithValue("@IdTeacher", IdTeacher);
            return Dal_Filiere.SelectAll(Command);
        }
        public static List<Filiere> GetAll(string CodeTypeDiplome, string CodeAnneeUniv)
        {
            var Command = new SqlCommand();
            Command.CommandText = "SELECT * FROM [Filiere] WHERE [CodeTypeDiplome] = @CodeTypeDiplome  AND [Id] In (SELECT [IdFiliere] FROM [AnneeUniversitaireFilieres] WHERE [CodeAnneeUniv] = @CodeAnneeUniv) ";
            Command.Parameters.AddWithValue("@CodeTypeDiplome", CodeTypeDiplome);
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);

            return Dal_Filiere.SelectAll(Command);
        }
        public static Filiere SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Filiere] WHERE Id = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Filiere.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static Filiere SelectTheLast()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Filiere] WHERE Id = (SELECT MAX(Id) FROM [Filiere])";
                var Command = new SqlCommand(StrSQL, Cnn);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Filiere.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static bool IsForeignKeyInTable(string TableName, long Id)
        {
            int NbOccs = 0;
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [IdFiliere] = @Id";  
                Command.Parameters.AddWithValue("@Id", Id);
                Command.Connection = Cnn;
                NbOccs = (int)DataBaseAccessUtilities.ScalarRequest(Command);
            }

            if (NbOccs == 0)
                return false;
            else
                return true;
        }
    }
}