using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.MyUtilities;
using Projet_PFE.Models;

namespace Projet_PFE.Dal
{
    public class Dal_Enseignant
    {
        private static Enseignant GetEntityFromDataRow(DataRow dr)
        {
            Enseignant Enseignant = new Enseignant();
            Enseignant.Id = (Int64)dr["Id"];
            Enseignant.CIN = (string)dr["CIN"];
            Enseignant.Nom = (string)dr["Nom"];
            Enseignant.Prenom = (string)dr["Prenom"];
            Enseignant.Grade = (string)dr["Grade"];
            Enseignant.Statut = (string)dr["Statut"];
            Enseignant.SituationAdministrative = (string)dr["SituationAdministrative"];
            Enseignant.Email1 = (string)dr["Email1"];
            Enseignant.Email2 = dr["Email2"] == System.DBNull.Value ? "" : (string)dr["Email2"];
            Enseignant.Telephone1 = (string)dr["Telephone1"];
            Enseignant.Telephone2 = dr["Telephone2"] == System.DBNull.Value ? "" : (string)dr["Telephone2"];

            Enseignant.CodeDepartement = (string)dr["CodeDepartement"];
            Enseignant.IntituleFrDepartement = Dal_Departement.SelectByCode(Enseignant.CodeDepartement).IntituleFr;

            return Enseignant;
        }

        private static Enseignant GetEntityFromDataRow2(DataRow dr)
        {
            Enseignant Enseignant = new Enseignant();
            Enseignant.CIN = (string)dr["CIN"];
            Enseignant.Nom = (string)dr["Nom"];
            Enseignant.Prenom = (string)dr["Prenom"];
            Enseignant.Grade = (string)dr["Grade"];
            Enseignant.Statut = (string)dr["Statut"];
            Enseignant.SituationAdministrative = (string)dr["SituationAdministrative"];

            Enseignant.CodeDepartement = (string)dr["CodeDepartement"];
            Enseignant.IntituleFrDepartement = Dal_Departement.SelectByCode(Enseignant.CodeDepartement).IntituleFr;

            return Enseignant;
        }
        private static List<Enseignant> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<Enseignant> L = new List<Enseignant>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_Enseignant.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }

