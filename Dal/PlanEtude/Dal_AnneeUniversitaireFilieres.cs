using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Projet_PFE.Models;
using Projet_PFE.MyUtilities;

namespace Projet_PFE.Dal
{
    public class Dal_AnneeUniversitaireFilieres
    {
        
        public static void Add(long IdFiliere, string CodeAnneeUniv)
        {
            using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                    var Command = new SqlCommand();
                    Command.Connection = Cnn;
                    Command.CommandText = "INSERT INTO [AnneeUniversitaireFilieres](IdFiliere,CodeAnneeUniv) VALUES(@IdFiliere,@CodeAnneeUniv)";
                    Command.Parameters.AddWithValue("@IdFiliere", IdFiliere);
                    Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);

                    DataBaseAccessUtilities.NonQueryRequest(Command);
            }
        }

        public static  void Delete(long IdFiliere, string CodeAnneeUniv)
        {
             using (var Cnn = new SqlConnection(Config.GetConnectionString()))
            {
                string StrRequest = "DELETE FROM [AnneeUniversitaireFilieres] WHERE [IdFiliere] = @IdFiliere AND [CodeAnneeUniv] = @CodeAnneeUniv";
                var Command = new SqlCommand(StrRequest,Cnn);
                Command.Parameters.AddWithValue("@IdFiliere", IdFiliere);
                Command.Parameters.AddWithValue("@CodeAnneeUniv", CodeAnneeUniv);

                DataBaseAccessUtilities.NonQueryRequest(Command);
                
            }
        }
    }
}