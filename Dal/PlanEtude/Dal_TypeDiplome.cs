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
    public class Dal_TypeDiplome
    {
        private static TypeDiplome GetEntityFromDataRow(DataRow dr)
        {
            TypeDiplome TypeD = new TypeDiplome();
            TypeD.Code = (string)dr["Code"];
            TypeD.IntituleFr = dr["IntituleFr"] == System.DBNull.Value ? "" : (string)dr["IntituleFr"];
            TypeD.IntituleAr = dr["IntituleAr"] == System.DBNull.Value ? "" : (string)dr["IntituleAr"];
            TypeD.IntituleAbrg = dr["IntituleAbrg"] == System.DBNull.Value ? "" : (string)dr["IntituleAbrg"];

            return TypeD;
        }

        private static List<TypeDiplome> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<TypeDiplome> L = new List<TypeDiplome>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_TypeDiplome.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(TypeDiplome TypeD)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if(DataBaseAccessUtilities.CheckKeyUnicity("TypeDiplome","Code",SqlDbType.VarChar,TypeD.Code) == true )
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "INSERT INTO [TypeDiplome](Code,IntituleFr,IntituleAr,IntituleAbrg) VALUES(@Code,@IntituleFr,@IntituleAr,@IntituleAbrg)";
                    Command.Parameters.AddWithValue("@Code",TypeD.Code);
                    Command.Parameters.AddWithValue("@IntituleFr",TypeD.IntituleFr);
                    Command.Parameters.AddWithValue("@IntituleAr",TypeD.IntituleAr);
                    Command.Parameters.AddWithValue("@IntituleAbrg",TypeD.IntituleAbrg);

                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
                else
                {
                    throw new MyException("Erreur dans l'ajout d'un Type de Diplôme ", "Le Code saisi est déja utilisé", "DAL");
                }
            }
        }

        public static int Delete(string Code)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            { 
                string StrRequest = "DELETE FROM [TypeDiplome] WHERE [Code]=@Code";
                var Command = new SqlCommand(StrRequest,Cnn);
                Command.Parameters.AddWithValue("@Code",Code);

                return DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }
        
        public static void Update(string OldCode, TypeDiplome TypeD )
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if((OldCode != TypeD.Code) && (DataBaseAccessUtilities.CheckKeyUnicity("TypeDiplome","Code",SqlDbType.VarChar,TypeD.Code) == false))
                {
                    throw new MyException("Erreur dans la modification d' un Type de Diplôme", "Le nouveau Code est déjà utilisé", "DAL");
                }
                else
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "UPDATE [TypeDiplome] SET [Code]=@Code, [IntituleFr]=@IntituleFr, [IntituleAr]=@IntituleAr, [IntituleAbrg]=@IntituleAbrg WHERE [Code]=@OldCode";
                    Command.Parameters.AddWithValue("@Code",TypeD.Code);
                    Command.Parameters.AddWithValue("@IntituleFr",TypeD.IntituleFr);
                    Command.Parameters.AddWithValue("@IntituleAr",TypeD.IntituleAr);
                    Command.Parameters.AddWithValue("@IntituleAbrg",TypeD.IntituleAbrg);
                    Command.Parameters.AddWithValue("@OldCode",OldCode);
                    
                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
            }
        }

        public static List<TypeDiplome> SelectAll()
        {
            List<TypeDiplome> ListeTypeDiplomes = new List<TypeDiplome>();
            TypeDiplome TypeD;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    string StrSQL = "SELECT * FROM [TypeDiplome] ";
                    var Command = new SqlCommand(StrSQL, Cnn);
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            TypeD = new TypeDiplome();                                               
                            TypeD.Code = dr.GetString(0);
                            TypeD.IntituleFr = dr.GetString(1);
                            TypeD.IntituleAr = dr.GetString(2);
                            TypeD.IntituleAbrg = dr.GetString(3);
                            ListeTypeDiplomes.Add(TypeD);
                        }
                    }
                    return ListeTypeDiplomes;
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

        public static TypeDiplome SelectByCode(string Code)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [TypeDiplome] WHERE Code = @Code";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Code", Code);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_TypeDiplome.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static TypeDiplome SelectByIntituleFr(string IntituleFr)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [TypeDiplome] WHERE IntituleFr = @IntituleFr";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@IntituleFr", IntituleFr);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_TypeDiplome.GetEntityFromDataRow(dt.Rows[0]);
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
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [CodeTypeDiplome] = @Code";  
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


