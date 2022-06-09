using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;
using Projet_PFE.MyUtilities;

namespace Projet_PFE.Dal
{
    public class Dal_User
    {
        private static RegisterModel GetEntityFromDataRow(DataRow dr)
        {
            RegisterModel User = new RegisterModel();
            User.Id = (Int64)dr["Id"];
            User.CIN = dr["CIN"] == System.DBNull.Value ? "" : (string)dr["CIN"];
            User.Nom = dr["Nom"] == System.DBNull.Value ? "" : (string)dr["Nom"];
            User.Prenom = dr["Prenom"] == System.DBNull.Value ? "" : (string)dr["Prenom"];
            User.Email = dr["Email"] == System.DBNull.Value ? "" : (string)dr["Email"];
            User.Profil = dr["Profil"] == System.DBNull.Value ? "" : (string)dr["Profil"];
            User.Affiliation = dr["Affiliation"] == System.DBNull.Value ? "" : (string)dr["Affiliation"];
            return User;
        }

        private static List<RegisterModel> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<RegisterModel> L = new List<RegisterModel>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_User.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(RegisterModel User)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [User](Nom, Prenom, Email, Password, Profil, Affiliation, CIN) VALUES(@Nom, @Prenom, @Email, @Password, @Profil, @Affiliation, @CIN)";
                Command.Parameters.AddWithValue("@Nom", User.Nom);
                Command.Parameters.AddWithValue("@Prenom", User.Prenom);
                Command.Parameters.Add(new SqlParameter("@Email",User.Email.Trim(' ')));

                if(string.IsNullOrEmpty(User.Password))
                    Command.Parameters.Add("@Password", SqlDbType.VarChar).Value = System.DBNull.Value;
                else
                    Command.Parameters.AddWithValue("@Password", User.Password);
                
                if(string.IsNullOrEmpty(User.Profil))
                    Command.Parameters.Add("@Profil", SqlDbType.VarChar).Value = System.DBNull.Value;
                else
                    Command.Parameters.AddWithValue("@Profil", User.Profil);

                if(string.IsNullOrEmpty(User.Affiliation))
                    Command.Parameters.Add("@Affiliation", SqlDbType.VarChar).Value = System.DBNull.Value;
                else
                    Command.Parameters.AddWithValue("@Affiliation", User.Affiliation);
                Command.Parameters.AddWithValue("@CIN", User.CIN.Trim(' '));
                
                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static  void Delete(long Id)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
             {
                string StrRequest = "DELETE FROM [User] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest ,Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update(RegisterModel User)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
               
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                if(string.IsNullOrEmpty(User.Nom) && !string.IsNullOrEmpty(User.Password))
                {
                    Command.CommandText = "UPDATE [User] SET [Password] = @Password WHERE Id = @Id";
                    Command.Parameters.Add(new SqlParameter("@Password", User.Password));
                }

                if(!string.IsNullOrEmpty(User.Nom) && string.IsNullOrEmpty(User.Profil))
                {
                    Command.CommandText = "UPDATE [User] SET [Nom]=@Nom, [Prenom]=@Prenom, [Email] = @Email WHERE Id = @Id";

                    Command.Parameters.Add(new SqlParameter("@Nom", User.Nom));
                    Command.Parameters.Add(new SqlParameter("@Prenom", User.Prenom));
                    Command.Parameters.Add(new SqlParameter("@Email",User.Email.Trim(' ')));
                }

                if(!string.IsNullOrEmpty(User.Nom) && !string.IsNullOrEmpty(User.Profil))
                {
                    Command.CommandText = "UPDATE [User] SET [Nom]=@Nom, [Prenom]=@Prenom, [Email] = @Email, [Profil] = @Profil, [Affiliation] = @Affiliation, @CIN = @CIN WHERE Id = @Id";
                    
                    Command.Parameters.Add(new SqlParameter("@Nom", User.Nom));
                    Command.Parameters.Add(new SqlParameter("@Prenom", User.Prenom));
                    Command.Parameters.Add(new SqlParameter("@Email",User.Email.Trim(' ')));
                    Command.Parameters.Add(new SqlParameter("@Profil", User.Profil));
                    Command.Parameters.Add(new SqlParameter("@Affiliation", User.Affiliation));
                    Command.Parameters.Add(new SqlParameter("@CIN", User.CIN));
                }
                Command.Parameters.Add(new SqlParameter("@Id", User.Id));
                
                DataBaseAccessUtilities.NonQueryRequest(Command);
            
            }
        }

        public static List<RegisterModel> SelectAll(SqlCommand Command)
        {
            List<RegisterModel> ListeRegisterModels = new List<RegisterModel>();
            RegisterModel User;

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
                            User = new RegisterModel();                                               
                            User.Id = dr.GetInt64(0);
                            User.Nom = dr.GetString(1);
                            User.Prenom = dr.GetString(2);
                            User.Email = dr.GetString(3);
                            User.Profil = dr.GetString(5);
                            User.Affiliation = dr.GetString(6);
                            User.CIN = dr.GetString(7);

                            ListeRegisterModels.Add(User);
                        }
                    }
                    return ListeRegisterModels;
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

        public  static List<RegisterModel> SelectAll()
        {
            var Command = new SqlCommand("SELECT * FROM [User]");
            return Dal_User.SelectAll(Command);
        }
        public static RegisterModel SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [User] WHERE [Id] = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_User.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static RegisterModel SelectByCIN(string CIN)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [User] WHERE [CIN] = @CIN";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@CIN", CIN);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_User.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

        public static RegisterModel SelectByEmail(string Email)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [User] WHERE [Email] = @Email";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Email", Email);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_User.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }

        public static RegisterModel SelectTheLast()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [User] WHERE [Id] = (SELECT MAX(Id) FROM [User])";
                var Command = new SqlCommand(StrSQL, Cnn);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_User.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
 
        public static RegisterModel Authentificate(string Email, string Password)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [User] WHERE [Email] = @Email AND [Password] = @Password";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Email", Email);
                Command.Parameters.AddWithValue("@Password", Password);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_User.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
       
       /* public static bool IsForeignKeyInTable(string TableName, string Code)
        {
            int NbOccs = 0;
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [CodeRegisterModel] = @Code";  
                Command.Parameters.AddWithValue("@Code", Code);
                Command.Connection = Cnn;
                NbOccs = (int)DataBaseAccessUtilities.ScalarRequest(Command);
            }

            if (NbOccs == 0)
                return false;
            else
                return true;
        }*/
    }
   
}