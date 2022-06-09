using System;
using System.Collections.Generic;
using Projet_PFE.MyUtilities;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;

namespace Projet_PFE.Dal
{
    public class Dal_ChargeEnseignementParPeriode
    {
        private static ChargeEnseignementParPeriode GetEntityFromDataRow(DataRow dr)
        {
            ChargeEnseignementParPeriode ChrgEnParPeriode = new ChargeEnseignementParPeriode();
            ChrgEnParPeriode.Id = (Int64)dr["Id"];
            ChrgEnParPeriode.Periode = (string)dr["Periode"];
            ChrgEnParPeriode.NumPeriodeDansAnnee = (int)dr["NumPeriodeDansAnnee"];
            ChrgEnParPeriode.NbSemainesPeriode = Convert.ToSingle(dr["NbSemainesPeriode"]); 
            ChrgEnParPeriode.UniteVolume = (string)dr["UniteVolume"];
            ChrgEnParPeriode.Grade = (string)dr["Grade"];
            ChrgEnParPeriode.Statut = (string)dr["Statut"];
            ChrgEnParPeriode.VolumeCours = Convert.ToSingle(dr["VolumeCours"]);
            ChrgEnParPeriode.VolumeTD = Convert.ToSingle(dr["VolumeTD"]);
            ChrgEnParPeriode.VolumeTP = Convert.ToSingle(dr["VolumeTP"]);
            ChrgEnParPeriode.VolumeSuppCours = Convert.ToSingle(dr["VolumeCours"]);
            ChrgEnParPeriode.VolumeSuppTD = Convert.ToSingle(dr["VolumeTD"]);
            ChrgEnParPeriode.VolumeSuppTP = Convert.ToSingle(dr["VolumeTP"]);

            ChrgEnParPeriode.CodeAnneeUniv = (string)dr["CodeAnneeUniv"];
            ChrgEnParPeriode.IdAUEnseignant = (Int64)dr["IdAUEnseignant"];

            return ChrgEnParPeriode;
        }

        private static List<ChargeEnseignementParPeriode> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<ChargeEnseignementParPeriode> L = new List<ChargeEnseignementParPeriode>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_ChargeEnseignementParPeriode.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(ChargeEnseignementParPeriode ChrgEnParPeriode)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [ChargeEnseignementParPeriode](Periode, UniteVolume, VolumeCours, VolumeTD, VolumeTP, CodeAnneeUniv, IdAUEnseignant, ";
                Command.CommandText += "NumPeriodeDansAnnee, NbSemainesPeriode, VolumeSuppCours, VolumeSuppTD, VolumeSuppTP, Grade, Statut) values(@Periode, @UniteVolume, ";
                Command.CommandText += "@VolumeCours, @VolumeTD, @VolumeTP, @CodeAnneeUniv, @IdAUEnseignant, @NumPeriodeDansAnnee, @NbSemainesPeriode, @VolumeSuppCours, ";
                Command.CommandText += "@VolumeSuppTD, @VolumeSuppTP, @Grade, @Statut)";
                
                Command.Parameters.AddWithValue("@Periode", ChrgEnParPeriode.Periode);
                Command.Parameters.AddWithValue("@UniteVolume", ChrgEnParPeriode.UniteVolume);
                Command.Parameters.AddWithValue("@VolumeCours", ChrgEnParPeriode.VolumeCours);
                Command.Parameters.AddWithValue("@VolumeTD", ChrgEnParPeriode.VolumeTD);
                Command.Parameters.AddWithValue("@VolumeTP", ChrgEnParPeriode.VolumeTP);
                Command.Parameters.AddWithValue("@CodeAnneeUniv", ChrgEnParPeriode.CodeAnneeUniv);
                Command.Parameters.AddWithValue("@IdAUEnseignant", ChrgEnParPeriode.IdAUEnseignant);
                Command.Parameters.AddWithValue("@NumPeriodeDansAnnee", ChrgEnParPeriode.NumPeriodeDansAnnee);
                Command.Parameters.AddWithValue("@NbSemainesPeriode", ChrgEnParPeriode.NbSemainesPeriode);
                Command.Parameters.AddWithValue("@VolumeSuppCours", ChrgEnParPeriode.VolumeSuppCours);
                Command.Parameters.AddWithValue("@VolumeSuppTD", ChrgEnParPeriode.VolumeSuppTD);
                Command.Parameters.AddWithValue("@VolumeSuppTP", ChrgEnParPeriode.VolumeSuppTP);
                Command.Parameters.AddWithValue("@Grade", ChrgEnParPeriode.Grade);
                Command.Parameters.AddWithValue("@Statut", ChrgEnParPeriode.Statut);
                

                DataBaseAccessUtilities.NonQueryRequest(Command);
            
            }
        }

