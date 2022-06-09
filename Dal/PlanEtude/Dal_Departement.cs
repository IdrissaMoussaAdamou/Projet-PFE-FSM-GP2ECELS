using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;
using Projet_PFE.MyUtilities;

namespace Projet_PFE.Dal
{
    public class Dal_Departement
    {
        private static Departement GetEntityFromDataRow(DataRow dr)
        {
            Departement Dept = new Departement();
            Dept.Code = (string)dr["Code"];
            Dept.IntituleFr = dr["IntituleFr"] == System.DBNull.Value ? "" : (string)dr["IntituleFr"];
            Dept.IntituleAr = dr["IntituleAr"] == System.DBNull.Value ? "" : (string)dr["IntituleAr"];

            return Dept;
        }

        private static List<Departement> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<Departement> L = new List<Departement>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_Departement.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(Departement Dept)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if(DataBaseAccessUtilities.CheckKeyUnicity("Departement","Code",SqlDbType.VarChar,Dept.Code) == true )
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "INSERT INTO [Departement](Code,IntituleFr,IntituleAr) VALUES(@Code,@IntituleFr,@IntituleAr)";
                    Command.Parameters.AddWithValue("@Code", Dept.Code);
                    Command.Parameters.AddWithValue("@IntituleFr", Dept.IntituleFr);
                    Command.Parameters.AddWithValue("@IntituleAr", Dept.IntituleAr);

                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
                else
                {
                    throw new MyException("Erreur dans l'ajout d'un Departement", "le Code est déjà utilisé", "DAL");
                }
            }
        }

        public static  void Delete(string Code)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
             {
                string StrRequest = "DELETE FROM [Departement] WHERE [Code]=@Code";
                var Command = new SqlCommand(StrRequest ,Cnn);
                Command.Parameters.AddWithValue("@Code", Code);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update( string OldCode, Departement Dept)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if ((OldCode != Dept.Code) && DataBaseAccessUtilities.CheckKeyUnicity("Departement", "Code", SqlDbType.VarChar, Dept.Code) == false)
                {
                    throw new MyException("Erreur dans la modification d' un Département", "Le nouveau Code est déjà utilisé", "DAL");
                }
                else
                {   
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "UPDATE [Departement] SET [Code]=@Code, [IntituleFr]=@IntituleFr, [IntituleAr]=@IntituleAr WHERE Code=@OldCode";
                    Command.Parameters.Add(new SqlParameter("@Code", Dept.Code));
                    Command.Parameters.Add(new SqlParameter("@IntituleFr", Dept.IntituleFr));
                    Command.Parameters.Add(new SqlParameter("@IntituleAr", Dept.IntituleAr));
                    Command.Parameters.Add(new SqlParameter("@OldCode", OldCode));
                    
                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }    
            
            }
        }

        public static List<Departement> SelectAll()
        {
            List<Departement> ListeDepartements = new List<Departement>();
            Departement Dept;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    string StrSQL = "SELECT * FROM [Departement] ";
                    var Command = new SqlCommand(StrSQL, Cnn);
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            Dept = new Departement();                                               
                            Dept.Code = dr.GetString(0);
                            Dept.IntituleFr = dr.GetString(1);
                            Dept.IntituleAr = dr.GetString(2);
                            ListeDepartements.Add(Dept);
                        }
                    }
                    return ListeDepartements;
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
        public static Departement SelectByCode(string Code)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Departement] WHERE Code = @Code";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Code", Code);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Departement.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static Departement SelectByIntituleFr(string IntituleFr)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Departement] WHERE IntituleFr = @IntituleFr";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@IntituleFr", IntituleFr);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Departement.GetEntityFromDataRow(dt.Rows[0]);
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
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [CodeDepartement] = @Code";  
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