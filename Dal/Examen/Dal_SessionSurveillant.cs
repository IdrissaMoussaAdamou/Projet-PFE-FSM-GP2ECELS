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
    public class Dal_SessionSurveillant
    {
        #region Default Methods
        public static SessionSurveillant GetEntityFromDataRow(DataRow dr)
        {
            SessionSurveillant SessionSurveillant = new SessionSurveillant(dr);
            return SessionSurveillant;
        }

        private static List<SessionSurveillant> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<SessionSurveillant> L = new List<SessionSurveillant>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_SessionSurveillant.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static SessionSurveillant SelectByCode(long Id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionSurveillant] WHERE [Id]=@Id";
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
                return Dal_SessionSurveillant.GetEntityFromDataRow(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<SessionSurveillant> SelectAll(long id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionSurveillant] WHERE [IdSession]=@Id";
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
                return Dal_SessionSurveillant.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionSurveillant>();
            }
        }



        public static string Insert(SessionSurveillant item)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                //s/*qlConnection.Open();*/
                if (DataBaseAccessUtilities.CheckKeyUnicity("SessionSalle", "Id", SqlDbType.VarChar, item.Id) == true)
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO [SessionSurveillant] ([CIN],[CodeDepartement],[Email1],[Email2],[Grade],[IdEnseignant],[IdSession],[Nom],[Prenom],[SituationAdministrative],[Statut],[Telephone1],[Telephone2])  VALUES (@CIN,@CodeDepartement,@Email1,@Email2,@Grade,@IdEnseignant,@IdSession,@Nom,@Prenom,@SituationAdministrative,@Statut,@Telephone1,@Telephone2); ";


                    sqlCommand.Parameters.AddWithValue("CIN", item.CIN);
                    sqlCommand.Parameters.AddWithValue("CodeDepartement", item.CodeDepartement);
                    sqlCommand.Parameters.AddWithValue("Email1", item.Email1 == null ? (object)DBNull.Value : item.Email1);
                    sqlCommand.Parameters.AddWithValue("Email2", item.Email2 == null ? (object)DBNull.Value : item.Email2);
                    sqlCommand.Parameters.AddWithValue("Grade", item.Grade == null ? (object)DBNull.Value : item.Grade);
                    sqlCommand.Parameters.AddWithValue("IdEnseignant", item.IdEnseignant);
                    sqlCommand.Parameters.AddWithValue("IdSession", item.IdSession);
                    sqlCommand.Parameters.AddWithValue("Nom", item.Nom == null ? (object)DBNull.Value : item.Nom);
                    sqlCommand.Parameters.AddWithValue("Prenom", item.Prenom == null ? (object)DBNull.Value : item.Prenom);
                    sqlCommand.Parameters.AddWithValue("SituationAdministrative", item.SituationAdministrative);
                    sqlCommand.Parameters.AddWithValue("Statut", item.Statut == null ? (object)DBNull.Value : item.Statut);
                    sqlCommand.Parameters.AddWithValue("Telephone1", item.Telephone1 == null ? (object)DBNull.Value : item.Telephone1);
                    sqlCommand.Parameters.AddWithValue("Telephone2", item.Telephone2 == null ? (object)DBNull.Value : item.Telephone2);

                    //var result = sqlCommand.ExecuteScalar();
                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
                else
                {
                    msg = "Cette session contient dejà Ce surveillant:" + item.Nom;
                }
            }
            return msg;
        }

        public static string Update(SessionSurveillant item, string OldCode)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {

                if ((OldCode != item.CIN && DataBaseAccessUtilities.CheckKeyUnicity("SessionSurveillant", "CIN", SqlDbType.VarChar, item.CIN) == false))
                {
                    //msg = "Erreur dans la modification d'une Salle,Le nouveau Code est déjà utilisé";
                }
                else
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "UPDATE [SessionSurveillant] SET [CIN]=@CIN, [CodeDepartement]=@CodeDepartement, [Email1]=@Email1, [Email2]=@Email2, [Grade]=@Grade, [IdEnseignant]=@IdEnseignant, [Nom]=@Nom, [Prenom]=@Prenom, [SituationAdministrative]=@SituationAdministrative, [Statut]=@Statut, [Telephone1]=@Telephone1, [Telephone2]=@Telephone2 WHERE [CIN]=@OldCode";


                    sqlCommand.Parameters.AddWithValue("OldCode", OldCode);
                    sqlCommand.Parameters.AddWithValue("CIN", item.CIN);
                    sqlCommand.Parameters.AddWithValue("CodeDepartement", item.CodeDepartement);
                    sqlCommand.Parameters.AddWithValue("Email1", item.Email1 == null ? (object)DBNull.Value : item.Email1);
                    sqlCommand.Parameters.AddWithValue("Email2", item.Email2 == null ? (object)DBNull.Value : item.Email2);
                    sqlCommand.Parameters.AddWithValue("Grade", item.Grade == null ? (object)DBNull.Value : item.Grade);
                    sqlCommand.Parameters.AddWithValue("IdEnseignant", item.IdEnseignant);
                    sqlCommand.Parameters.AddWithValue("Nom", item.Nom == null ? (object)DBNull.Value : item.Nom);
                    sqlCommand.Parameters.AddWithValue("Prenom", item.Prenom == null ? (object)DBNull.Value : item.Prenom);
                    sqlCommand.Parameters.AddWithValue("SituationAdministrative", item.SituationAdministrative);
                    sqlCommand.Parameters.AddWithValue("Statut", item.Statut == null ? (object)DBNull.Value : item.Statut);
                    sqlCommand.Parameters.AddWithValue("Telephone1", item.Telephone1 == null ? (object)DBNull.Value : item.Telephone1);
                    sqlCommand.Parameters.AddWithValue("Telephone2", item.Telephone2 == null ? (object)DBNull.Value : item.Telephone2);

                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
            }
            return msg;
        }

        public static void Delete(long id)
        {

            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                string query = "DELETE FROM [SessionSurveillant] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
            }
        }
        #endregion
    }
}
