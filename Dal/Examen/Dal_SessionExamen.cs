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
    public class Dal_SessionExamen
    {
        #region Default Methods
        public static SessionExamen GetEntityFromDataRow(DataRow dr)
        {
            SessionExamen SessionExamen = new SessionExamen(dr);
            return SessionExamen;
        }

        private static List<SessionExamen> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<SessionExamen> L = new List<SessionExamen>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_SessionExamen.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static SessionExamen SelectByCode(long Id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionExamen] WHERE [Id]=@Id";
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
                return Dal_SessionExamen.GetEntityFromDataRow(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<SessionExamen> SelectAll(long id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionExamen] WHERE [IdSession]=@Id";
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
                return Dal_SessionExamen.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionExamen>();
            }
        }
        
        public static List<SessionExamen> Select(long idSession, List<long> idSessionJour, List<string> niveaux)
        {
            if (idSession < +0 || idSessionJour == null || idSessionJour.Count <= 0 || niveaux == null || niveaux.Count <= 0)
                return null;

            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = $"SELECT * FROM [SessionExamen] WHERE [IdSession]=@id AND [IdSessionJour] IN ({string.Join(",", idSessionJour)}) AND [Niveau] IN ('{string.Join("','",niveaux)}')";
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
                return Dal_SessionExamen.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionExamen>();
            }
        }

        public static List<SessionExamen> Select(long idSession, long idSessionJour, long idSessionseance, string niveau)
        {
            if (idSession < +0 || idSessionJour < +0 || idSessionseance < +0)
                return null;

            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = $"SELECT * FROM [SessionExamen]  WHERE [IdSession]=@id AND [IdSessionJour]=@idSessionJour AND [IdSessionSeance]=@idSessionseance AND [Niveau] In (Select [Niveau] FROM [SessionSection] WHERE [Niveaudyplo] = @niveau )";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("id", idSession);
                    sqlCommand.Parameters.AddWithValue("idSessionJour", idSessionJour);
                    sqlCommand.Parameters.AddWithValue("idSessionseance", idSessionseance);
                    sqlCommand.Parameters.AddWithValue("niveau", niveau);

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
                return Dal_SessionExamen.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionExamen>();
            }
        }

        public static string Insert(SessionExamen item)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                //s/*qlConnection.Open();*/
                if (DataBaseAccessUtilities.CheckKeyUnicity("SessionExamen", "Id", SqlDbType.VarChar, item.Id) == true)
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO [SessionExamen] ([CodeFiliere],[CodeModule],[CodeParcours],[HeureDebut],[HeureFin],[IdSession],[IdSessionJour],[IdSessionSeance],[Intitule],[Nature],[NbEtudiants],[Niveau],[numcell],[Periode])  VALUES (@CodeFiliere,@CodeModule,@CodeParcours,@HeureDebut,@HeureFin,@IdSession,@IdSessionJour,@IdSessionSeance,@Intitule,@Nature,@NbEtudiants,@Niveau,@numcell,@Periode); ";

                    sqlCommand.Parameters.AddWithValue("CodeFiliere", item.CodeFiliere);
                    sqlCommand.Parameters.AddWithValue("CodeModule", item.CodeModule);
                    sqlCommand.Parameters.AddWithValue("CodeParcours", item.CodeParcours);
                    sqlCommand.Parameters.AddWithValue("HeureDebut", item.HeureDebut);
                    sqlCommand.Parameters.AddWithValue("HeureFin", item.HeureFin);
                    sqlCommand.Parameters.AddWithValue("IdSession", item.IdSession);
                    sqlCommand.Parameters.AddWithValue("IdSessionJour", item.IdSessionJour);
                    sqlCommand.Parameters.AddWithValue("IdSessionSeance", item.IdSessionSeance);
                    sqlCommand.Parameters.AddWithValue("Intitule", item.Intitule);
                    sqlCommand.Parameters.AddWithValue("Nature", item.Nature);
                    sqlCommand.Parameters.AddWithValue("NbEtudiants", item.NbEtudiants == null ? (object)DBNull.Value : item.NbEtudiants);
                    sqlCommand.Parameters.AddWithValue("Niveau", item.Niveau == null ? (object)DBNull.Value : item.Niveau);
                    sqlCommand.Parameters.AddWithValue("Periode", item.Periode);
                    sqlCommand.Parameters.AddWithValue("numcell", item.numcell);

                    //var result = sqlCommand.ExecuteScalar();
                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
                else
                {
                    msg = "Erreur dans l'ajout d'une SessionExamen";
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
        public static void Delete(long ids, long idj, string sec, long nbc)
        {

            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                string query = "DELETE FROM [SessionExamen] WHERE [IdSession]=@IdSession AND [IdSessionJour]=@IdSessionJour AND [Niveau]=@Niveau AND [numcell]=@numcell";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("IdSession", ids);
                sqlCommand.Parameters.AddWithValue("IdSessionJour", idj);
                sqlCommand.Parameters.AddWithValue("Niveau", sec);
                sqlCommand.Parameters.AddWithValue("numcell", nbc);
                DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
            }
        }
        #endregion
    }
}
