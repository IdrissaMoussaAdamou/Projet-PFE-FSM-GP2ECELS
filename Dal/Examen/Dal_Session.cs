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
    public class Dal_Session
    {
        #region Default Methods
        public static Session GetEntityFromDataRow(DataRow dr)
        {
            Session Session = new Session(dr);
            return Session;
        }

        private static List<Session> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<Session> L = new List<Session>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_Session.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static Session SelectByCode(long Id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [Session] WHERE [Id]=@Id";
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
                return Dal_Session.GetEntityFromDataRow(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Session> SelectAll()
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [Session]";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

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
                return Dal_Session.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<Session>();
            }
        }
        //public static List<Salle> SelectAll(string FieldName, string FielValue/*, string CodeAnneeUniv*/)
        //{
        //    var dataTable = new DataTable();
        //    using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
        //    {
        //        try
        //        {
        //            sqlConnection.Open();

        //            string query;

        //            if (FieldName == "Type")
        //                query = "SELECT * FROM [Salle] WHERE [Type] LIKE @FielValue";

        //            else if (FieldName == "CapaciteEnseignement")
        //                query = "SELECT * FROM [Salle] WHERE [CapaciteEnseignement] = @FielValue";

        //            else if (FieldName == "CapaciteExamen")
        //                query = "SELECT * FROM [Salle] WHERE [CapaciteExamen] = %@FielValue% ";

        //            else if (FieldName == "Etat")
        //                query = "SELECT * FROM [Salle] WHERE [Etat] LIKE %@FielValue% ";

        //            else
        //                query = "SELECT * FROM [Salle] WHERE [Code] LIKE @FielValue";


        //            //Command.CommandText += " AND [CIN] NOT IN (SELECT [CIN] FROM [AnneeUniversitaireEnseignant] WHERE [CodeAnneeUniv] = @CodeAnneeUniv)";

        //            //Command.Parameters.AddWithValue("@FielValue", FielValue);
        //            //Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);



        //            var sqlCommand = new SqlCommand(query, sqlConnection);
        //            sqlCommand.Parameters.AddWithValue("@FielValue", FielValue);

        //            new SqlDataAdapter(sqlCommand).Fill(dataTable);
        //        }
        //        catch (SqlException e)
        //        {
        //            throw new MyException(e, "DataBase Errors", e.Message, "DAL");
        //        }
        //        finally
        //        {
        //            sqlConnection.Close();
        //        }
        //    }

        //    if (dataTable.Rows.Count > 0)
        //    {
        //        //return dataTable.Rows.Cast<DataRow>().Select(x => new Salle(x)).ToList();
        //        return Dal_Salle.GetListFromDataTable(dataTable);
        //    }
        //    else
        //    {
        //        return new List<Salle>();
        //    }
        //}


        public static string Insert(Session item)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                //s/*qlConnection.Open();*/
                if (DataBaseAccessUtilities.CheckKeyUnicity("Session", "Designation", SqlDbType.VarChar, item.Designation) == true)
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO [Session] ([AnneeUniversitaire],[DateBebut],[DateFin],[Designation],[Etat],[Periode],[TypeSession])  VALUES (@AnneeUniversitaire,@DateBebut,@DateFin,@Designation,@Etat,@Periode,@TypeSession); ";

                    sqlCommand.Parameters.AddWithValue("AnneeUniversitaire", item.AnneeUniversitaire);
                    sqlCommand.Parameters.AddWithValue("DateBebut", item.DateBebut);
                    sqlCommand.Parameters.AddWithValue("DateFin", item.DateFin);
                    sqlCommand.Parameters.AddWithValue("Designation", item.Designation);
                    sqlCommand.Parameters.AddWithValue("Etat", item.Etat);
                    sqlCommand.Parameters.AddWithValue("Periode", item.Periode);
                    sqlCommand.Parameters.AddWithValue("TypeSession", item.TypeSession);

                    //var result = sqlCommand.ExecuteScalar();
                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
                else
                {
                    msg = "Erreur dans l'ajout d'une Session ,la Designation est déjà utilisé";
                }
            }
            return msg;
        }
        public static string Update(Session item,long Oldcode)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {

                if ((DataBaseAccessUtilities.CheckKeyUnicity("Session", "Id", SqlDbType.VarChar, item.Id) == false))
                {
                    msg = "Erreur dans la modification d'une Session,la Designation est déjà utilisé";
                }
                else
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "UPDATE [Session] SET [AnneeUniversitaire]=@AnneeUniversitaire, [DateBebut]=@DateBebut, [DateFin]=@DateFin, [Designation]=@Designation, [Etat]=@Etat, [Periode]=@Periode, [TypeSession]=@TypeSession WHERE [Id]=@Id";

                    sqlCommand.Parameters.AddWithValue("Id", Oldcode);
                    sqlCommand.Parameters.AddWithValue("AnneeUniversitaire", item.AnneeUniversitaire);
                    sqlCommand.Parameters.AddWithValue("DateBebut", item.DateBebut);
                    sqlCommand.Parameters.AddWithValue("DateFin", item.DateFin);
                    sqlCommand.Parameters.AddWithValue("Designation", item.Designation);
                    sqlCommand.Parameters.AddWithValue("Etat", item.Etat);
                    sqlCommand.Parameters.AddWithValue("Periode", item.Periode);
                    sqlCommand.Parameters.AddWithValue("TypeSession", item.TypeSession);

                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
            }
            return msg;
        }
        public static void Delete(long id)
        {

            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                string query = "DELETE FROM [Session] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
            }
        }
        #endregion
    }
}
