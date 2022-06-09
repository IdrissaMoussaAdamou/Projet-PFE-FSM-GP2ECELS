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
    public class Dal_Niveau
    {
        private static Niveau GetEntityFromDataRow(DataRow dr)
        {
            Niveau Niv = new Niveau();
            Niv.IdFiliere = (Int64)dr["IdFiliere"];    
            Niv.Code = (string)dr["Code"];
            Niv.IntituleFr = dr["IntituleFr"] == System.DBNull.Value ? "" : (string)dr["IntituleFr"];
            Niv.IntituleAr = dr["IntituleAr"] == System.DBNull.Value ? "" : (string)dr["IntituleAr"];
            Niv.IntituleAbrg = dr["IntituleAbrg"] == System.DBNull.Value ? "" : (string)dr["IntituleAbrg"];
            Niv.Id = (Int64)dr["Id"]; 
            return Niv;
        }

        private static List<Niveau> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<Niveau> L = new List<Niveau>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_Niveau.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(Niveau Niv)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if(DataBaseAccessUtilities.CheckKeyUnicity("Niveau","Code",SqlDbType.NVarChar,Niv.Code) == true)
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "INSERT INTO [Niveau](IdFiliere,Code,IntituleFr,IntituleAr,IntituleAbrg)";
                    Command.CommandText += "VALUES(@IdFiliere,@Code,@IntituleFr,@IntituleAr,@IntituleAbrg)";

                    Command.Parameters.AddWithValue("@IdFiliere",Niv.IdFiliere);
                    Command.Parameters.AddWithValue("@Code",Niv.Code);
                    Command.Parameters.AddWithValue("@IntituleFr",Niv.IntituleFr);
                    Command.Parameters.AddWithValue("@IntituleAr",Niv.IntituleAr);
                    Command.Parameters.AddWithValue("@IntituleAbrg",Niv.IntituleAbrg);

                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
                else
                {
                    throw new MyException("Erreur dans l'ajout d'un Niveau", "Le Code saisi est déja utilisé", "DAL");   
                }
            }
        }

        public static int Delete(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {   
                string StrRequest = "DELETE FROM [Niveau] WHERE [Id] = @Id";
                var Command = new SqlCommand(StrRequest, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);

                return DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }
        
        public static void Update(Niveau Niv )
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE [Niveau] SET [IdFiliere] = @IdFiliere, [Code] = @Code, [IntituleFr] = @IntituleFr,";
                Command.CommandText += "[IntituleAr] = @IntituleAr ,[IntituleAbrg] = @IntituleAbrg WHERE [Id] = @Id";

                Command.Parameters.AddWithValue("@IdFiliere", Niv.IdFiliere);
                Command.Parameters.AddWithValue("@Code", Niv.Code);
                Command.Parameters.AddWithValue("@IntituleFr", Niv.IntituleFr);
                Command.Parameters.AddWithValue("@IntituleAr", Niv.IntituleAr);
                Command.Parameters.AddWithValue("@IntituleAbrg", Niv.IntituleAbrg);
                Command.Parameters.AddWithValue("@Id", Niv.Id);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static List<Niveau> SelectAll(SqlCommand Command)
        {
            List<Niveau> ListeNiveaux = new List<Niveau>();
            Niveau Niv;

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
                            Niv = new Niveau();                                               
                            Niv.Code = dr.GetString(0);
                            Niv.IntituleFr = dr.GetString(1);
                            Niv.IntituleAr = dr.GetString(2);
                            Niv.IntituleAbrg = dr.GetString(3);
                            Niv.IdFiliere = dr.GetInt64(4);
                            Niv.Id = dr.GetInt64(5);
                            ListeNiveaux.Add(Niv);
                        }
                    }
                    return ListeNiveaux;
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

        public static List<Niveau> SelectAll()
        {
            string StrSQL = "SELECT * FROM [Niveau]";
            var Command = new SqlCommand(StrSQL);
            
            return Dal_Niveau.SelectAll(Command);
        }
        public static List<Niveau> SelectAll(string IdFiliere, string CodeAnneeUniv)
        {
            string StrSQL = "SELECT * FROM [Niveau] WHERE [IdFiliere]= @IdFiliere AND [Code] NOT IN "; 
            StrSQL += "(SELECT [CodeNiveau] FROM [AnneeUniversitaireNiveauParcoursPeriode] WHERE [IdFiliere] = @IdFiliere AND [CodeAnneeUniv] =@CodeAnneeUniv)";
            var Command = new SqlCommand(StrSQL);
            Command.Parameters.AddWithValue("@IdFiliere", IdFiliere);
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);

            return Dal_Niveau.SelectAll(Command);
        }

        public static List<Niveau> GetAll(long IdFiliere, string CodeAnneeUniv)
        {
            string StrSQL = "SELECT * FROM [Niveau] WHERE [IdFiliere]= @IdFiliere AND [Id] IN "; 
            StrSQL += "(SELECT [IdNiveau] FROM [AnneeUniversitaireNiveauParcoursPeriode] WHERE [IdFiliere] = @IdFiliere AND [CodeAnneeUniv] =@CodeAnneeUniv)";
            var Command = new SqlCommand(StrSQL);
            Command.Parameters.AddWithValue("@IdFiliere", IdFiliere);
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);

            return Dal_Niveau.SelectAll(Command);
        }


        public static List<Niveau> SelectFiliereNiveaux(long IdFiliere)
        {
            List<Niveau> ListeNiveaux = new List<Niveau>();
            Niveau Niv;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    var Command = new SqlCommand();
                    Command.CommandText = "SELECT * FROM [Niveau] WHERE [IdFiliere]= @IdFiliere";
                    Command.Parameters.Add("@IdFiliere",SqlDbType.VarChar).Value = IdFiliere;
                    Command.Connection = Cnn;

                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            Niv = new Niveau();                                               
                            Niv.Code = dr.GetString(0);
                            Niv.IntituleFr = dr.GetString(1);
                            Niv.IntituleAr = dr.GetString(2);
                            Niv.IntituleAbrg = dr.GetString(3);
                            Niv.IdFiliere = dr.GetInt64(4);
                            Niv.Id = dr.GetInt64(5);

                            ListeNiveaux.Add(Niv);
                        }
                    }
                    return ListeNiveaux;
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

        public static Niveau SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Niveau] WHERE [Id] = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Niveau.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static Niveau SelectTheLast()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Niveau] WHERE [Id] = (SELECT MAX(Id) FROM [Niveau] )";
                var Command = new SqlCommand(StrSQL, Cnn);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Niveau.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static Niveau SelectByIntituleFr(string IntituleFr)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Niveau] WHERE IntituleFr = @IntituleFr";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@IntituleFr", IntituleFr);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Niveau.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

        public static Niveau SelectByIntituleAbr(string IntituleAbr)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Niveau] WHERE IntituleAbrg = @IntituleFr";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@IntituleFr", IntituleAbr);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Niveau.GetEntityFromDataRow(dt.Rows[0]);
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
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [IdNiveau] = @Id";  
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