        private static List<Enseignant> GetListFromDataTable2(DataTable dt)
        {
            if (dt != null)
            {
                List<Enseignant> L = new List<Enseignant>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_Enseignant.GetEntityFromDataRow2(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(Enseignant Enseignant)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if(DataBaseAccessUtilities.CheckKeyUnicity("Enseignant","CIN",SqlDbType.VarChar,Enseignant.CIN) == true )
                {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "INSERT INTO [Enseignant](CIN,Nom,Prenom,CodeDepartement,Grade,Statut,SituationAdministrative,Email1,Email2,Telephone1,Telephone2) ";
                    Command.CommandText += "VALUES(@CIN,@Nom,@Prenom,@CodeDepartement,@Grade,@Statut,@SituationAdministrative,@Email1,@Email2,@Telephone1,@Telephone2)"; 
                   
                    Command.Parameters.AddWithValue("@CIN", Enseignant.CIN);
                    Command.Parameters.AddWithValue("@Nom", Enseignant.Nom);
                    Command.Parameters.AddWithValue("@Prenom", Enseignant.Prenom);
                    Command.Parameters.AddWithValue("@CodeDepartement", Enseignant.CodeDepartement);
                    Command.Parameters.AddWithValue("@Grade", Enseignant.Grade);
                    Command.Parameters.AddWithValue("@Statut", Enseignant.Statut);
                    Command.Parameters.AddWithValue("@SituationAdministrative", Enseignant.SituationAdministrative);
                    Command.Parameters.AddWithValue("@Email1", Enseignant.Email1);
                    
                    if(string.IsNullOrEmpty(Enseignant.Email2))
                        Command.Parameters.Add("@Email2", SqlDbType.VarChar).Value = System.DBNull.Value;
                    else
                        Command.Parameters.AddWithValue("@Email2", Enseignant.Email2);

                    Command.Parameters.AddWithValue("@Telephone1", Enseignant.Telephone1);

                    if(string.IsNullOrEmpty(Enseignant.Telephone2))
                        Command.Parameters.Add("@Telephone2", SqlDbType.VarChar).Value = System.DBNull.Value;
                    else
                        Command.Parameters.AddWithValue("@Telephone2", Enseignant.Telephone2);


                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }
                else
                {
                    throw new MyException("Erreur dans l'ajout d'un Enseignant", "le CIN est déjà utilisé", "DAL");
                }
            }
        }

        public static  void Delete(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrRequest = "DELETE FROM [Enseignant] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update( string OldCIN, Enseignant Enseignant)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                if((OldCIN != Enseignant.CIN && DataBaseAccessUtilities.CheckKeyUnicity("Enseignant", "CIN",SqlDbType.VarChar, Enseignant.CIN) == false))
                {
                    throw new MyException("Erreur dans la modification  d'un Enseignant", "Le nouveau CIN est déjà utilisé", "DAL");
                }
                else
                {   
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "UPDATE [Enseignant] SET [CIN]=@CIN, [Nom]=@Nom, [Prenom]=@Prenom, [CodeDepartement] = @CodeDepartement, ";
                    Command.CommandText += "[Grade] = @Grade, [Statut] = @Statut, [SituationAdministrative] = @SituationAdministrative, ";
                    Command.CommandText += "[Email1] = @Email1, [Email2] = @Email2, [Telephone1] = @Telephone1, [Telephone2] = @Telephone2 WHERE CIN = @OldCIN";
                    
                    Command.Parameters.AddWithValue("@CIN", Enseignant.CIN);
                    Command.Parameters.AddWithValue("@Nom", Enseignant.Nom);
                    Command.Parameters.AddWithValue("@Prenom", Enseignant.Prenom);
                    Command.Parameters.AddWithValue("@CodeDepartement", Enseignant.CodeDepartement);
                    Command.Parameters.AddWithValue("@Grade", Enseignant.Grade);
                    Command.Parameters.AddWithValue("@Statut", Enseignant.Statut);
                    Command.Parameters.AddWithValue("@SituationAdministrative", Enseignant.SituationAdministrative);
                    Command.Parameters.AddWithValue("@Email1", Enseignant.Email1);

                    if(string.IsNullOrEmpty(Enseignant.Email2))
                        Command.Parameters.Add("@Email2", SqlDbType.VarChar).Value = System.DBNull.Value;
                    else
                        Command.Parameters.AddWithValue("@Email2", Enseignant.Email2);

                    Command.Parameters.AddWithValue("@Telephone1", Enseignant.Telephone1);
                    
                    if(string.IsNullOrEmpty(Enseignant.Telephone2))
                        Command.Parameters.Add("@Telephone2", SqlDbType.VarChar).Value = System.DBNull.Value;
                    else
                        Command.Parameters.AddWithValue("@Telephone2", Enseignant.Telephone2);

                    Command.Parameters.AddWithValue("@OldCIN", OldCIN);
                    DataBaseAccessUtilities.NonQueryRequest(Command);
                }    
            
            }
        }
        public static List<Enseignant> SelectAll(SqlCommand Command)
        {
            List<Enseignant> ListeEnseignant = new List<Enseignant>();
            Enseignant Enseignant;

            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    Command.Connection = Cnn;
                    Cnn.Open();
                    SqlDataReader dr = Command.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            Enseignant = new Enseignant();                                               
                            Enseignant.Id = dr.GetInt64(0);
                            Enseignant.CIN = dr.GetString(1);
                            Enseignant.Nom = dr.GetString(2);
                            Enseignant.Prenom = dr.GetString(3);
                             Enseignant.CodeDepartement = dr.GetString(4);
                            Enseignant.Grade = dr.GetString(5);
                            Enseignant.Statut = dr.GetString(6);
                            Enseignant.SituationAdministrative = dr.GetString(7);
                            Enseignant.Email1 = dr.GetString(8);
                            Enseignant.Email2 = dr["Email2"] == System.DBNull.Value ? "" : (string)dr["Email2"];
                            Enseignant.Telephone1 = dr.GetString(10);
                            Enseignant.Telephone2 = dr["Telephone2"] == System.DBNull.Value ? "" : (string)dr["Telephone2"];

                            Enseignant.IntituleFrDepartement = Dal_Departement.SelectByCode(Enseignant.CodeDepartement).IntituleFr;
                            
                            ListeEnseignant.Add(Enseignant);
                        }
                    }
                    return ListeEnseignant;
                }
                catch (SqlException e)
                {
                    //throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");
                    throw new MyException(e, "DataBase Errors", e.Message, "DAL");
                }
                finally
                {
                    Cnn.Close();
                }

            }

        }
        public static List<Enseignant> SelectAll()
        {
            var Command = new SqlCommand();
            Command.CommandText = "SELECT * FROM [Enseignant] ";

            return Dal_Enseignant.SelectAll(Command);
        }
        public static List<Enseignant> SelectAll(string FieldName, string FielValue, string CodeAnneeUniv)
        {
            var Command = new SqlCommand();
            if(FieldName == "Departement")
                Command.CommandText  = "SELECT * FROM [Enseignant] WHERE [CodeDepartement] = (SELECT [Code] FROM [Departement] WHERE [IntituleFr] = @FielValue)";

            else if(FieldName == "Grade")
                Command.CommandText  = "SELECT * FROM [Enseignant] WHERE [Grade] = @FielValue";
            
            else if(FieldName == "Statut")
                Command.CommandText  = "SELECT * FROM [Enseignant] WHERE [Statut] = @FielValue";

            else if(FieldName == "Nom")
                Command.CommandText  = "SELECT * FROM [Enseignant] WHERE [Nom] LIKE %@FielValue% ";
            
            else
                Command.CommandText  = "SELECT * FROM [Enseignant] WHERE [CIN] = @FielValue";
            

            Command.CommandText  += " AND [CIN] NOT IN (SELECT [CIN] FROM [AnneeUniversitaireEnseignant] WHERE [CodeAnneeUniv] = @CodeAnneeUniv)";
            
            Command.Parameters.AddWithValue("@FielValue", FielValue);
            Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);

            return Dal_Enseignant.SelectAll(Command);            
        }

        public static List<Enseignant> SelectAll(long id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT [CIN],[Nom],[Prenom],[Grade],[Statut],[CodeDepartement],[SituationAdministrative] FROM [Enseignant] except SELECT [CIN],[Nom],[Prenom],[Grade],[Statut],[CodeDepartement],[SituationAdministrative] FROM [SessionSurveillant] where [IdSession]=@IdSession ";
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
                return Dal_Enseignant.GetListFromDataTable2(dataTable);
            }
            else
            {
                return new List<Enseignant>();
            }
        }

        public static List<Enseignant> SelectAll(string FieldName, string FielValue, long ids) {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Config.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();
                    string query;
                    if (FieldName == "Departement") {
                        query = "SELECT [CIN],[Nom],[Prenom],[Grade],[Statut],[CodeDepartement],[SituationAdministrative] FROM [Enseignant] where [CodeDepartement]=(SELECT [Code] FROM [Departement] WHERE [IntituleFr] = @FielValue)  except SELECT [CIN],[Nom],[Prenom],[Grade],[Statut],[CodeDepartement],[SituationAdministrative] FROM [SessionSurveillant] where [IdSession]=@IdSession ";
                    }
                    else if (FieldName == "Grade") {
                        query = "SELECT [CIN],[Nom],[Prenom],[Grade],[Statut],[CodeDepartement],[SituationAdministrative] FROM [Enseignant] where [Grade]=@FielValue  except SELECT [CIN],[Nom],[Prenom],[Grade],[Statut],[CodeDepartement],[SituationAdministrative] FROM [SessionSurveillant] where [IdSession]=@IdSession ";
                         }
                    else if (FieldName == "Statut") {
                        query = "SELECT [CIN],[Nom],[Prenom],[Grade],[Statut],[CodeDepartement],[SituationAdministrative] FROM [Enseignant] where [Statut]=@FielValue  except SELECT [CIN],[Nom],[Prenom],[Grade],[Statut],[CodeDepartement],[SituationAdministrative] FROM [SessionSurveillant] where [IdSession]=@IdSession ";
                    }
                    else
                    {
                        query = "SELECT [CIN],[Nom],[Prenom],[Grade],[Statut],[CodeDepartement],[SituationAdministrative] FROM [Enseignant] except SELECT [CIN],[Nom],[Prenom],[Grade],[Statut],[CodeDepartement],[SituationAdministrative] FROM [SessionSurveillant] where [IdSession]=@IdSession ";
                    }


                            var sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("FielValue", FielValue);
                    sqlCommand.Parameters.AddWithValue("IdSession", ids);

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
                return Dal_Enseignant.GetListFromDataTable2(dataTable);
            }
            else
            {
                return new List<Enseignant>();
            }
        }

        public static Enseignant SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Enseignant] WHERE Id = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Enseignant.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static Enseignant SelectByCIN(string CIN)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [Enseignant] WHERE [CIN] = @CIN";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@CIN", CIN);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_Enseignant.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static bool HasRowInTableInAnneeUnivEnseignant(string CIN)
        {
            int NbOccs = 0;
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.CommandText = "SELECT COUNT(*) FROM [AnneeUniversitaireEnseignant] WHERE [CIN] = @CIN";
                Command.Parameters.AddWithValue("@CIN", CIN);
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

