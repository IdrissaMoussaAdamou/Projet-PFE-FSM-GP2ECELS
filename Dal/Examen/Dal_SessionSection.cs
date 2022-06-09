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
    public class Dal_SessionSection
    {
        #region Default Methods
        public static SessionSection GetEntityFromDataRow(DataRow dr)
        {
            SessionSection SessionSection = new SessionSection(dr);
            return SessionSection;
        }

        public static UneSection GetEntityFromDataRow2(DataRow dr)
        {
            UneSection UneSection = new UneSection(dr);
            return UneSection;
        }

        private static List<SessionSection> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<SessionSection> L = new List<SessionSection>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_SessionSection.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }

        private static List<UneSection> GetListFromDataTable2(DataTable dt)
        {
            if (dt != null)
            {
                List<UneSection> L = new List<UneSection>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_SessionSection.GetEntityFromDataRow2(dr));
                return L;
            }
            else
                return null;
        }
        public static SessionSection SelectByCode(long Id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionSection] WHERE [Id]=@Id";
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
                return Dal_SessionSection.GetEntityFromDataRow(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<SessionSection> SelectAll(long id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionSection] WHERE [IdSession]=@Id";
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
                return Dal_SessionSection.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionSection>();
            }
        }
        public static List<SessionSection> SelectByNiveau(long id, string niveau)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [SessionSection] WHERE [IdSession]=@Id " +(string.IsNullOrWhiteSpace(niveau) ? "": "AND[Niveaudyplo] = @niveau");
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("Id", id);
                    if(!string.IsNullOrWhiteSpace(niveau))
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
                //return dataTable.Rows.Cast<DataRow>().Select(x => new Salle(x)).ToList();
                return Dal_SessionSection.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<SessionSection>();
            }
        }

        public static List<UneSection> SelectAll(long ids ,string date)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "select [Filiere].IntituleFr as IntituleFiliere, [Parcours].IntituleFr as IntituleParcours, [Niveau].IntituleAbrg as Niveau, [AnneeUniversitaireNiveauParcoursPeriode].Id, [AnneeUniversitaireNiveauParcoursPeriode].CodeAnneeUniv, [AnneeUniversitaireNiveauParcoursPeriode].NbEtudiants from [AnneeUniversitaireNiveauParcoursPeriode] INNER JOIN [Filiere] ON [Filiere].Id = [AnneeUniversitaireNiveauParcoursPeriode].IdFiliere INNER JOIN [Parcours] On [Parcours].Id =[AnneeUniversitaireNiveauParcoursPeriode].IdParcours INNER JOIN [Niveau] ON [Niveau].Id = [AnneeUniversitaireNiveauParcoursPeriode].IdNiveau where[AnneeUniversitaireNiveauParcoursPeriode].CodeAnneeUniv like @date";
                        //string Q = " except SELECT ";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("date", date);

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
                return Dal_SessionSection.GetListFromDataTable2(dataTable);
            }
            else
            {
                return new List<UneSection>();
            }
        }

        public static string Insert(SessionSection item)
        {
            string msg = "";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                //s/*qlConnection.Open();*/
                if (DataBaseAccessUtilities.CheckKeyUnicity("SessionSection", "Id", SqlDbType.VarChar, item.Id) == true)
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO [SessionSection] ([CodeFiliere],[CodeParcours],[IdSession],[IntituleFiliere],[IntituleFiliereAbrg],[IntituleParcours],[IntituleParcoursAbrg],[NbEtudiants],[Niveau],[Niveaudyplo],[Periode],[TypeDiplome])  VALUES (@CodeFiliere,@CodeParcours,@IdSession,@IntituleFiliere,@IntituleFiliereAbrg,@IntituleParcours,@IntituleParcoursAbrg,@NbEtudiants,@Niveau,@Niveaudyplo,@Periode,@TypeDiplome); ";

                    sqlCommand.Parameters.AddWithValue("CodeFiliere", item.CodeFiliere == null ? (object)DBNull.Value : item.CodeFiliere);
                    sqlCommand.Parameters.AddWithValue("CodeParcours", item.CodeParcours == null ? (object)DBNull.Value : item.CodeParcours);
                    sqlCommand.Parameters.AddWithValue("IdSession", item.IdSession);
                    sqlCommand.Parameters.AddWithValue("IntituleFiliere", item.IntituleFiliere == null ? (object)DBNull.Value : item.IntituleFiliere);
                    sqlCommand.Parameters.AddWithValue("IntituleFiliereAbrg", item.IntituleFiliereAbrg == null ? (object)DBNull.Value : item.IntituleFiliereAbrg);
                    sqlCommand.Parameters.AddWithValue("IntituleParcours", item.IntituleParcours == null ? (object)DBNull.Value : item.IntituleParcours);
                    sqlCommand.Parameters.AddWithValue("IntituleParcoursAbrg", item.IntituleParcoursAbrg == null ? (object)DBNull.Value : item.IntituleParcoursAbrg);
                    sqlCommand.Parameters.AddWithValue("NbEtudiants", item.NbEtudiants == null ? (object)DBNull.Value : item.NbEtudiants);
                    sqlCommand.Parameters.AddWithValue("Niveau", item.Niveau == null ? (object)DBNull.Value : item.Niveau);
                    sqlCommand.Parameters.AddWithValue("Niveaudyplo", item.Niveaudyplo == null ? (object)DBNull.Value : item.Niveaudyplo);
                    sqlCommand.Parameters.AddWithValue("Periode", item.Periode == null ? (object)DBNull.Value : item.Periode);
                    sqlCommand.Parameters.AddWithValue("TypeDiplome", item.TypeDiplome);
                    //var result = sqlCommand.ExecuteScalar();
                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
                else
                {
                    msg = "Cette session contient dejà cette section";
                }
            }
            return msg;
        }

        public static void Delete(long id)
        {

            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                string query = "DELETE FROM [SessionSection] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
            }
        }
        #endregion
    }
}
