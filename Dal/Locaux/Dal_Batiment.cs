using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;
using Projet_PFE.MyUtilities;

namespace Projet_PFE.Dal
{
    public class Dal_Batiment
    {

        public static Batiment GetEntityFromDataRow(DataRow dr)
        {
            Batiment Batiment = new Batiment();
            Batiment.Code = (string)dr["Code"];
            Batiment.Nom = (string)dr["Nom"];
            return Batiment;
        }
        public static List<Batiment> SelectAll()
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [Batiment]";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }
                catch (SqlException e)
                {
                    throw new MyException(e, "DataBase Errors", e.Message, "DAL");
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Batiment(x)).ToList();
            }
            else
            {
                return new List<Batiment>();
            }
        }

        public static string Insert(Batiment item)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                //s/*qlConnection.Open();*/
                if (DataBaseAccessUtilities.CheckKeyUnicity("Batiment", "Code", SqlDbType.VarChar, item.Code) == true)
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO [Batiment] ([Code],[Nom])  VALUES (@Code,@Nom); ";

                    sqlCommand.Parameters.AddWithValue("Code", item.Code);
                    sqlCommand.Parameters.AddWithValue("Nom", item.Nom == null ? (object)DBNull.Value : item.Nom);

                    //var result = sqlCommand.ExecuteScalar();
                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
                else
                {
                    msg = "Erreur dans l'ajout d'un Batiment ,le Code est déjà utilisé";
                }
            }
            return msg;
        }

        public static void Delete(string code)
        {

            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                string query = "DELETE FROM [Batiment] WHERE [Code]=@Code";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Code", string.IsNullOrWhiteSpace(code) ? "" : code);
                DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
            }
        }


        public static Batiment SelectByCode(string code)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [Batiment] WHERE [Code]=@Id";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("Id", string.IsNullOrWhiteSpace(code) ? "" : code);

                    new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }
                catch (SqlException e)
                {
                    throw new MyException(e, "DataBase Errors", e.Message, "DAL");
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            if (dataTable.Rows.Count > 0)
            {
                //return new Salle(dataTable.Rows[0]);
                return Dal_Batiment.GetEntityFromDataRow(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static bool IsForeignKeyInTable(string TableName, string Code)
        {
            int NbOccs = 0;
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.CommandText = "SELECT COUNT(*) FROM " + "[" + TableName + "]" + " WHERE [CodeBatiment] = @Code";
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
