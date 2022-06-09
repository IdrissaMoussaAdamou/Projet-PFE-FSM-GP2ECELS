using System;
using System.Collections.Generic;
using Projet_PFE.MyUtilities;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;

namespace Projet_PFE.Dal
{
    public class Dal_ChargeEncadrement
    {
        private static ChargeEncadrement GetEntityFromDataRow(DataRow dr)
        {
            ChargeEncadrement ChrgEncadrement = new ChargeEncadrement();
            ChrgEncadrement.Id = (Int64)dr["Id"];
            ChrgEncadrement.CodeAnneeUniv = (string)dr["CodeAnneeUniv"];
            ChrgEncadrement.IdTypeEncadrement = (Int64)dr["IdTypeEncadrement"];
            ChrgEncadrement.IdAUEnseignant = (Int64)dr["IdAUEnseignant"];
            ChrgEncadrement.NbEncadrements = (int)dr["NbEncadrements"];
            ChrgEncadrement.TypeEncad = Dal_TypeEncadrement.SelectById(ChrgEncadrement.IdTypeEncadrement);

            return ChrgEncadrement;
        }

        private static List<ChargeEncadrement> GetListFromDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<ChargeEncadrement> L = new List<ChargeEncadrement>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                    L.Add(Dal_ChargeEncadrement.GetEntityFromDataRow(dr));
                return L;
            }
            else
                return null;
        }
        public static void Add(ChargeEncadrement ChrgEncadrement)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "INSERT INTO [ChargeEncadrement](CodeAnneeUniv, IdTypeEncadrement, IdAUEnseignant, NbEncadrements) ";
                Command.CommandText += "values(@CodeAnneeUniv, @IdTypeEncadrement, @IdAUEnseignant, @NbEncadrements)";

                Command.Parameters.AddWithValue("@CodeAnneeUniv", ChrgEncadrement.CodeAnneeUniv);
                Command.Parameters.AddWithValue("@IdTypeEncadrement", ChrgEncadrement.IdTypeEncadrement);
                Command.Parameters.AddWithValue("@IdAUEnseignant", ChrgEncadrement.IdAUEnseignant);
                Command.Parameters.AddWithValue("@NbEncadrements", ChrgEncadrement.NbEncadrements);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static  void Delete(long Id)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
             {
                string StrRequest = "DELETE FROM [ChargeEncadrement] WHERE [Id]=@Id";
                var Command = new SqlCommand(StrRequest ,Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                    
                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
        
        public static void Update(ChargeEncadrement ChrgEncadrement)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
               
                var Command = new SqlCommand();
                Command.Connection = Cnn;
                Command.CommandText = "UPDATE [ChargeEncadrement] SET [CodeAnneeUniv] = @CodeAnneeUniv, [IdTypeEncadrement] = @IdTypeEncadrement, [IdAUEnseignant] = @IdAUEnseignant, ";
                Command.CommandText += "[NbEncadrements] = @NbEncadrements WHERE [Id] = @Id";
                
                Command.Parameters.AddWithValue("@CodeAnneeUniv", ChrgEncadrement.CodeAnneeUniv);
                Command.Parameters.AddWithValue("@IdTypeEncadrement", ChrgEncadrement.IdTypeEncadrement);
                Command.Parameters.AddWithValue("@IdAUEnseignant", ChrgEncadrement.IdAUEnseignant);
                Command.Parameters.AddWithValue("@NbEncadrements", ChrgEncadrement.NbEncadrements);
                Command.Parameters.AddWithValue("@Id", ChrgEncadrement.Id);

                DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static List<ChargeEncadrement> SelectAll()
        {
            
            string StrSQL = "SELECT * FROM [ChargeEncadrement] ";
            var Command = new SqlCommand(StrSQL);

            return Dal_ChargeEncadrement.SelectAll(Command);
        }
        public static List<ChargeEncadrement> SelectAll(long TeacherId)
        {
            
            string StrSQL = "SELECT * FROM [ChargeEncadrement] WHERE [IdAUEnseignant] = @TeacherId";
            var Command = new SqlCommand(StrSQL);
            Command.Parameters.AddWithValue("@TeacherId", TeacherId);
            return Dal_ChargeEncadrement.SelectAll(Command);
        }

        public static List<ChargeEncadrement> SelectAll(SqlCommand Command)
        {
            List<ChargeEncadrement> ListeChargeEncadrements = new List<ChargeEncadrement>();
            ChargeEncadrement ChrgEncadrement;

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
                            ChrgEncadrement = new ChargeEncadrement();                                               
                            ChrgEncadrement.Id = dr.GetInt64(0);
                            ChrgEncadrement.CodeAnneeUniv = dr.GetString(1);
                            ChrgEncadrement.IdTypeEncadrement = dr.GetInt64(2);
                            ChrgEncadrement.IdAUEnseignant = dr.GetInt64(3);
                            ChrgEncadrement.NbEncadrements = dr.GetInt32(4);
                            ChrgEncadrement.TypeEncad = Dal_TypeEncadrement.SelectById(ChrgEncadrement.IdTypeEncadrement);
                            
                            ListeChargeEncadrements.Add(ChrgEncadrement);
                        }
                    }
                    return ListeChargeEncadrements;
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
        public static ChargeEncadrement SelectById(long Id)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrSQL = "SELECT * FROM [ChargeEncadrement] WHERE Id = @Id";
                var Command = new SqlCommand(StrSQL, Cnn);
                Command.Parameters.AddWithValue("@Id", Id);
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_ChargeEncadrement.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public static ChargeEncadrement SelectTheLast()
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                var Command = new SqlCommand();
                Command.CommandText = "SELECT * FROM [ChargeEncadrement] WHERE Id = (SELECT MAX(Id) FROM [ChargeEncadrement])";
                Command.Connection = Cnn;
                DataTable dt = DataBaseAccessUtilities.DisconnectedSelectRequest(Command);
                if (dt != null && dt.Rows.Count != 0)
                    return Dal_ChargeEncadrement.GetEntityFromDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
    }
}