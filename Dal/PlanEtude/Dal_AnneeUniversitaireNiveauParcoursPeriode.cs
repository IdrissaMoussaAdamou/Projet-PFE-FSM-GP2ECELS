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
    public class Dal_AnneeUniversitaireNiveauParcoursPeriode
    {
        private static AnneeUniversitaireNiveauParcoursPeriode GetEntityFromDataRow(DataRow dr)
        {
            AnneeUniversitaireNiveauParcoursPeriode AnneUniv = new AnneeUniversitaireNiveauParcoursPeriode();
            AnneUniv.Id = (Int64)dr["Id"];
            AnneUniv.CodeAnneeUniv = dr["CodeAnneeUniv"] == System.DBNull.Value ? "" : (string)dr["CodeAnneeUniv"];
            AnneUniv.NbGroupesC = (int)dr["NbGroupesC"];
            AnneUniv.NbGroupesTD = (int)dr["NbGroupesTD"];
            AnneUniv.NbGroupesTP = (int)dr["NbGroupesTP"];
            AnneUniv.NbGroupesCI = (int)dr["NbGroupesCI"];
            AnneUniv.NbEtudiants = (int)dr["NbEtudiants"];
            AnneUniv.IdFiliere = (Int64)dr["IdFiliere"];
            AnneUniv.IdNiveau = (Int64)dr["IdNiveau"];
            AnneUniv.IdParcours = (Int64)dr["IdParcours"];
            AnneUniv.Periode = dr["Periode"] == System.DBNull.Value? 0 : (int)dr["Periode"];
            var Niveau = Dal_Niveau.SelectById(AnneUniv.IdNiveau);
            AnneUniv.IntituleFrNiveau = Niveau.IntituleFr;
            AnneUniv.IntituleAbrgNiveau = Niveau.IntituleAbrg;
            
            AnneUniv.IntituleFrParcours = Dal_Parcours.SelectById(AnneUniv.IdParcours).IntituleFr;
            return AnneUniv;
        }

        private static List<AnneeUniversitaireNiveauParcoursPeriode> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<AnneeUniversitaireNiveauParcoursPeriode> L = new List<AnneeUniversitaireNiveauParcoursPeriode>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_AnneeUniversitaireNiveauParcoursPeriode.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(AnneeUniversitaireNiveauParcoursPeriode AnneUniv)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [AnneeUniversitaireNiveauParcoursPeriode] (NbGroupesC,NbGroupesTD,NbGroupesTP,NbGroupesCI,IdFiliere,NbEtudiants,IdNiveau,IdParcours,CodeAnneeUniv,Periode) ";
                Command.CommandText += "VALUES(@NbGroupesC,@NbGroupesTD,@NbGroupesTP,@NbGroupesCI,@IdFiliere,@NbEtudiants,@IdNiveau,@IdParcours,@CodeAnneeUniv,@Periode)";

                Command.Parameters.AddWithValue("@NbGroupesC", AnneUniv.NbGroupesC);
                Command.Parameters.AddWithValue("@NbGroupesTD", AnneUniv.NbGroupesTD);
                Command.Parameters.AddWithValue("@NbGroupesTP", AnneUniv.NbGroupesTP);
                Command.Parameters.AddWithValue("@NbGroupesCI", AnneUniv.NbGroupesCI);
                Command.Parameters.AddWithValue("@IdFiliere", AnneUniv.IdFiliere);
                Command.Parameters.AddWithValue("@NbEtudiants", AnneUniv.NbEtudiants);
                Command.Parameters.AddWithValue("@IdNiveau", AnneUniv.IdNiveau);
                Command.Parameters.AddWithValue("@IdParcours", AnneUniv.IdParcours);
                Command.Parameters.AddWithValue("@CodeAnneeUniv", AnneUniv.CodeAnneeUniv);
                Command.Parameters.AddWithValue("@Periode", AnneUniv.Periode);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            
            }
        }

        public static  void Delete(long Id)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrRequest = "DELETE FROM [AnneeUniversitaireNiveauParcoursPeriode] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest,Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update( AnneeUniversitaireNiveauParcoursPeriode AnneUniv)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE  [AnneeUniversitaireNiveauParcoursPeriode] SET [NbGroupesC] = @NbGroupesC, [NbGroupesTD] = @NbGroupesTD, [NbGroupesTP] = @NbGroupesTP, ";
                Command.CommandText += "[NbGroupesCI] = @NbGroupesCI, [NbEtudiants] = @NbEtudiants ,[Periode] = @Periode WHERE [Id] = @Id";
                Command.Parameters.AddWithValue("@AnneeUniv",AnneUniv.CodeAnneeUniv);
                Command.Parameters.AddWithValue("@NbGroupesC",AnneUniv.NbGroupesC);
                Command.Parameters.AddWithValue("@NbGroupesTD",AnneUniv.NbGroupesTD);
                Command.Parameters.AddWithValue("@NbGroupesTP",AnneUniv.NbGroupesTP);
                Command.Parameters.AddWithValue("@NbGroupesCI",AnneUniv.NbGroupesCI);
                Command.Parameters.AddWithValue("@NbEtudiants",AnneUniv.NbEtudiants);
                Command.Parameters.AddWithValue("@Periode", AnneUniv.Periode);
                Command.Parameters.AddWithValue("@Id",AnneUniv.Id);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }    
        }

        public static List<AnneeUniversitaireNiveauParcoursPeriode> SelectAll(SqlCommand Command)
        {
            List<AnneeUniversitaireNiveauParcoursPeriode> ListAnUnivNivParcours = new List<AnneeUniversitaireNiveauParcoursPeriode>();
            AnneeUniversitaireNiveauParcoursPeriode AnneUniv;

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
                            AnneUniv = new AnneeUniversitaireNiveauParcoursPeriode();                                               
                            AnneUniv.Id = dr.GetInt64(0);
                            AnneUniv.NbGroupesC = dr.GetInt32(1);
                            AnneUniv.NbGroupesTD = dr.GetInt32(2);
                            AnneUniv.NbGroupesTP = dr.GetInt32(3);
                            AnneUniv.NbGroupesCI = dr.GetInt32(4);
                            AnneUniv.NbEtudiants = dr.GetInt32(5);
                            AnneUniv.CodeAnneeUniv = dr.GetString(6);
                            AnneUniv.Periode = dr["Periode"] == System.DBNull.Value? 0 : dr.GetInt32(7);
                            AnneUniv.IdFiliere = dr.GetInt64(8);
                            AnneUniv.IdParcours = dr.GetInt64(9);
                            AnneUniv.IdNiveau = dr.GetInt64(10);
                            
                            var Niveau = Dal_Niveau.SelectById(AnneUniv.IdNiveau);
                            AnneUniv.IntituleFrNiveau = Niveau.IntituleFr;
                            AnneUniv.IntituleAbrgNiveau = Niveau.IntituleAbrg;
                            
                            AnneUniv.IntituleFrParcours = Dal_Parcours.SelectById(AnneUniv.IdParcours).IntituleFr;
                            ListAnUnivNivParcours.Add(AnneUniv);
                        }
                    }
                    return ListAnUnivNivParcours;
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

        public static List<AnneeUniversitaireNiveauParcoursPeriode> SelectAll(long IdFiliere, string CodeAnneeUniv)
        {
            var Command = new SqlCommand();
            Command.CommandText = "SELECT * FROM [AnneeUniversitaireNiveauParcoursPeriode] WHERE [CodeAnneeUniv] = @CodeAnneeUniv AND [IdFiliere] = @IdFiliere";
            
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);
            Command.Parameters.AddWithValue("@IdFiliere", IdFiliere);

            return Dal_AnneeUniversitaireNiveauParcoursPeriode.SelectAll(Command);
        }

        
        public static AnneeUniversitaireNiveauParcoursPeriode SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [AnneeUniversitaireNiveauParcoursPeriode] WHERE Id = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_AnneeUniversitaireNiveauParcoursPeriode.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

         public static AnneeUniversitaireNiveauParcoursPeriode SelectByAnneeUnivNiv(long IdFiliere, long IdNiveau, long IdParcours, string CodeAnneeUniv)
         {
            using (var Cnn = new SqlConnection(Config.GetConnectionString())) 
            {
                string StrSQL = "SELECT * FROM [AnneeUniversitaireNiveauParcoursPeriode] WHERE [IdFiliere] = @IdFiliere AND [IdNiveau] = @IdNiveau AND ";
                StrSQL +=  "[IdParcours] = @IdParcours AND [CodeAnneeUniv] = @CodeAnneeUniv";
                var Command = new SqlCommand(StrSQL, Cnn);

                Command.Parameters.AddWithValue("@IdFiliere", IdFiliere);
                Command.Parameters.AddWithValue("@IdNiveau", IdNiveau);
                Command.Parameters.AddWithValue("@IdParcours", IdParcours);
                Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_AnneeUniversitaireNiveauParcoursPeriode.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
         }
        
        public static bool CheckEntityUnicity(long IdFiliere, long IdNiveau, long IdParcours, string CodeAnneeUniv)
        {
            int NbOccs = 0;
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT COUNT(*) FROM [AnneeUniversitaireNiveauParcoursPeriode] WHERE [IdFiliere] = @IdFiliere AND [IdNiveau] =@IdNiveau";
                StrSQL += " AND [IdParcours] =@IdParcours AND [CodeAnneeUniv] =@CodeAnneeUniv";
                SqlCommand Cmd = new SqlCommand(StrSQL, Cnn);
                Cmd.Parameters.AddWithValue("@IdFiliere", IdFiliere);
                Cmd.Parameters.AddWithValue("@IdNiveau", IdNiveau);
                Cmd.Parameters.AddWithValue("@IdParcours", IdParcours);
                Cmd.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);

                NbOccs = (int)DataBaseAccessUtilities.ScalarRequest(Cmd);
            }

            if (NbOccs == 0)
                return true;
            else
                return false;
        }
    }
}