        public static  void Delete(long Id)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
             {
                string StrRequest = "DELETE FROM [ChargeEnseignementParPeriode] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest ,Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update(ChargeEnseignementParPeriode ChrgEnParPeriode)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
               
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE [ChargeEnseignementParPeriode] SET [Periode] = @Periode, [UniteVolume] = @UniteVolume, [VolumeCours] = @VolumeCours ";
                Command.CommandText += "[VolumeTD] = @VolumeTD, [VolumeTP] = @VolumeTP, [NumPeriodeDansAnnee] = @NumPeriodeDansAnnee, [NbSemainesPeriode] = @NbSemainesPeriode";
                Command.CommandText += "[VolumeSuppCours] = @VolumeSuppCours, [VolumeSuppTD] = @VolumeSuppTD, [VolumeSuppTP] = @VolumeSuppTP, [Grade] = @Grade, ";
                Command.CommandText += "[Statut] = @Statut WHERE [Id] = @Id";
                
                Command.Parameters.AddWithValue("@Periode", ChrgEnParPeriode.Periode);
                Command.Parameters.AddWithValue("@UniteVolume", ChrgEnParPeriode.UniteVolume);
                Command.Parameters.AddWithValue("@VolumeCours", ChrgEnParPeriode.VolumeCours);
                Command.Parameters.AddWithValue("@VolumeTD", ChrgEnParPeriode.VolumeTD);
                Command.Parameters.AddWithValue("@VolumeTP", ChrgEnParPeriode.VolumeTP);
                Command.Parameters.AddWithValue("@NumPeriodeDansAnnee", ChrgEnParPeriode.NumPeriodeDansAnnee);
                Command.Parameters.AddWithValue("@NbSemainesPeriode", ChrgEnParPeriode.NbSemainesPeriode);
                Command.Parameters.AddWithValue("@VolumeSuppCours", ChrgEnParPeriode.VolumeSuppCours);
                Command.Parameters.AddWithValue("@VolumeSuppTD", ChrgEnParPeriode.VolumeSuppTD);
                Command.Parameters.AddWithValue("@VolumeSuppTP", ChrgEnParPeriode.VolumeSuppTP);
                Command.Parameters.AddWithValue("@Grade", ChrgEnParPeriode.Grade);
                Command.Parameters.AddWithValue("@Statut", ChrgEnParPeriode.Statut);
                
                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static List<ChargeEnseignementParPeriode> SelectAll(long TeacherId)
        {
            List<ChargeEnseignementParPeriode> ListeChargeEnseignementParPeriodes = new List<ChargeEnseignementParPeriode>();
            ChargeEnseignementParPeriode ChrgEnParPeriode;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    string StrSQL = "SELECT * FROM [ChargeEnseignementParPeriode] Where [IdAUEnseignant] = @TeacherId";
                    var Command = new SqlCommand(StrSQL, Cnn);
                    Cnn.Open();
                    Command.Parameters.AddWithValue("@TeacherId", TeacherId);
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            ChrgEnParPeriode = new ChargeEnseignementParPeriode();                                               
                            ChrgEnParPeriode.Id = dr.GetInt64(0);
                            ChrgEnParPeriode.Periode = dr.GetString(1);
                            ChrgEnParPeriode.UniteVolume = dr.GetString(2);
                            ChrgEnParPeriode.VolumeCours = Convert.ToSingle(dr["VolumeCours"]);
                            ChrgEnParPeriode.VolumeTD = Convert.ToSingle(dr["VolumeTD"]);
                            ChrgEnParPeriode.VolumeTP = Convert.ToSingle(dr["VolumeTP"]);
                            ChrgEnParPeriode.CodeAnneeUniv = dr.GetString(6);
                            ChrgEnParPeriode.IdAUEnseignant = dr.GetInt64(7);
                            ChrgEnParPeriode.NumPeriodeDansAnnee = dr.GetInt32(8);
                            ChrgEnParPeriode.NbSemainesPeriode = Convert.ToSingle(dr["NbSemainesPeriode"]);
                            ChrgEnParPeriode.VolumeSuppCours = Convert.ToSingle(dr["VolumeSuppCours"]);
                            ChrgEnParPeriode.VolumeSuppTD = Convert.ToSingle(dr["VolumeSuppTD"]);
                            ChrgEnParPeriode.VolumeSuppTP = Convert.ToSingle(dr["VolumeSuppTP"]);
                            ChrgEnParPeriode.Grade = dr.GetString(13);
                            ChrgEnParPeriode.Statut = dr.GetString(14);

                            ListeChargeEnseignementParPeriodes.Add(ChrgEnParPeriode);
                        }
                    }
                    return ListeChargeEnseignementParPeriodes;
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
        public static ChargeEnseignementParPeriode SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [ChargeEnseignementParPeriode] WHERE Id = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_ChargeEnseignementParPeriode.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
 
    }
}