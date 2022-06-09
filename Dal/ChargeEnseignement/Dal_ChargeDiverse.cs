using System;
using System.Collections.Generic;
using Projet_PFE.MyUtilities;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;

namespace Projet_PFE.Dal
{
    public class Dal_ChargeDiverse
    {
        private static ChargeDiverse GetEntityFromDataRow(DataRow dr)
        {
            ChargeDiverse ChargDivers = new ChargeDiverse();
            ChargDivers.Id = (Int64)dr["Id"];

            ChargDivers.CodeAnneeUniv = (string)dr["CodeAnneeUniv"];
            ChargDivers.IdTypeChargeDiverse = (Int64)dr["IdTypeChargeDiverse"];
            ChargDivers.IdAUEnseignant = (Int64)dr["IdAUEnseignant"];

            ChargDivers.Periode = (string)dr["Periode"];
            ChargDivers.NumPeriodeDansAnnee = dr["NumPeriodeDansAnnee"] == System.DBNull.Value ? 0 : (int)dr["NumPeriodeDansAnnee"];
            ChargDivers.NbSemainesPeriode = dr["NbSemainesPeriode"] == System.DBNull.Value ? 0 : Convert.ToSingle(dr["NbSemainesPeriode"]);
            ChargDivers.NatureCharge = (string)dr["NatureCharge"];
            ChargDivers.Volume = Convert.ToSingle(dr["Volume"]);
            ChargDivers.Observations = dr["Observation"] == System.DBNull.Value ? "" : (string)dr["Observation"];
            ChargDivers.UniteVolume = (string)dr["UniteVolume"];

            return ChargDivers;
        }

