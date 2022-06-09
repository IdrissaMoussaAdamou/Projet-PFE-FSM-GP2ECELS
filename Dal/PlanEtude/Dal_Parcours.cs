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
    public class Dal_Parcours
    {
        private static Parcours GetEntityFromDataRow(DataRow dr)
        {
            Parcours Pars = new Parcours();
            Pars.IdFiliere = Convert.ToInt64(dr["IdFiliere"]);
            Pars.Id = Convert.ToInt64(dr["Id"]);   
            Pars.Code = (string)dr["Code"];
            Pars.IntituleFr = dr["IntituleFr"] == System.DBNull.Value ? "" : (string)dr["IntituleFr"];
            Pars.IntituleAr = dr["IntituleAr"] == System.DBNull.Value ? "" : (string)dr["IntituleAr"];
            Pars.IntituleAbrg = dr["IntituleAbrg"] == System.DBNull.Value ? "" : (string)dr["IntituleAbrg"];
            Pars.PeriodeHabilitation = dr["PeriodeHabilitation"] == System.DBNull.Value ? "" : (string)dr["PeriodeHabilitation"];
            Pars.PeriodeDebut = (int)dr["PeriodeDebut"];
            Pars.PeriodeFin = (int)dr["PeriodeFin"];

            return Pars;
        }

        private static List<Parcours> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<Parcours> L = new List<Parcours>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_Parcours.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }

        public static void Add(Parcours Pars)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [Parcours](IdFiliere,Code,IntituleFr,IntituleAr,IntituleAbrg,PeriodeHabilitation,PeriodeDebut,PeriodeFin)";
                Command.CommandText += "VALUES(@IdFiliere,@Code,@IntituleFr,@IntituleAr,@IntituleAbrg,@PeriodeHabilitation,@PeriodeDebut,@PeriodeFin)";

                Command.Parameters.AddWithValue("@IdFiliere", Pars.IdFiliere);
                Command.Parameters.AddWithValue("@Code", Pars.Code);
                Command.Parameters.AddWithValue("@IntituleFr", Pars.IntituleFr);
                Command.Parameters.AddWithValue("@IntituleAr", Pars.IntituleAr);
                Command.Parameters.AddWithValue("@IntituleAbrg", Pars.IntituleAbrg);
                Command.Parameters.AddWithValue("@PeriodeHabilitation", Pars.PeriodeHabilitation);
                Command.Parameters.AddWithValue("@PeriodeDebut", Pars.PeriodeDebut);
                Command.Parameters.AddWithValue("@PeriodeFin", Pars.PeriodeFin);
                
                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static void Delete(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrRequest = "DELETE FROM [Parcours] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                
                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }
        
        public static void Update(Parcours Pars )
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
               
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE [Parcours] SET [Code] = @Code, [IntituleFr] = @IntituleFr, [IntituleAr] = @IntituleAr, ";
                Command.CommandText += "[IntituleAbrg] = @IntituleAbrg, [PeriodeHabilitation] = @PeriodeHabilitation, [PeriodeDebut] = @PeriodeDebut, [PeriodeFin] = @PeriodeFin WHERE [Id]= @Id";

                Command.Parameters.AddWithValue("@Code", Pars.Code);
                Command.Parameters.AddWithValue("@IntituleFr", Pars.IntituleFr);
                Command.Parameters.AddWithValue("@IntituleAr", Pars.IntituleAr);
                Command.Parameters.AddWithValue("@IntituleAbrg", Pars.IntituleAbrg);
                Command.Parameters.AddWithValue("@PeriodeHabilitation", Pars.PeriodeHabilitation);
                Command.Parameters.AddWithValue("@PeriodeDebut", Pars.PeriodeDebut);
                Command.Parameters.AddWithValue("@PeriodeFin", Pars.PeriodeFin);
                Command.Parameters.AddWithValue("@Id", Pars.Id);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            
            }
        }

        public static List<Parcours> SelectAll(SqlCommand Command)
        {
            List<Parcours> ListeParcours = new List<Parcours>();
            Parcours Pars;

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
                            Pars = new Parcours();                                               
                            Pars.Code = dr.GetString(0);
                            Pars.IntituleFr = dr.GetString(1);
                            Pars.IntituleAr = dr.GetString(2);
                            Pars.IntituleAbrg = dr.GetString(3);
                            Pars.PeriodeHabilitation = dr.GetString(4);
                            Pars.PeriodeDebut = dr.GetInt32(5);
                            Pars.PeriodeFin = dr.GetInt32(6);
                            Pars.IdFiliere = dr.GetInt64(7);
                            Pars.Id = dr.GetInt64(8);
           
                            ListeParcours.Add(Pars);
                        }
                    }
                    return ListeParcours;
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

        public static List<Parcours> SelectAll()
        {
            string StrSQL = "SELECT * FROM [Parcours]";
            var Command = new SqlCommand(StrSQL);

            return Dal_Parcours.SelectAll(Command);
        }

        public static List<Parcours> SelectAll(string CodeFiliere, string CodeAnneeUniv)
        {
            var Command = new SqlCommand();
            Command.CommandText = "SELECT * FROM [Parcours] WHERE [CodeFiliere] = @CodeFiliere AND [Code] NOT IN ";
            Command.CommandText += "(Select [CodeParcours] FROM [AnneeUniversitaireNiveauParcoursPeriode] WHERE [CodeFiliere] = @CodeFiliere AND [CodeAnneeUniv] = @CodeAnneeUniv)";

            Command.Parameters.AddWithValue("@CodeFiliere", CodeFiliere);
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv); 

            return Dal_Parcours.SelectAll(Command);
        }

        public static List<Parcours> GetAll(long IdFiliere, string CodeAnneeUniv)
        {
            var Command = new SqlCommand();
            Command.CommandText = "SELECT * FROM [Parcours] WHERE [IdFiliere] = @IdFiliere AND [Id] IN ";
            Command.CommandText += "(Select [IdParcours] FROM [AnneeUniversitaireNiveauParcoursPeriode] WHERE [IdFiliere] = @IdFiliere AND [CodeAnneeUniv] = @CodeAnneeUniv)";

            Command.Parameters.AddWithValue("@IdFiliere", IdFiliere);
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv); 

            return Dal_Parcours.SelectAll(Command);
        }

        public static List<Parcours> SelectFiliereParcours(long IdFiliere)
        {
            List<Parcours> ListeParcours = new List<Parcours>();
            Parcours Pars;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    var Command = new SqlCommand();
                    Command.CommandText = "SELECT * FROM [Parcours] WHERE [IdFiliere]= @IdFiliere";
                    Command.Parameters.Add("@IdFiliere",SqlDbType.VarChar).Value = IdFiliere;
                    Command.Connection = Cnn;
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            Pars = new Parcours();                                               
                            Pars.Code = dr.GetString(0);
                            Pars.IntituleFr = dr.GetString(1);
                            Pars.IntituleAr = dr.GetString(2);
                            Pars.IntituleAbrg = dr.GetString(3);
                            Pars.PeriodeHabilitation = dr.GetString(4);
                            Pars.PeriodeDebut = dr.GetInt32(5);
                            Pars.PeriodeFin = dr.GetInt32(6);
                            Pars.IdFiliere = dr.GetInt64(7);
                            Pars.Id = dr.GetInt64(8);
           
                            ListeParcours.Add(Pars);
                        }
                    }
                    return ListeParcours;
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

        public static Parcours SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Parcours] WHERE [Id] = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Parcours.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

        public static Parcours SelectTheLast()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Parcours] WHERE [Id] = (SELECT MAX(Id) FROM [Parcours])";
                var Command = new SqlCommand(StrSQL, Cnn);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Parcours.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static Parcours SelectByIntituleFr(string IntituleFr)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Parcours] WHERE IntituleFr = @IntituleFr";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@IntituleFr", IntituleFr);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Parcours.GetEntityFromDataRow(dt.Rows[0]);
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
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [IdParcours] = @Id";  
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