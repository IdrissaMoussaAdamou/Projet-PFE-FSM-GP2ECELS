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
    public class Dal_AnneeUniversitaireNomOption
    {
        private static NomOption GetEntityFromDataRow(DataRow dr)
        {
            NomOption Option = new NomOption();
            Option.IdModule = (Int64)dr["IdModule"];
            Option.CodeAnneeUniv = dr["CodeAnneeUniv"] == System.DBNull.Value ? "" : (string)dr["CodeAnneeUniv"];
            Option.Intitule = dr["Intitule"] == System.DBNull.Value ? "" : (string)dr["Intitule"];
            Option.IntituleAbrg = dr["IntituleAbrg"] == System.DBNull.Value ? "" : (string)dr["IntituleAbrg"];

            return Option;
        }

        private static List<NomOption> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<NomOption> L = new List<NomOption>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_AnneeUniversitaireNomOption.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(NomOption Option)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "INSERT INTO [AnneeUniversitaireNomOption](Intitule,IdModule,IntituleAbrg,CodeAnneeUniv) VALUES(@Intitule,@IdModule,@IntituleAbrg,@CodeAnneeUniv)";
                    Command.Parameters.AddWithValue("@Intitule",Option.Intitule);
                    Command.Parameters.AddWithValue("@IdModule",Option.IdModule);
                    Command.Parameters.AddWithValue("@IntituleAbrg",Option.IntituleAbrg);
                    Command.Parameters.AddWithValue("@CodeAnneeUniv",Option.CodeAnneeUniv);

                    DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static void Update(NomOption Option)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "Update  [AnneeUniversitaireNomOption] SET [Intitule] = @Intitule, [IntituleAbrg] =@IntituleAbrg WHERE Id = @Id";
                    Command.Parameters.AddWithValue("@Intitule", Option.Intitule);
                    Command.Parameters.AddWithValue("@IntituleAbrg", Option.IntituleAbrg);
                    Command.Parameters.AddWithValue("@Id", Option.Id);

                    DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }
        
        public static List<NomOption> SelectAll(long IdNiveau, long IdParcours, int Periode, string CodeAnneeUniv)
        {
            List<NomOption> ListNomOptions = new List<NomOption>();
            NomOption Option;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    string StrSQL = "SELECT * FROM [AnneeUniversitaireNomOption] WHERE [CodeAnneeUniv] = @CodeAnneeUniv AND [IdModule] IN ";
                    StrSQL += "(SELECT IdModule FROM [Module] WHERE [IdNiveau] = @IdNiveau AND [IdParcours] = @IdParcours AND [Periode] = @Periode)";
                    var Command = new SqlCommand(StrSQL, Cnn);

                    Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);
                    Command.Parameters.Add("@IdNiveau", SqlDbType.BigInt).Value = IdNiveau;
                    Command.Parameters.Add("@IdParcours", SqlDbType.BigInt).Value = IdParcours;
                    Command.Parameters.Add("@Periode", SqlDbType.Int).Value = Periode;
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            Option = new NomOption();                                               
                            Option.Id = dr.GetInt64(0);
                            Option.Intitule = dr.GetString(1);
                            Option.IdModule = dr.GetInt64(2);
                            Option.IntituleAbrg = dr.GetString(3);
                            Option.CodeAnneeUniv = dr.GetString(4);

                            ListNomOptions.Add(Option);
                        }
                    }
                    return ListNomOptions;
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
        public static NomOption SelectModuleNomOption(long IdModule, string CodeAnneeUniv)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [AnneeUniversitaireNomOption] WHERE [CodeAnneeUniv] = @CodeAnneeUniv AND  [IdModule] = @IdModule";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);
                Command.Parameters.AddWithValue("@IdModule", IdModule);

                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_AnneeUniversitaireNomOption.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        
    }
   
}