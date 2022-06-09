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
   public class Dal_UniteEnseignement
    {
        private static UniteEnseignement GetEntityFromDataRow(DataRow dr)
        {
            var Unite = new UniteEnseignement();
            Unite.IdUniteEnseignement = (Int64)dr["IdUniteEnseignement"];
            Unite.Code = (string)dr["Code"];
            Unite.IntituleFr =  dr["IntituleFr"] == System.DBNull.Value ? "" : (string)dr["IntituleFr"];
            Unite.IntituleAr =  dr["IntituleAr"] == System.DBNull.Value ? "" : (string)dr["IntituleAr"];
            Unite.IntituleAbrg =  dr["IntituleAbrg"] == System.DBNull.Value ? "" : (string)dr["IntituleAbrg"];
            Unite.Nature =  dr["Nature"] == System.DBNull.Value ? "" : (string)dr["Nature"];
            Unite.Credits = Convert.ToSingle(dr["Credits"]);
            Unite.Coefficient = Convert.ToSingle(dr["Coefficient"]);
            Unite.IdParcours =(Int64)dr["IdParcours"]; 
            Unite.IdNiveau = (Int64)dr["IdNiveau"]; 
            Unite.Periode =(int)dr["Periode"];
            
            Unite.IntituleFrNiveau = Dal_Niveau.SelectById(Unite.IdNiveau).IntituleFr;
            Unite.IntituleFrParcours = Dal_Parcours.SelectById(Unite.IdParcours).IntituleFr;
            
            return Unite;
        }

        private static List<UniteEnseignement> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<UniteEnseignement> L = new List<UniteEnseignement>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_UniteEnseignement.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }

        public static void Add(UniteEnseignement Unite)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if(DataBaseAccessUtilities.CheckKeyUnicity("UniteEnseignement","Code",SqlDbType.VarChar,Unite.Code) == true)
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "INSERT INTO [UniteEnseignement](Code,IntituleFr,IntituleAr,IntituleAbrg,Nature,Credits,Coefficient,IdNiveau,IdParcours,Periode)";
                    Command.CommandText += "VALUES(@Code,@IntituleFr,@IntituleAr,@IntituleAbrg,@Nature,@Credits,@Coefficient,@IdNiveau,@IdParcours,@Periode)";
                    
                    Command.Parameters.AddWithValue("@Code", Unite.Code);
                    Command.Parameters.AddWithValue("@IntituleFr", Unite.IntituleFr);
                    Command.Parameters.AddWithValue("@IntituleAr", Unite.IntituleAr);
                    Command.Parameters.AddWithValue("@IntituleAbrg", Unite.IntituleAbrg);
                    Command.Parameters.AddWithValue("@Nature", Unite.Nature);
                    Command.Parameters.AddWithValue("@Credits", Unite.Credits);
                    Command.Parameters.AddWithValue("@Coefficient", Unite.Coefficient);
                    Command.Parameters.AddWithValue("@IdNiveau", Unite.IdNiveau);
                    Command.Parameters.AddWithValue("@IdParcours", Unite.IdParcours);
                    Command.Parameters.AddWithValue("@Periode", Unite.Periode);
                    
                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
                else
                {
                    throw new MyException("Erreur dans l'ajout d'une Nouvelle Unité D'Enseignement", "Le Code saisi est déja utilisé", "DAL");
                }
            }

        }

        public static void Delete(long IdUniteEnseignement)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrRequest = "DELETE FROM [UniteEnseignement] WHERE [IdUniteEnseignement]=@IdUniteEnseignement";
                var Command = new SqlCommand(StrRequest, Cnn);
                Command.Parameters.AddWithValue("@IdUniteEnseignement",IdUniteEnseignement);
                
                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }
        
        public static void Update(UniteEnseignement Unite)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if((Unite.OldCode != Unite.Code) && (DataBaseAccessUtilities.CheckKeyUnicity("UniteEnseignement", "Code", SqlDbType.VarChar, Unite.Code) == false))
                {
                    throw new MyException("Erreur dans la modification d'une Unité d'Enseignement", "Le nouveau Code est déjà utilisé", "DAL");
                }
                else
                {    
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "UPDATE [UniteEnseignement] SET [Code] = @Code, [IntituleFr] = @IntituleFr, [IntituleAr] = @IntituleAr, [IntituleAbrg] = @IntituleAbrg,";
                    Command.CommandText += "[Nature] = @Nature, [Credits] = @Credits, [Coefficient] = @Coefficient, [IdNiveau] = @IdNiveau, [IdParcours] = @IdParcours,[Periode]=@Periode ";
                    Command.CommandText += "WHERE [IdUniteEnseignement] = @IdUniteEnseignement";
                    Command.Parameters.AddWithValue("@Code", Unite.Code);
                    Command.Parameters.AddWithValue("@IntituleFr", Unite.IntituleFr);
                    Command.Parameters.AddWithValue("@IntituleAr", Unite.IntituleAr);
                    Command.Parameters.AddWithValue("@IntituleAbrg", Unite.IntituleAbrg);
                    Command.Parameters.AddWithValue("@Nature", Unite.Nature);
                    Command.Parameters.AddWithValue("@Credits", Unite.Credits);
                    Command.Parameters.AddWithValue("@Coefficient", Unite.Coefficient);
                    Command.Parameters.AddWithValue("@IdNiveau", Unite.IdNiveau);
                    Command.Parameters.AddWithValue("@IdParcours", Unite.IdParcours);
                    Command.Parameters.AddWithValue("@Periode", Unite.Periode);
                    Command.Parameters.AddWithValue("@IdUniteEnseignement", Unite.IdUniteEnseignement);
                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
            }
        }

        public static List<UniteEnseignement> SelectParcoursUnites(long IdNiveau, long IdParcours, int Periode)
        {
            List<UniteEnseignement> ListeUnites = new List<UniteEnseignement>();
            UniteEnseignement Unite;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    string StrSQL = "SELECT * FROM [UniteEnseignement] WHERE [IdNiveau] = @IdNiveau AND [IdParcours] = @IdParcours AND [Periode] = @Periode";
                    var Command = new SqlCommand(StrSQL, Cnn);
                    Command.Parameters.Add("@IdNiveau", SqlDbType.BigInt).Value = IdNiveau;
                    Command.Parameters.Add("@IdParcours", SqlDbType.BigInt).Value = IdParcours;
                    Command.Parameters.Add("@Periode", SqlDbType.Int).Value = Periode;
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            Unite = new UniteEnseignement();
                            Unite.IdUniteEnseignement = dr.GetInt64(0);
                            Unite.Code = dr.GetString(1);
                            Unite.IntituleFr =  dr.GetString(2);
                            Unite.IntituleAr =  dr.GetString(3);
                            Unite.IntituleAbrg =  dr.GetString(4);
                            Unite.Nature =  dr.GetString(5);
                            Unite.Credits = Convert.ToSingle(dr["Credits"]);
                            Unite.Coefficient = Convert.ToSingle(dr["Coefficient"]);
                            Unite.IdParcours = dr.GetInt64(8); 
                            Unite.IdNiveau = dr.GetInt64(9);
                            Unite.Periode = dr.GetInt32(10);
                           
                            Unite.IntituleFrNiveau = Dal_Niveau.SelectById(Unite.IdNiveau).IntituleFr;
                            Unite.IntituleFrParcours = Dal_Parcours.SelectById(Unite.IdParcours).IntituleFr;
                            ListeUnites.Add(Unite);
                        }
                    }
                    return ListeUnites;
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

        public static UniteEnseignement SelectByCode(string Code)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [UniteEnseignement] WHERE Code = @Code";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Code", Code);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_UniteEnseignement.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

        public static UniteEnseignement SelectById(long IdUniteEnseignement)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [UniteEnseignement] WHERE IdUniteEnseignement = @IdUniteEnseignement";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@IdUniteEnseignement", IdUniteEnseignement);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_UniteEnseignement.GetEntityFromDataRow(dt.Rows[0]);
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
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [IdUniteEnseignement] = @Id";  
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