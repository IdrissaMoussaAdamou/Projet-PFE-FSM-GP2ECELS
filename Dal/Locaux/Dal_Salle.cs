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
    public class Dal_Salle
    {
        #region Default Methods
        public static Salle GetEntityFromDataRow(DataRow dr)
        {
            Salle Salle = new Salle();
            Salle.Code = (string)dr["Code"];
            Salle.Type = (string)dr["Type"];
            Salle.CodeBatiment = (string)dr["CodeBatiment"];
            Salle.Etage = (int)dr["Etage"];
            Salle.CapaciteEnseignement = (int)dr["CapaciteEnseignement"];
            Salle.CapaciteExamen = (int)dr["CapaciteExamen"];
            Salle.NbSurveillants = (int)dr["NbSurveillants"];
            Salle.Etat = (string)dr["Etat"];
            return Salle;
        }

        public static Salle GetEntityFromDataRow2(DataRow dr)
        {
            Salle Salle = new Salle();
            Salle.Code = (string)dr["Code"];
            Salle.CapaciteEnseignement = (int)dr["CapaciteEnseignement"];
            Salle.CapaciteExamen = (int)dr["CapaciteExamen"];
            Salle.NbSurveillants = (int)dr["NbSurveillants"];
            Salle.Etat = (string)dr["Etat"];
            return Salle;
        }

        private static List<Salle> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<Salle> L = new List<Salle>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_Salle.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }

        private static List<Salle> GetListFromDataTable2(DataTable dt)
        {
            if (dt != null)
            {
                List<Salle> L = new List<Salle>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_Salle.GetEntityFromDataRow2(dr));
                return L;
            }
            else
                return null;
        }
        public static Salle SelectByCode(string code)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [Salle] WHERE [Code]=@Id";
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
                return Dal_Salle.GetEntityFromDataRow(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        
        public static List<Salle> SelectAll()
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [Salle]";
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
                return Dal_Salle.GetListFromDataTable(dataTable);
            }
            else
            {
                return new List<Salle>();
            }
        }

        public static List<Salle> SelectAll(long id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT [Code],[CapaciteEnseignement],[CapaciteExamen],[NbSurveillants],[Etat] FROM [Salle] except SELECT [CodeSalle],[CapaciteEnseignement],[CapaciteExamen],[NbSurveillants],[Etat] FROM [SessionSalle] where [IdSession]=@IdSession ";
                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("IdSession", id);

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
                return Dal_Salle.GetListFromDataTable2(dataTable);
            }
            else
            {
                return new List<Salle>();
            }
        }

        public static List<Salle> SelectAll(string FieldName, string FielValue, long id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query;
                    if (FieldName == "Batiment")
                    {
                        query = "SELECT [Code],[CapaciteEnseignement],[CapaciteExamen],[NbSurveillants],[Etat] FROM [Salle] where [CodeBatiment]=@FielValue except SELECT [CodeSalle],[CapaciteEnseignement],[CapaciteExamen],[NbSurveillants],[Etat] FROM [SessionSalle] where [IdSession]=@IdSession ";

                    }
                    else if (FieldName == "type")
                        query = "SELECT [Code],[CapaciteEnseignement],[CapaciteExamen],[NbSurveillants],[Etat] FROM [Salle] where [Type]=@FielValue except SELECT [CodeSalle],[CapaciteEnseignement],[CapaciteExamen],[NbSurveillants],[Etat] FROM [SessionSalle] where [IdSession]=@IdSession ";

                    else if (FieldName == "Etat")
                        query = "SELECT [Code],[CapaciteEnseignement],[CapaciteExamen],[NbSurveillants],[Etat] FROM [Salle] where [Etat]=@FielValue except SELECT [CodeSalle],[CapaciteEnseignement],[CapaciteExamen],[NbSurveillants],[Etat] FROM [SessionSalle] where [IdSession]=@IdSession ";
                    else
                        query = "SELECT[Code],[CapaciteEnseignement],[CapaciteExamen],[NbSurveillants],[Etat] FROM [Salle] except SELECT[CodeSalle],[CapaciteEnseignement],[CapaciteExamen],[NbSurveillants],[Etat] FROM[SessionSalle] where[IdSession] = @IdSession ";

                    var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("FielValue", FielValue);
                    sqlCommand.Parameters.AddWithValue("IdSession", id);

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
                return Dal_Salle.GetListFromDataTable2(dataTable);
            }
            else
            {
                return new List<Salle>();
            }
        }


        public static string Insert(Salle item)
        {
            string msg="";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                //s/*qlConnection.Open();*/
                if (DataBaseAccessUtilities.CheckKeyUnicity("Salle", "Code", SqlDbType.VarChar, item.Code) == true)
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO [Salle] ([Code],[CapaciteEnseignement],[CapaciteExamen],[CodeBatiment],[Etage],[Etat],[NbSurveillants],[Type])  VALUES (@Code,@CapaciteEnseignement,@CapaciteExamen,@CodeBatiment,@Etage,@Etat,@NbSurveillants,@Type); ";
                    
                        sqlCommand.Parameters.AddWithValue("Code", item.Code);
                        sqlCommand.Parameters.AddWithValue("CapaciteEnseignement", item.CapaciteEnseignement == null ? (object)DBNull.Value : item.CapaciteEnseignement);
                        sqlCommand.Parameters.AddWithValue("CapaciteExamen", item.CapaciteExamen == null ? (object)DBNull.Value : item.CapaciteExamen);
                        sqlCommand.Parameters.AddWithValue("CodeBatiment", item.CodeBatiment == null ? (object)DBNull.Value : item.CodeBatiment);
                        sqlCommand.Parameters.AddWithValue("Etage", item.Etage == null ? (object)DBNull.Value : item.Etage);
                        sqlCommand.Parameters.AddWithValue("Etat", item.Etat == null ? (object)DBNull.Value : item.Etat);
                        sqlCommand.Parameters.AddWithValue("NbSurveillants", item.NbSurveillants == null ? (object)DBNull.Value : item.NbSurveillants);
                        sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);

                        //var result = sqlCommand.ExecuteScalar();
                        DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
                else
                {
                    msg = "Erreur dans l'ajout d'une Salle ,le Code est déjà utilisé";
                }
            }
            return msg;
        }
        public static string Update(Salle item, string OldCode)
        {
            string msg="";
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
           
                if ((OldCode != item.Code && DataBaseAccessUtilities.CheckKeyUnicity("Salle", "Code", SqlDbType.VarChar, item.Code) == false))
                {
                    msg= "Erreur dans la modification d'une Salle,Le nouveau Code est déjà utilisé";
                }
                else 
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "UPDATE [Salle] SET [Code]=@Code, [CapaciteEnseignement]=@CapaciteEnseignement, [CapaciteExamen]=@CapaciteExamen, [CodeBatiment]=@CodeBatiment, [Etage]=@Etage, [Etat]=@Etat, [NbSurveillants]=@NbSurveillants, [Type]=@Type WHERE [Code]=@OldCode";
             
                    sqlCommand.Parameters.AddWithValue("Code", item.Code);
                    sqlCommand.Parameters.AddWithValue("CapaciteEnseignement", item.CapaciteEnseignement == null ? (object)DBNull.Value : item.CapaciteEnseignement);
                    sqlCommand.Parameters.AddWithValue("CapaciteExamen", item.CapaciteExamen == null ? (object)DBNull.Value : item.CapaciteExamen);
                    sqlCommand.Parameters.AddWithValue("CodeBatiment", item.CodeBatiment == null ? (object)DBNull.Value : item.CodeBatiment);
                    sqlCommand.Parameters.AddWithValue("Etage", item.Etage == null ? (object)DBNull.Value : item.Etage);
                    sqlCommand.Parameters.AddWithValue("Etat", item.Etat == null ? (object)DBNull.Value : item.Etat);
                    sqlCommand.Parameters.AddWithValue("NbSurveillants", item.NbSurveillants == null ? (object)DBNull.Value : item.NbSurveillants);
                    sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);
                    sqlCommand.Parameters.AddWithValue("@OldCode", OldCode);

                    DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
                }
            }
            return msg;
        }
        public static void Delete(string code)
        {
            
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                string query = "DELETE FROM [Salle] WHERE [Code]=@Code";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Code", string.IsNullOrWhiteSpace(code)? "": code);
                DataBaseAccessUtilities.NonQueryRequest(sqlCommand);
            }
        }
        #endregion

        #region Custom Methods



        #endregion
    }
}