        private static List<ChargeDiverse> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<ChargeDiverse> L = new List<ChargeDiverse>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_ChargeDiverse.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(ChargeDiverse ChargDivers)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [ChargeDiverse](CodeAnneeUniv, IdTypeChargeDiverse, IdAUEnseignant, Periode, NatureCharge, Volume, Observation, UniteVolume, ";
                Command.CommandText += "NumPeriodeDansAnnee, NbSemainesPeriode)";
                Command.CommandText += "values(@CodeAnneeUniv, @IdTypeChargeDiverse, @IdAUEnseignant, @Periode, @NatureCharge, @Volume, @Observation, @UniteVolume, @NumPeriodeDansAnnee, @NbSemainesPeriode)";

                Command.Parameters.AddWithValue("@CodeAnneeUniv", ChargDivers.CodeAnneeUniv);
                Command.Parameters.AddWithValue("@IdTypeChargeDiverse", ChargDivers.IdTypeChargeDiverse);
                Command.Parameters.AddWithValue("@IdAUEnseignant", ChargDivers.IdAUEnseignant);
                Command.Parameters.AddWithValue("@Periode", ChargDivers.Periode);
                Command.Parameters.AddWithValue("@NatureCharge", ChargDivers.NatureCharge);
                Command.Parameters.AddWithValue("@Volume", ChargDivers.Volume);
                
                 if(string.IsNullOrEmpty(ChargDivers.Observations))
                        Command.Parameters.Add("@Observation", SqlDbType.VarChar).Value = System.DBNull.Value;
                else
                    Command.Parameters.AddWithValue("@Observation", ChargDivers.Observations);
                Command.Parameters.AddWithValue("@UniteVolume", ChargDivers.UniteVolume);

                Command.Parameters.AddWithValue("@NumPeriodeDansAnnee", ChargDivers.NumPeriodeDansAnnee);
                Command.Parameters.AddWithValue("@NbSemainesPeriode", ChargDivers.NbSemainesPeriode);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static  void Delete(long Id)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
             {
                string StrRequest = "DELETE FROM [ChargeDiverse] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest ,Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update(ChargeDiverse ChargDivers)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
               
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE [ChargeDiverse] SET [CodeAnneeUniv] = @CodeAnneeUniv, [IdTypeChargeDiverse] = @IdTypeChargeDiverse, [IdAUEnseignant] = @IdAUEnseignant, ";
                Command.CommandText += "[Periode] = @Periode, [NatureCharge] = @NatureCharge, [Volume] = @Volume, [Observation] = @Observation, [UniteVolume] = @UniteVolume, ";
                Command.CommandText += "[NumPeriodeDansAnnee] = @NumPeriodeDansAnnee, [NbSemainesPeriode] = @NbSemainesPeriode WHERE [Id] = @Id";
                
                Command.Parameters.AddWithValue("@CodeAnneeUniv", ChargDivers.CodeAnneeUniv);
                Command.Parameters.AddWithValue("@IdTypeChargeDiverse", ChargDivers.IdTypeChargeDiverse);
                Command.Parameters.AddWithValue("@IdAUEnseignant", ChargDivers.IdAUEnseignant);
                Command.Parameters.AddWithValue("@Periode", ChargDivers.Periode);
                Command.Parameters.AddWithValue("@NatureCharge", ChargDivers.NatureCharge);
                Command.Parameters.AddWithValue("@Volume", ChargDivers.Volume);

                if(string.IsNullOrEmpty(ChargDivers.Observations))
                        Command.Parameters.Add("@Observation", SqlDbType.VarChar).Value = System.DBNull.Value;
                else
                    Command.Parameters.AddWithValue("@Observation", ChargDivers.Observations);
                
                Command.Parameters.AddWithValue("@UniteVolume", ChargDivers.UniteVolume);
                Command.Parameters.AddWithValue("@Id", ChargDivers.Id);
                Command.Parameters.AddWithValue("@NumPeriodeDansAnnee", ChargDivers.NumPeriodeDansAnnee);
                Command.Parameters.AddWithValue("@NbSemainesPeriode", ChargDivers.NbSemainesPeriode);


                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static List<ChargeDiverse> SelectAll()
        {
        string StrSQL = "SELECT * FROM [ChargeDiverse] ";
        var Command = new SqlCommand(StrSQL);

        return Dal_ChargeDiverse.SelectAll(Command);
        }

        public static List<ChargeDiverse> SelectAll(long TeacherId)
        {
            var Command = new SqlCommand();
            Command.CommandText = "SELECT * FROM [ChargeDiverse] WHERE [IdAUEnseignant] = @TeacherId";
            Command.Parameters.AddWithValue("@TeacherId", TeacherId);

            return Dal_ChargeDiverse.SelectAll(Command);
        }

        public static List<ChargeDiverse> SelectAll(SqlCommand Command)
        {
            List<ChargeDiverse> ListeChargeDiverses = new List<ChargeDiverse>();
            ChargeDiverse ChargDivers;

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
                            ChargDivers = new ChargeDiverse();                                               
                            ChargDivers.Id = dr.GetInt64(0);
                            ChargDivers.CodeAnneeUniv = dr.GetString(1);
                            ChargDivers.IdTypeChargeDiverse = dr.GetInt64(2);
                            ChargDivers.IdAUEnseignant = dr.GetInt64(3);

                            ChargDivers.Periode =  dr.GetString(4);
                            ChargDivers.NatureCharge =  dr.GetString(5);
                            ChargDivers.Volume = Convert.ToSingle(dr["Volume"]);
                            ChargDivers.Observations = dr["Observation"] == System.DBNull.Value ? "" : (string)dr["Observation"];
                            ChargDivers.UniteVolume =  dr.GetString(8);
                            ChargDivers.NumPeriodeDansAnnee = dr["NumPeriodeDansAnnee"] == System.DBNull.Value ? 0 : (int)dr["NumPeriodeDansAnnee"];
                            ChargDivers.NbSemainesPeriode = dr["NbSemainesPeriode"] == System.DBNull.Value ? 0 : Convert.ToSingle(dr["NbSemainesPeriode"]);

                            ListeChargeDiverses.Add(ChargDivers);
                        }
                    }
                    return ListeChargeDiverses;
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
    
        public static ChargeDiverse SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [ChargeDiverse] WHERE Id = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_ChargeDiverse.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

        public static ChargeDiverse SelectTheLast()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [ChargeDiverse] WHERE Id = (SELECT MAX(Id) FROM [ChargeDiverse])";
                var Command = new SqlCommand(StrSQL, Cnn);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_ChargeDiverse.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
 
    }
}