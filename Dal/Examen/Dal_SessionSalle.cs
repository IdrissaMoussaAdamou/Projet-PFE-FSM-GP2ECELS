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
    public class Dal_SessionSalle
    {
        #region Default Methods
        public static SessionSalle GetEntityFromDataRow(DataRow dr)
        {
            SessionSalle SessionSalle = new SessionSalle(dr);
            return SessionSalle;
        }

        private static List<SessionSalle> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<SessionSalle> L = new List<SessionSalle>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_SessionSalle.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static SessionSalle SelectByCode(long Id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionSalle] WHERE [Id]=@Id";
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
                return Dal_SessionSalle.GetEntityFromDataRow(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<SessionSalle> SelectAll(long id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionSalle] WHERE [IdSession]=@Id";
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
                return Dal_SessionSalle.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionSalle>();
            }
        }



        public static string Insert(SessionSalle item)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                //s/*qlConnection.Open();*/
                if (DataBaseAccessUtilities.CheckKeyUnicity("SessionSalle", "Id", SqlDbType.VarChar, item.Id) == true)
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO [SessionSalle] ([CapaciteEnseignement],[CapaciteExamen],[CodeSalle],[Etat],[IdSession],[NbSurveillants])  VALUES (@CapaciteEnseignement,@CapaciteExamen,@CodeSalle,@Etat,@IdSession,@NbSurveillants); ";

                    sqlCommand.Parameters.AddWithValue("CapaciteEnseignement", item.CapaciteEnseignement == null ? (object)DBNull.Value : item.CapaciteEnseignement);
                    sqlCommand.Parameters.AddWithValue("CapaciteExamen", item.CapaciteExamen == null ? (object)DBNull.Value : item.CapaciteExamen);
                    sqlCommand.Parameters.AddWithValue("CodeSalle", item.CodeSalle);
                    sqlCommand.Parameters.AddWithValue("Etat", item.Etat == null ? (object)DBNull.Value : item.Etat);
                    sqlCommand.Parameters.AddWithValue("IdSession", item.IdSession);
                    sqlCommand.Parameters.AddWithValue("NbSurveillants", item.NbSurveillants == null ? (object)DBNull.Value : item.NbSurveillants);

                    //var result = sqlCommand.ExecuteScalar();
                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
                else
                {
                    msg = "Cette session contient dejà la salle de code"+ item.CodeSalle;
                }
            }
            return msg;
        }

        public static string Update(SessionSalle item, string OldCode)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {

                if ((OldCode != item.CodeSalle && DataBaseAccessUtilities.CheckKeyUnicity("SessionSalle", "CodeSalle", SqlDbType.VarChar, item.CodeSalle) == false))
                {
                    msg = "Erreur dans la modification d'une Salle,Le nouveau Code est déjà utilisé";
                }
                else
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "UPDATE [SessionSalle] SET [CapaciteEnseignement]=@CapaciteEnseignement, [CapaciteExamen]=@CapaciteExamen, [CodeSalle]=@CodeSalle, [Etat]=@Etat, [NbSurveillants]=@NbSurveillants WHERE [CodeSalle]=@OldCode";

                    sqlCommand.Parameters.AddWithValue("OldCode", OldCode);
                    sqlCommand.Parameters.AddWithValue("CapaciteEnseignement", item.CapaciteEnseignement == null ? (object)DBNull.Value : item.CapaciteEnseignement);
                    sqlCommand.Parameters.AddWithValue("CapaciteExamen", item.CapaciteExamen == null ? (object)DBNull.Value : item.CapaciteExamen);
                    sqlCommand.Parameters.AddWithValue("CodeSalle", item.CodeSalle);
                    sqlCommand.Parameters.AddWithValue("Etat", item.Etat == null ? (object)DBNull.Value : item.Etat);
                    sqlCommand.Parameters.AddWithValue("NbSurveillants", item.NbSurveillants == null ? (object)DBNull.Value : item.NbSurveillants);

                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
            }
            return msg;
        }

        public static void Delete(long id)
        {

            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                string query = "DELETE FROM [SessionSalle] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
            }
        }
        #endregion
    }
}
