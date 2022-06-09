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
    public class Dal_SessionSeance
    {
        #region Default Methods
        public static SessionSeance GetEntityFromDataRow(DataRow dr)
        {
            SessionSeance SessionSeance = new SessionSeance(dr);
            return SessionSeance;
        }

        private static List<SessionSeance> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<SessionSeance> L = new List<SessionSeance>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_SessionSeance.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static SessionSeance SelectByCode(long Id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionSeance] WHERE [Id]=@Id";
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
                return Dal_SessionSeance.GetEntityFromDataRow(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<SessionSeance> SelectAll(long id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionSeance] WHERE [IdSession]=@Id";
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
                return Dal_SessionSeance.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionSeance>();
            }
        }



        public static string Insert(SessionSeance item)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                //s/*qlConnection.Open();*/
                if ((DataBaseAccessUtilities.CheckKeyUnicity("SessionSeance", "Designation", SqlDbType.VarChar, item.Id) == true) &&((DataBaseAccessUtilities.CheckKeyUnicity("SessionSeance", "DesignationAbregee", SqlDbType.VarChar, item.Id) == true)))
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO [SessionSeance] ([Designation],[DesignationAbregee],[HeureDebut],[HeureFin],[IdSession])  VALUES (@Designation,@DesignationAbregee,@HeureDebut,@HeureFin,@IdSession); ";

                    sqlCommand.Parameters.AddWithValue("Designation", item.Designation);
                    sqlCommand.Parameters.AddWithValue("DesignationAbregee", item.DesignationAbregee == null ? (object)DBNull.Value : item.DesignationAbregee);
                    sqlCommand.Parameters.AddWithValue("HeureDebut", item.HeureDebut == null ? (object)DBNull.Value : item.HeureDebut);
                    sqlCommand.Parameters.AddWithValue("HeureFin", item.HeureFin == null ? (object)DBNull.Value : item.HeureFin);
                    sqlCommand.Parameters.AddWithValue("IdSession", item.IdSession);
                    //var result = sqlCommand.ExecuteScalar();
                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
                else
                {
                    msg = "Cette session contient dejà une séance" + item.Designation +"---" +item.DesignationAbregee;
                }
            }
            return msg;
        }

        public static string Update(SessionSeance item, long Oldcode)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {

                if ((DataBaseAccessUtilities.CheckKeyUnicity("SessionSeance", "Designation", SqlDbType.VarChar, item.Id) == false) && ((DataBaseAccessUtilities.CheckKeyUnicity("SessionSeance", "DesignationAbregee", SqlDbType.VarChar, item.Id) == false)))
                {
                    msg = "Erreur dans la modification d'une Seance, la Designation ou la DesignationAbregee est déjà utilisé";
                }
                else
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "UPDATE [SessionSeance] SET [Designation]=@Designation, [DesignationAbregee]=@DesignationAbregee, [HeureDebut]=@HeureDebut, [HeureFin]=@HeureFin, [IdSession]=@IdSession WHERE [Id]=@Id";

                    sqlCommand.Parameters.AddWithValue("Id", Oldcode);
                    sqlCommand.Parameters.AddWithValue("Designation", item.Designation);
                    sqlCommand.Parameters.AddWithValue("DesignationAbregee", item.DesignationAbregee == null ? (object)DBNull.Value : item.DesignationAbregee);
                    sqlCommand.Parameters.AddWithValue("HeureDebut", item.HeureDebut == null ? (object)DBNull.Value : item.HeureDebut);
                    sqlCommand.Parameters.AddWithValue("HeureFin", item.HeureFin == null ? (object)DBNull.Value : item.HeureFin);
                    sqlCommand.Parameters.AddWithValue("IdSession", item.IdSession);

                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
            }
            return msg;
        }

        public static void Delete(long id)
        {

            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                string query = "DELETE FROM [SessionSeance] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
            }
        }
        #endregion
    }
}
