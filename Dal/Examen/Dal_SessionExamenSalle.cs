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
    public class Dal_SessionExamenSalle
    {
        #region Default Methods
        public static SessionExamenSalle GetEntityFromDataRow(DataRow dr)
        {
            SessionExamenSalle SessionExamenSalle = new SessionExamenSalle(dr);
            return SessionExamenSalle;
        }

        private static List<SessionExamenSalle> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<SessionExamenSalle> L = new List<SessionExamenSalle>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_SessionExamenSalle.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static SessionExamenSalle SelectByCode(long Id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionExamenSalle] WHERE [Id]=@Id";
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
                return Dal_SessionExamenSalle.GetEntityFromDataRow(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<SessionExamenSalle> SelectAll(long id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionExamenSalle] WHERE [IdSession]=@Id";
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
                return Dal_SessionExamenSalle.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionExamenSalle>();
            }
        }

        public static List<SessionExamenSalle> Select(long idSession, List<long> idSessionExam, List<long> idSessionSalle)
        {
            if (idSession < +0 || idSessionExam == null || idSessionExam.Count <= 0 || idSessionSalle == null || idSessionSalle.Count <= 0)
                return null;

            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = $"SELECT * FROM [SessionExamenSalle] WHERE [IdSession]=@id AND [IdSessionExamen] IN ({string.Join(",", idSessionExam)}) AND [IdSessionSalle] IN ('{string.Join("','", idSessionSalle)}')";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("id", idSession);

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
                return Dal_SessionExamenSalle.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionExamenSalle>();
            }
        }

        public static List<SessionExamenSalle> Select(long idSession, long idSessionExam, List<long> idSessionSalle)
        {
            if (idSession < +0 || idSessionExam < +0 || idSessionSalle == null || idSessionSalle.Count <= 0)
                return null;

            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = $"SELECT * FROM [SessionExamenSalle] WHERE [IdSession]=@id AND [IdSessionExamen]=@idse  AND [IdSessionSalle] IN ('{string.Join("','", idSessionSalle)}')";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("id", idSession);
                    sqlCommand.Parameters.AddWithValue("idse", idSessionExam);

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
                return Dal_SessionExamenSalle.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionExamenSalle>();
            }
        }

        public static string Insert(SessionExamenSalle item)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                //s/*qlConnection.Open();*/
                if (DataBaseAccessUtilities.CheckKeyUnicity("SessionExamenSalle", "Id", SqlDbType.VarChar, item.Id) == true)
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO [SessionExamenSalle] ([IdSession],[IdSessionExamen],[IdSessionSalle])  VALUES (@IdSession,@IdSessionExamen,@IdSessionSalle); ";

                    sqlCommand.Parameters.AddWithValue("IdSession", item.IdSession);
                    sqlCommand.Parameters.AddWithValue("IdSessionExamen", item.IdSessionExamen);
                    sqlCommand.Parameters.AddWithValue("IdSessionSalle", item.IdSessionSalle);
                    //var result = sqlCommand.ExecuteScalar();
                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
                else
                {
                    msg = "Erreur dans l'ajout d'une SessionExamenSalle";
                }
            }
            return msg;
        }
        //public static string Update(SessionExamen item, long Oldcode)
        //{
        //    string msg = "";
        //    using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
        //    {

        //        if ((DataBaseAccessUtilities.CheckKeyUnicity("Session", "Id", SqlDbType.VarChar, item.Id) == false))
        //        {
        //            msg = "Erreur dans la modification d'une Session,la Designation est déjà utilisé";
        //        }
        //        else
        //        {
        //            var sqlCommand = new SqlCommand();
        //            sqlCommand.Connection = sqlConnection;
        //            sqlCommand.CommandText = "UPDATE [Session] SET [AnneeUniversitaire]=@AnneeUniversitaire, [DateBebut]=@DateBebut, [DateFin]=@DateFin, [Designation]=@Designation, [Etat]=@Etat, [Periode]=@Periode, [TypeSession]=@TypeSession WHERE [Id]=@Id";

        //            sqlCommand.Parameters.AddWithValue("Id", Oldcode);
        //            sqlCommand.Parameters.AddWithValue("AnneeUniversitaire", item.AnneeUniversitaire);
        //            sqlCommand.Parameters.AddWithValue("DateBebut", item.DateBebut);
        //            sqlCommand.Parameters.AddWithValue("DateFin", item.DateFin);
        //            sqlCommand.Parameters.AddWithValue("Designation", item.Designation);
        //            sqlCommand.Parameters.AddWithValue("Etat", item.Etat);
        //            sqlCommand.Parameters.AddWithValue("Periode", item.Periode);
        //            sqlCommand.Parameters.AddWithValue("TypeSession", item.TypeSession);

        //            DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
        //        }
        //    }
        //    return msg;
        //}
        public static void Delete(long ids, long idE, long idS)
        {

            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                string query = "DELETE FROM [SessionExamenSalle] WHERE [IdSession]=@IdSession AND [IdSessionExamen]=@IdSessionExamen AND [IdSessionSalle]=@IdSessionSalle ";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("IdSession", ids);
                sqlCommand.Parameters.AddWithValue("IdSessionExamen", idE);
                sqlCommand.Parameters.AddWithValue("IdSessionSalle", idS);
                DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
            }
        }
        #endregion
    }
}
