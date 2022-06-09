using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.MyUtilities;
using Projet_PFE.Models;

namespace Projet_PFE.Dal
{
    public class Dal_ChargeParModule
    {
        private static ChargeParModule GetEntityFromDataRow(DataRow dr)
        {
            ChargeParModule ChgEnParModule = new ChargeParModule();
            ChgEnParModule.Id = (Int64)dr["Id"];
            ChgEnParModule.Periode = (string)dr["Periode"];
            ChgEnParModule.TypeCalcul = (string)dr["TypeCalcul"];
            ChgEnParModule.NatureEnseignement = (string)dr["NatureEnseignement"];
            ChgEnParModule.VolumeHebdoParGroupe = Convert.ToSingle(dr["VolumeHebdoParGroupe"]);
            ChgEnParModule.NbGroupes = (int)dr["NbGroupes"];
            ChgEnParModule.VolumeTotal = dr["VolumeTotal"] == System.DBNull.Value? 0 : Convert.ToSingle(dr["VolumeTotal"]);
            ChgEnParModule.NbSemainesPeriode = dr["NbSemainesPeriode"] == System.DBNull.Value? 0 : Convert.ToSingle(dr["NbSemainesPeriode"]);
            ChgEnParModule.NumPeriodeDansAnnee = dr["NumPeriodeDansAnnee"] == System.DBNull.Value ? 0 :  (int)dr["NumPeriodeDansAnnee"];

            ChgEnParModule.IdFiliere = (Int64)dr["IdFiliere"];
            ChgEnParModule.IdNiveau = (Int64)dr["IdNiveau"];
            ChgEnParModule.IdModule = (Int64)dr["IdModule"];
            ChgEnParModule.CodeAnneeUniv = (string)dr["CodeAnneeUniv"];
            ChgEnParModule.IdAUEnseignant = (Int64)dr["IdAUEnseignant"];

            ChgEnParModule.IntituleAbrgNiveau = Dal_Niveau.SelectById(ChgEnParModule.IdNiveau).IntituleAbrg;
            var Course = Dal_Module.SelectById(ChgEnParModule.IdModule);
            
            if(Course.Nature == "Optionnelle")
            {
                var Option = Dal_AnneeUniversitaireNomOption.SelectModuleNomOption(ChgEnParModule.IdModule,ChgEnParModule.CodeAnneeUniv);
                if(Option != null)
                    ChgEnParModule.IntituleFrModule = Option.Intitule;
                else
                    ChgEnParModule.IntituleFrModule = "Option";
            }
            else
            {
                ChgEnParModule.IntituleFrModule = Course.IntituleFr;
            }

            return ChgEnParModule;
        }

        private static List<ChargeParModule> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<ChargeParModule> L = new List<ChargeParModule>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_ChargeParModule.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(ChargeParModule chgEnParModule)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [ChargeParModule](Periode, TypeCalcul, NatureEnseignement, VolumeHebdoParGroupe, NbGroupes, VolumeTotal, ";
                Command.CommandText += "IdFiliere, IdNiveau, IdModule, CodeAnneeUniv, IdAUEnseignant, NumPeriodeDansAnnee, NbSemainesPeriode) values(@Periode, @TypeCalcul, @NatureEnseignement,";
                Command.CommandText += "@VolumeHebdoParGroupe, @NbGroupes, @VolumeTotal, @IdFiliere, @IdNiveau, @IdModule, @CodeAnneeUniv, @IdAUEnseignant, @NumPeriodeDansAnnee, @NbSemainesPeriode)";
                
                Command.Parameters.AddWithValue("@Periode", chgEnParModule.Periode);
                Command.Parameters.AddWithValue("@TypeCalcul", chgEnParModule.TypeCalcul);
                Command.Parameters.AddWithValue("@NatureEnseignement", chgEnParModule.NatureEnseignement);
                Command.Parameters.AddWithValue("@VolumeHebdoParGroupe", chgEnParModule.VolumeHebdoParGroupe);
                Command.Parameters.AddWithValue("@NbGroupes", chgEnParModule.NbGroupes);
                Command.Parameters.AddWithValue("@VolumeTotal", (chgEnParModule.NbGroupes * chgEnParModule.VolumeHebdoParGroupe * chgEnParModule.NbSemainesPeriode));
                Command.Parameters.AddWithValue("@IdFiliere", chgEnParModule.IdFiliere);
                Command.Parameters.AddWithValue("@IdNiveau", chgEnParModule.IdNiveau);
                Command.Parameters.AddWithValue("@IdModule", chgEnParModule.IdModule);
                Command.Parameters.AddWithValue("@CodeAnneeUniv", chgEnParModule.CodeAnneeUniv);
                Command.Parameters.AddWithValue("@IdAUEnseignant", chgEnParModule.IdAUEnseignant);
                Command.Parameters.AddWithValue("@NumPeriodeDansAnnee", chgEnParModule.NumPeriodeDansAnnee);
                Command.Parameters.AddWithValue("@NbSemainesPeriode", chgEnParModule.NbSemainesPeriode);


                DataBaseAccessUtilities.NonQueryRequest(Command);
            
            }
        }

