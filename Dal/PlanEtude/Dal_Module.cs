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
   public class Dal_Module
    {
        private static Module GetEntityFromDataRow(DataRow dr)
        {
            Module Course = new Module();
            Course.IdModule = (Int64)dr["IdModule"];
            Course.IdUniteEnseignement = (Int64)dr["IdUniteEnseignement"];
            Course.Code = (string)dr["Code"];
            Course.IntituleFr = dr["IntituleFr"] == System.DBNull.Value ? "" : (string)dr["IntituleFr"];
            Course.IntituleAr = dr["IntituleAr"] == System.DBNull.Value ? "" : (string)dr["IntituleAr"];
            Course.IntituleAbrg= dr["IntituleAbrg"] == System.DBNull.Value ? "" : (string)dr["IntituleAbrg"];
            Course.Credits = Convert.ToSingle(dr["Credits"]);
            Course.Coefficient = Convert.ToSingle(dr["Coefficient"]);
            Course.RegimeExamen = dr["RegimeExamen"] == System.DBNull.Value ? "" : (string)dr["RegimeExamen"];
            Course.UniteVolumeHoraire = dr["UniteVolumeHoraire"] == System.DBNull.Value ? "" : (string)dr["UniteVolumeHoraire"];
            Course.NbHeuresCours = Convert.ToSingle(dr["NbHeuresCours"]);
            Course.NbHeuresTD = Convert.ToSingle(dr["NbHeuresTD"]);
            Course.NbHeuresTP = Convert.ToSingle(dr["NbHeuresTP"]);
            Course.NbHeuresCI = Convert.ToSingle(dr["NbHeuresCI"]);
            Course.Periode = (int)dr["Periode"];
            Course.IdNiveau = (Int64)dr["IdNiveau"];
            Course.IdParcours = (Int64)dr["IdParcours"];
            Course.DureeExamen = dr["DureeExamen"] == System.DBNull.Value ? "" : (string)dr["DureeExamen"];
            Course.Nature = dr["Nature"] == System.DBNull.Value ? "" : (string)dr["Nature"];
            
            Course.IntituleFrNiveau = Dal_Niveau.SelectById(Course.IdNiveau).IntituleFr;
            Course.IntituleFrParcours = Dal_Parcours.SelectById(Course.IdParcours).IntituleFr;
            Course.IntileFrUniteE = Dal_UniteEnseignement.SelectById(Course.IdUniteEnseignement).IntituleFr;
            return Course;
        }
        private static List<Module> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<Module> L = new List<Module>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_Module.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(Module course)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if(DataBaseAccessUtilities.CheckKeyUnicity("Module", "Code", SqlDbType.VarChar, course.Code) == true)
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "INSERT INTO [Module](IdUniteEnseignement,Code,IntituleFr,IntituleAr,IntituleAbrg,Credits,Coefficient,RegimeExamen,UniteVolumeHoraire,NbHeuresCours,NbHeuresTD,";
                    Command.CommandText += "NbHeuresTP,NbHeuresCI,Periode,IdNiveau,IdParcours,DureeExamen,Nature)";
                    Command.CommandText += "VALUES(@IdUniteEnseignement,@Code,@IntituleFr,@IntituleAr,@IntituleAbrg,@Credits,@Coefficient,@RegimeExamen,@UniteVolumeHoraire,@NbHeuresCours,@NbHeuresTD,";
                    Command.CommandText += "@NbHeuresTP,@NbHeuresCI,@Periode,@IdNiveau,@IdParcours,@DureeExamen,@Nature)";
                    
                    Command.Parameters.AddWithValue("@IdUniteEnseignement", course.IdUniteEnseignement);
                    Command.Parameters.AddWithValue("@Code", course.Code);
                    Command.Parameters.AddWithValue("@IntituleFr", course.IntituleFr);
                    Command.Parameters.AddWithValue("@IntituleAr", course.IntituleAr);
                    Command.Parameters.AddWithValue("@IntituleAbrg", course.IntituleAbrg);
                    Command.Parameters.AddWithValue("@Credits", course.Credits);
                    Command.Parameters.AddWithValue("@Coefficient", course.Coefficient);
                    Command.Parameters.AddWithValue("@RegimeExamen", course.RegimeExamen);
                    Command.Parameters.AddWithValue("@UniteVolumeHoraire", course.UniteVolumeHoraire);
                    Command.Parameters.AddWithValue("@NbHeuresCours", course.NbHeuresCours);
                    Command.Parameters.AddWithValue("@NbHeuresTD", course.NbHeuresTD);
                    Command.Parameters.AddWithValue("@NbHeuresTP", course.NbHeuresTP);
                    Command.Parameters.AddWithValue("@NbHeuresCI", course.NbHeuresCI);
                    Command.Parameters.AddWithValue("@Periode", course.Periode);
                    Command.Parameters.AddWithValue("@IdNiveau", course.IdNiveau);
                    Command.Parameters.AddWithValue("@IdParcours", course.IdParcours);
                    Command.Parameters.AddWithValue("@DureeExamen", course.DureeExamen);
                    Command.Parameters.AddWithValue("@Nature", course.Nature);
                    
                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
                else
                {
                    throw new MyException("Erreur dans l'ajout d'un Module", "Le Code saisi est déja utilisé", "DAL");
                }
                
            }
        }
        public static void Delete(long IdModule)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrRequest = "DELETE FROM [Module] WHERE [IdModule]=@IdModule";
                var Command = new SqlCommand(StrRequest,Cnn);
                Command.Parameters.AddWithValue("@IdModule",IdModule);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
            
        }
        public static void Update( string OldCode, Module course)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if((OldCode != course.Code) && (DataBaseAccessUtilities.CheckKeyUnicity("Module", "Code", SqlDbType.VarChar, course.Code) == false))
                {
                    throw new MyException("Erreur dans la modification d'un Module", "Le nouveau Code est déjà utilisé", "DAL");
                }
                else
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "UPDATE  [Module] SET [IdUniteEnseignement]=@IdUniteEnseignement, [Code]=@Code, [IntituleFr]=@IntituleFr, [IntituleAr]=@IntituleAr, ";
                    Command.CommandText += "[IntituleAbrg]=@IntituleAbrg, [Credits]=@Credits, [Coefficient]=@Coefficient, [RegimeExamen]=@RegimeExamen, [UniteVolumeHoraire]=@UniteVolumeHoraire, [NbHeuresCours]=@NbHeuresCours, ";
                    Command.CommandText += "[NbHeuresTD]=@NbHeuresTD, [NbHeuresTP]=@NbHeuresTP, [NbHeuresCI]=@NbHeuresCI, [Periode]=@Periode, [IdNiveau]=@IdNiveau, ";
                    Command.CommandText += "[IdParcours]=@IdParcours, [DureeExamen]=@DureeExamen, [Nature]=@Nature WHERE [Code]=@OldCode";
                    
                    Command.Parameters.AddWithValue("@IdUniteEnseignement", course.IdUniteEnseignement);
                    Command.Parameters.AddWithValue("@Code", course.Code);
                    Command.Parameters.AddWithValue("@IntituleFr", course.IntituleFr);
                    Command.Parameters.AddWithValue("@IntituleAr", course.IntituleAr);
                    Command.Parameters.AddWithValue("@IntituleAbrg", course.IntituleAbrg);
                    Command.Parameters.AddWithValue("@Credits", course.Credits);
                    Command.Parameters.AddWithValue("@Coefficient", course.Coefficient);
                    Command.Parameters.AddWithValue("@RegimeExamen", course.RegimeExamen);
                    Command.Parameters.AddWithValue("@UniteVolumeHoraire", course.UniteVolumeHoraire);
                    Command.Parameters.AddWithValue("@NbHeuresCours", course.NbHeuresCours);
                    Command.Parameters.AddWithValue("@NbHeuresTD", course.NbHeuresTD);
                    Command.Parameters.AddWithValue("@NbHeuresTP", course.NbHeuresTP);
                    Command.Parameters.AddWithValue("@NbHeuresCI", course.NbHeuresCI);
                    Command.Parameters.AddWithValue("@Periode", course.Periode);
                    Command.Parameters.AddWithValue("@IdNiveau", course.IdNiveau);
                    Command.Parameters.AddWithValue("@IdParcours", course.IdParcours);
                    Command.Parameters.AddWithValue("@DureeExamen", course.DureeExamen);
                    Command.Parameters.AddWithValue("@Nature", course.Nature);
                    Command.Parameters.AddWithValue("@OldCode", OldCode);
                    
                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
            }    
        }
        public static List<Module> SelectAll(SqlCommand Command)
        {
            List<Module> ListeCourses = new List<Module>();
            Module Course;

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
                            Course = new Module();                                               
                            Course.IdModule = dr.GetInt64(0);
                            Course.IdUniteEnseignement = dr.GetInt64(1);
                            Course.Code = dr.GetString(2);
                            Course.IntituleFr = dr.GetString(3);
                            Course.IntituleAr = dr.GetString(4);
                            Course.IntituleAbrg= dr.GetString(5);
                            Course.Credits = Convert.ToSingle(dr["Credits"]);
                            Course.Coefficient = Convert.ToSingle(dr["Coefficient"]);
                            Course.RegimeExamen=dr.GetString(8);
                            Course.UniteVolumeHoraire = dr.GetString(9);
                            Course.NbHeuresCours = Convert.ToSingle(dr["NbHeuresCours"]);
                            Course.NbHeuresTD = Convert.ToSingle(dr["NbHeuresTD"]);
                            Course.NbHeuresTP = Convert.ToSingle(dr["NbHeuresTP"]);
                            Course.NbHeuresCI = Convert.ToSingle(dr["NbHeuresCI"]);
                            Course.DureeExamen = dr.GetString(14);
                            Course.Nature = dr.GetString(15);
                            Course.IdParcours = dr.GetInt64(16);
                            Course.IdNiveau = dr.GetInt64(17);
                            Course.Periode = dr.GetInt32(18);
                            
                            Course.IntituleFrNiveau = Dal_Niveau.SelectById(Course.IdNiveau).IntituleFr;
                            Course.IntituleFrParcours = Dal_Parcours.SelectById(Course.IdParcours).IntituleFr;
                            Course.IntileFrUniteE = Dal_UniteEnseignement.SelectById(Course.IdUniteEnseignement).IntituleFr;
                            ListeCourses.Add(Course);
                        }
                    }
                    return ListeCourses;
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
        public static List<Module> SelectAll(long IdNiveau)
        {
            var Command = new SqlCommand();
            Command.CommandText = "SELECT * FROM [Module] WHERE [IdNiveau] = @IdNiveau";
            Command.Parameters.AddWithValue("@IdNiveau", IdNiveau);

            return Dal_Module.SelectAll(Command);
        }
        public static List<Module> SelectParcourscourses(long IdNiveau, long IdParcours, int Periode)
        {
            var Command = new SqlCommand();
            Command.CommandText = "SELECT * FROM [Module] WHERE [IdNiveau] = @IdNiveau AND [IdParcours] = @IdParcours AND [Periode] = @Periode";
            Command.Parameters.Add("@IdNiveau", SqlDbType.BigInt).Value = IdNiveau;
            Command.Parameters.Add("@IdParcours", SqlDbType.BigInt).Value = IdParcours;
            Command.Parameters.Add("@Periode", SqlDbType.Int).Value = Periode;

            return Dal_Module.SelectAll(Command);

        }
        public static Module SelectByCode(string Code)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Module] WHERE Code = @Code";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Code", Code);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Module.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static Module SelectById(long IdModule)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Module] WHERE [IdModule] = @IdModule";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@IdModule", IdModule);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Module.GetEntityFromDataRow(dt.Rows[0]);
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
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [IdModule] = @Id";  
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