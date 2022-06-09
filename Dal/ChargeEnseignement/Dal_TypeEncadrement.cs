using System;
using System.Collections.Generic;
using Projet_PFE.MyUtilities;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;

namespace Projet_PFE.Dal
{
    public class Dal_TypeEncadrement
    {
        private static TypeEncadrement GetEntityFromDataRow(DataRow dr)
        {
            TypeEncadrement TyEncadrement = new TypeEncadrement();
            TyEncadrement.Id = (Int64)dr["Id"];
            TyEncadrement.Libelle = (string)dr["Libelle"];
            TyEncadrement.Cycle = (string)dr["Cycle"];
            TyEncadrement.NatureCharge = (string)dr["NatureCharge"];
            TyEncadrement.VolumeHebdoCharge = Convert.ToSingle(dr["VolumeHebdoCharge"]);
            TyEncadrement.Periode = (string)dr["Periode"];
            TyEncadrement.NumPeriodeDansAnnee = dr["NumPeriodeDansAnnee"] == System.DBNull.Value? 0 : (int)dr["NumPeriodeDansAnnee"]; 
            TyEncadrement.NbSemainesPeriode = dr["NbSemainesPeriode"] == System.DBNull.Value? 0 : Convert.ToSingle(dr["NbSemainesPeriode"]); 

            return TyEncadrement;
        }

        private static List<TypeEncadrement> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<TypeEncadrement> L = new List<TypeEncadrement>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_TypeEncadrement.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(TypeEncadrement TyEncadrement)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [TypeEncadrement](Libelle, Cycle, NatureCharge, VolumeHebdoCharge, Periode, NumPeriodeDansAnnee, NbSemainesPeriode) ";
                Command.CommandText += "values(@Libelle, @Cycle, @NatureCharge, @VolumeHebdoCharge, @Periode, @NumPeriodeDansAnnee, @NbSemainesPeriode)";

                Command.Parameters.AddWithValue("@Libelle", TyEncadrement.Libelle);
                Command.Parameters.AddWithValue("@Cycle", TyEncadrement.Cycle);
                Command.Parameters.AddWithValue("@NatureCharge", TyEncadrement.NatureCharge);
                Command.Parameters.AddWithValue("@VolumeHebdoCharge", TyEncadrement.VolumeHebdoCharge);
                Command.Parameters.AddWithValue("@Periode", TyEncadrement.Periode);
                Command.Parameters.AddWithValue("@NumPeriodeDansAnnee", TyEncadrement.NumPeriodeDansAnnee);
                Command.Parameters.AddWithValue("@NbSemainesPeriode", TyEncadrement.NbSemainesPeriode);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static  void Delete(long Id)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
             {
                string StrRequest = "DELETE FROM [TypeEncadrement] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest ,Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update(TypeEncadrement TyEncadrement)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
               
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE [TypeEncadrement] SET [Libelle] = @Libelle, [Cycle] = @Cycle, [NatureCharge] = @NatureCharge, ";
                Command.CommandText += "[VolumeHebdoCharge] = @VolumeHebdoCharge, [Periode] = @Periode, [NumPeriodeDansAnnee] = @NumPeriodeDansAnnee, [NbSemainesPeriode] = @NbSemainesPeriode  WHERE [Id] = @Id";
                
                Command.Parameters.AddWithValue("@Libelle", TyEncadrement.Libelle);
                Command.Parameters.AddWithValue("@Cycle", TyEncadrement.Cycle);
                Command.Parameters.AddWithValue("@NatureCharge", TyEncadrement.NatureCharge);
                Command.Parameters.AddWithValue("@VolumeHebdoCharge", TyEncadrement.VolumeHebdoCharge);
                Command.Parameters.AddWithValue("@Periode", TyEncadrement.Periode);
                Command.Parameters.AddWithValue("@NumPeriodeDansAnnee", TyEncadrement.NumPeriodeDansAnnee);
                Command.Parameters.AddWithValue("@NbSemainesPeriode", TyEncadrement.NbSemainesPeriode);
                Command.Parameters.AddWithValue("@Id", TyEncadrement.Id);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static List<TypeEncadrement> SelectAll()
        
        {
            List<TypeEncadrement> ListeTypeEncadrements = new List<TypeEncadrement>();
            TypeEncadrement TyEncadrement;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    string StrSQL = "SELECT * FROM [TypeEncadrement] ";
                    var Command = new SqlCommand(StrSQL, Cnn);
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            TyEncadrement = new TypeEncadrement();                                               
                            TyEncadrement.Id = dr.GetInt64(0);
                            TyEncadrement.Libelle = dr.GetString(1);
                            TyEncadrement.Cycle = dr.GetString(2);
                            TyEncadrement.NatureCharge = dr.GetString(3);
                            TyEncadrement.VolumeHebdoCharge = Convert.ToSingle(dr["VolumeHebdoCharge"]);
                            TyEncadrement.Periode = dr.GetString(5);
                            TyEncadrement.NumPeriodeDansAnnee = dr["NumPeriodeDansAnnee"] == System.DBNull.Value? 0 : (int)dr["NumPeriodeDansAnnee"]; 
                            TyEncadrement.NbSemainesPeriode = dr["NbSemainesPeriode"] == System.DBNull.Value? 0 : Convert.ToSingle(dr["NbSemainesPeriode"]); 


                            ListeTypeEncadrements.Add(TyEncadrement);
                        }
                    }
                    return ListeTypeEncadrements;
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
        public static TypeEncadrement SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [TypeEncadrement] WHERE Id = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_TypeEncadrement.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static TypeEncadrement SelectTheLast()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.CommandText = "SELECT* FROM [TypeEncadrement] WHERE [Id] = (SELECT MAX(Id) FROM [TypeEncadrement])";  
                Command.Connection = Cnn;
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_TypeEncadrement.GetEntityFromDataRow(dt.Rows[0]);
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
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [IdTypeEncadrement] = @Id";  
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