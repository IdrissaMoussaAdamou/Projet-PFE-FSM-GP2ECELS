using System;
using System.Collections.Generic;
using Projet_PFE.MyUtilities;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;

namespace Projet_PFE.Dal
{
    public class Dal_TypeChargeDiverse
    {
        private static TypeChargeDiverse GetEntityFromDataRow(DataRow dr)
        {
            TypeChargeDiverse TypeChrgDivers = new TypeChargeDiverse();
            TypeChrgDivers.Id = (Int64)dr["Id"];
            TypeChrgDivers.Libelle = (string)dr["Libelle"];

            return TypeChrgDivers;
        }

        private static List<TypeChargeDiverse> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<TypeChargeDiverse> L = new List<TypeChargeDiverse>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_TypeChargeDiverse.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(TypeChargeDiverse TypeChrgDivers)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [TypeChargeDiverse](Libelle)  values(@Libelle)";
                Command.Parameters.AddWithValue("@Libelle", TypeChrgDivers.Libelle);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static  void Delete(long Id)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
             {
                string StrRequest = "DELETE FROM [TypeChargeDiverse] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest ,Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update(TypeChargeDiverse TypeChrgDivers)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
               
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE [TypeChargeDiverse] SET [Libelle] = @Libelle  WHERE [Id] = @Id";
                
                Command.Parameters.AddWithValue("@Libelle", TypeChrgDivers.Libelle);
                Command.Parameters.AddWithValue("@Id", TypeChrgDivers.Id);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static List<TypeChargeDiverse> SelectAll()
        {
            List<TypeChargeDiverse> ListeTypeChargeDiverses = new List<TypeChargeDiverse>();
            TypeChargeDiverse TypeChrgDivers;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    string StrSQL = "SELECT * FROM [TypeChargeDiverse] ";
                    var Command = new SqlCommand(StrSQL, Cnn);
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            TypeChrgDivers = new TypeChargeDiverse();                                               
                            TypeChrgDivers.Id = dr.GetInt64(0);
                            TypeChrgDivers.Libelle = dr.GetString(1);
                                            
                            ListeTypeChargeDiverses.Add(TypeChrgDivers);
                        }
                    }
                    return ListeTypeChargeDiverses;
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
        public static TypeChargeDiverse SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [TypeChargeDiverse] WHERE Id = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_TypeChargeDiverse.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static TypeChargeDiverse SelectTheLast()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [TypeChargeDiverse] WHERE Id = (SELECT MAX(Id) FROM [TypeChargeDiverse])";
                var Command = new SqlCommand(StrSQL, Cnn);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_TypeChargeDiverse.GetEntityFromDataRow(dt.Rows[0]);
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
                Command.CommandText = "SELECT COUNT(*) FROM " + "["+TableName+"]" + " WHERE [IdTypeChargeDiverse] = @Id";
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