        public static  void Delete(long Id)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
             {
                string StrRequest = "DELETE FROM [ChargeParModule] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest ,Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update(ChargeParModule chgEnParModule)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
               
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE [ChargeParModule] SET [Periode] = @Periode, [TypeCalcul] = @TypeCalcul, [NatureEnseignement] = @NatureEnseignement, ";
                Command.CommandText += "[VolumeHebdoParGroupe] = @VolumeHebdoParGroupe, [NbGroupes] = @NbGroupes, [VolumeTotal] = @VolumeTotal, [IdFiliere] = @IdFiliere, [IdNiveau] = @IdNiveau, ";
                Command.CommandText += "[IdModule] = @IdModule, [NumPeriodeDansAnnee] = @NumPeriodeDansAnnee, [NbSemainesPeriode]= @NbSemainesPeriode  WHERE [Id] = @Id";
                
                Command.Parameters.AddWithValue("@Periode", chgEnParModule.Periode);
                Command.Parameters.AddWithValue("@TypeCalcul", chgEnParModule.TypeCalcul);
                Command.Parameters.AddWithValue("@NatureEnseignement", chgEnParModule.NatureEnseignement);
                Command.Parameters.AddWithValue("@VolumeHebdoParGroupe", chgEnParModule.VolumeHebdoParGroupe);
                Command.Parameters.AddWithValue("@NbGroupes", chgEnParModule.NbGroupes);
                Command.Parameters.AddWithValue("@VolumeTotal", (chgEnParModule.NbGroupes * chgEnParModule.VolumeHebdoParGroupe * chgEnParModule.NbSemainesPeriode));
                Command.Parameters.AddWithValue("@IdFiliere", chgEnParModule.IdFiliere);
                Command.Parameters.AddWithValue("@IdNiveau", chgEnParModule.IdNiveau);
                Command.Parameters.AddWithValue("@IdModule", chgEnParModule.IdModule);
                Command.Parameters.AddWithValue("@NumPeriodeDansAnnee", chgEnParModule.NumPeriodeDansAnnee);
                Command.Parameters.AddWithValue("@NbSemainesPeriode", chgEnParModule.NbSemainesPeriode);
                Command.Parameters.AddWithValue("@Id", chgEnParModule.Id);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static List<ChargeParModule> SelectAll()
        {
            var Command =  new SqlCommand();
            Command.CommandText = "SELECT * FROM [ChargeParModule]";
            
            return Dal_ChargeParModule.SelectAll(Command);
        }
        public static List<ChargeParModule> SelectAll(long TeacherId)
        {
            var Command =  new SqlCommand();
            Command.CommandText = "SELECT * FROM [ChargeParModule] WHERE [IdAUEnseignant] = @TeacherId";
            Command.Parameters.AddWithValue("@TeacherId",TeacherId);

            return Dal_ChargeParModule.SelectAll(Command);
        }
        public static List<ChargeParModule> SelectAll(SqlCommand Command)
        {
            List<ChargeParModule> ListeChargeParModules = new List<ChargeParModule>();
            ChargeParModule ChgEnParModule;

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
                            ChgEnParModule = new ChargeParModule();                                               
                            ChgEnParModule.Id = dr.GetInt64(0);
                            ChgEnParModule.Periode = dr.GetString(1);
                            ChgEnParModule.VolumeHebdoParGroupe = Convert.ToSingle(dr["VolumeHebdoParGroupe"]);
                            ChgEnParModule.NbGroupes = dr.GetInt32(3);
                            ChgEnParModule.VolumeTotal = dr["VolumeTotal"] == System.DBNull.Value? 0 : Convert.ToSingle(dr["VolumeTotal"]);
                    
                            ChgEnParModule.IdFiliere = dr.GetInt64(5);
                            ChgEnParModule.IdNiveau = dr.GetInt64(6);
                            ChgEnParModule.IdModule = dr.GetInt64(7);
                            ChgEnParModule.CodeAnneeUniv = dr.GetString(8);
                            ChgEnParModule.IdAUEnseignant = dr.GetInt64(9);
                            ChgEnParModule.TypeCalcul = dr.GetString(10);
                            ChgEnParModule.NatureEnseignement = dr.GetString(11);
                            ChgEnParModule.NbSemainesPeriode = dr["NbSemainesPeriode"] == System.DBNull.Value? 0 : Convert.ToSingle(dr["NbSemainesPeriode"]);
                            ChgEnParModule.NumPeriodeDansAnnee = dr["NumPeriodeDansAnnee"] == System.DBNull.Value ? 0 :  (int)dr["NumPeriodeDansAnnee"];
                            
                            ChgEnParModule.IntituleAbrgNiveau = Dal_Niveau.SelectById(ChgEnParModule.IdNiveau).IntituleAbrg;
                            var Course = Dal_Module.SelectById(ChgEnParModule.IdModule);
                            
                            if(Course.Nature == "Optionnelle")
                            {
                                var Option = Dal_AnneeUniversitaireNomOption.SelectModuleNomOption(ChgEnParModule.IdModule,ChgEnParModule.CodeAnneeUniv);
                                if(Option != null)
                                    ChgEnParModule.IntituleFrModule = Option.Intitule;
                                else
                                    ChgEnParModule.IntituleFrModule = "Option";
                            }
                            else
                            {
                                ChgEnParModule.IntituleFrModule = Course.IntituleFr;
                            }
                                ListeChargeParModules.Add(ChgEnParModule);
                        }
                    }
                    return ListeChargeParModules;
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
        public static ChargeParModule SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [ChargeParModule] WHERE Id = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_ChargeParModule.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

        public static ChargeParModule SelecttheLast()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [ChargeParModule] WHERE Id = (SELECT MAX(Id) FROM [ChargeParModule])";
                var Command = new SqlCommand(StrSQL, Cnn);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_ChargeParModule.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

    }
}
