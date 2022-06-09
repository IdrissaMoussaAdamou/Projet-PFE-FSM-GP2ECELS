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
    public class Dal_SessionJour
    {
        #region Default Methods
        public static SessionJour GetEntityFromDataRow(DataRow dr)
        {
            SessionJour SessionJour = new SessionJour(dr);
            return SessionJour;
        }

        private static List<SessionJour> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<SessionJour> L = new List<SessionJour>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_SessionJour.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static SessionJour SelectByCode(long Id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionJour] WHERE [Id]=@Id";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("Id", Id);

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
                return Dal_SessionJour.GetEntityFromDataRow(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }


        public static List<SessionJour> SelectAll(long id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionJour] WHERE [IdSession]=@Id";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("Id", id);

                    new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }
                catch (SqlException e)
                {
                    throw new MyException(e, "DataBase Errors", e.Message, "DAL");
                }
                //finally
                //{
                //    sqlConnection.Close();
                //}
            }

            if (dataTable.Rows.Count > 0)
            {
                //return dataTable.Rows.Cast<DataRow>().Select(x => new Salle(x)).ToList();
                return Dal_SessionJour.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionJour>();
            }
        }
      


        public static string Insert(SessionJour item)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                //s/*qlConnection.Open();*/
                if (DataBaseAccessUtilities.CheckKeyUnicity("SessionJour", "Id", SqlDbType.VarChar, item.Id) == true)
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO [SessionJour] ([IdSession],[Jour])  VALUES (@IdSession,@Jour); ";

                    sqlCommand.Parameters.AddWithValue("IdSession", item.IdSession);
                    sqlCommand.Parameters.AddWithValue("Jour", item.Jour);

                    //var result = sqlCommand.ExecuteScalar();
                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
                else
                {
                    msg = "Erreur dans l'ajout d'une SessionJour ,l'Id est déjà utilisé";
                }
            }
            return msg;
        }
        public static string Update(SessionJour item, long Oldcode)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {

                if ((DataBaseAccessUtilities.CheckKeyUnicity("Session", "Id", SqlDbType.VarChar, item.Id) == false))
                {
                    msg = "Erreur dans la modification d'une SessionJour,l'id est déjà utilisé";
                }
                else
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "UPDATE [SessionJour] SET [IdSession]=@IdSession, [Jour]=@Jour WHERE [Id]=@Id";

                    sqlCommand.Parameters.AddWithValue("Id", Oldcode);
                    sqlCommand.Parameters.AddWithValue("IdSession", item.IdSession);
                    sqlCommand.Parameters.AddWithValue("Jour", item.Jour);

                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
            }
            return msg;
        }
        public static void Delete(long id)
        {

            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                string query = "DELETE FROM[SessionJour] WHERE[Id] = @Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
            }
        }
        #endregion
    }
}
