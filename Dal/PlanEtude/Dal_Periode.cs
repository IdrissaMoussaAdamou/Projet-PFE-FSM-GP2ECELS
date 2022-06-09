using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;
using Projet_PFE.MyUtilities;

namespace Projet_PFE.Dal
{
   public class Dal_TypePeriode
    {
        private static TypePeriode GetEntityFromDataRow(DataRow dr)
        {
            TypePeriode PerD = new TypePeriode();
            PerD.Code = (string)dr["Code"];
            PerD.IntituleFr = dr["IntituleFr"] == System.DBNull.Value ? "" : (string)dr["IntituleFr"];
            PerD.IntituleAr = dr["IntituleAr"] == System.DBNull.Value ? "" : (string)dr["IntituleAr"];
            PerD.IntituleAbrg = dr["IntituleAbrg"] == System.DBNull.Value ? "" : (string)dr["IntituleAbrg"];
            PerD.Type = dr["Type"] == System.DBNull.Value ? "" : (string)dr["Type"];
            PerD.Duree = Convert.ToSingle(dr["Duree"]);

            return PerD;
        }

        private static List<TypePeriode> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<TypePeriode> L = new List<TypePeriode>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_TypePeriode.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }

        public static void Add(TypePeriode PerD)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if(DataBaseAccessUtilities.CheckKeyUnicity("TypePeriode","Code",SqlDbType.VarChar,PerD.Code) == true )
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "INSERT INTO [TypePeriode](Code,IntituleFr,IntituleAr,IntituleAbrg,Type,Duree) VALUES(@Code,@IntituleFr,@IntituleAr,@IntituleAbrg,@Type,@Duree)";
                    Command.Parameters.AddWithValue("@Code",PerD.Code);
                    Command.Parameters.AddWithValue("@IntituleFr",PerD.IntituleFr);
                    Command.Parameters.AddWithValue("@IntituleAr",PerD.IntituleAr);
                    Command.Parameters.AddWithValue("@IntituleAbrg",PerD.IntituleAbrg);
                    Command.Parameters.AddWithValue("@Type",PerD.Type);
                    Command.Parameters.AddWithValue("@Duree",PerD.Duree);

                     DataBaseAccessUtilities.NonQueryRequest(Command);
                }
                else
                {
                    throw new MyException("Erreur dans l'ajout d'une Période ", "Le Code saisi est déja utilisé", "DAL");
                }
            }
        }

        public static void Delete(string Code)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {   
                string StrRequest = "DELETE FROM [TypePeriode] WHERE [Code]=@Code";
                var Command = new SqlCommand(StrRequest,Cnn);
                Command.Parameters.AddWithValue("@Code",Code);
                
                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }
        
        public static void Update( string OldCode, TypePeriode PerD)
        {
            using(var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if((OldCode != PerD.Code) && DataBaseAccessUtilities.CheckKeyUnicity("TypePeriode", "Code", SqlDbType.VarChar, PerD.Code) == false)
                {

                }
                else
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "UPDATE [TypePeriode] SET [Code]=@Code, [IntituleFr]=@IntituleFr, [IntituleAr]=@IntituleAr,";
                    Command.CommandText +=  "[IntituleAbrg]=@IntituleAbrg, [Type]=@Type, [Duree]=@Duree WHERE Code=@OldCode";

                    Command.Parameters.AddWithValue("@Code", PerD.Code);
                    Command.Parameters.AddWithValue("@IntituleFr", PerD.IntituleFr);
                    Command.Parameters.AddWithValue("@IntituleAr", PerD.IntituleAr);
                    Command.Parameters.AddWithValue("@IntituleAbrg", PerD.IntituleAbrg);
                    Command.Parameters.AddWithValue("@Type", PerD.Type);
                    Command.Parameters.AddWithValue("@Duree", PerD.Duree);
                    Command.Parameters.AddWithValue("@OldCode", OldCode);

                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
            }
        }

        public static List<TypePeriode> SelectAll()
        {
            List<TypePeriode> ListeTypePeriodes = new List<TypePeriode>();
            TypePeriode PerD;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    string StrSQL = "SELECT * FROM [TypePeriode]";
                    var Command = new SqlCommand(StrSQL, Cnn);
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            PerD = new TypePeriode();                                               
                            PerD.Code = dr.GetString(0);
                            PerD.IntituleFr = dr.GetString(1);
                            PerD.IntituleAr = dr.GetString(2);
                            PerD.IntituleAbrg = dr.GetString(3);
                            PerD.Type = dr.GetString(4);
                            PerD.Duree = Convert.ToSingle(dr["Duree"]);
                            ListeTypePeriodes.Add(PerD);
                        }
                    }
                    return ListeTypePeriodes;
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

        public static TypePeriode SelectByCode(string Code)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [TypePeriode] WHERE Code = @Code";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Code", Code);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_TypePeriode.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static TypePeriode SelectByIntituleFr(string IntituleFr)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [TypePeriode] WHERE IntituleFr = @IntituleFr";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@IntituleFr", IntituleFr);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_TypePeriode.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static bool IsForeignKeyInTable(string TableName, string Code)
        {
            int NbOccs = 0;
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [CodeTypePeriode] = @Code";  
                Command.Parameters.AddWithValue("@Code", Code);
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