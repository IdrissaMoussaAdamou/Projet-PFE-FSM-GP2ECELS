using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Projet_PFE.Models;
using Projet_PFE;

namespace Projet_PFE.MyUtilities
{
    public class DataBaseAccessUtilities
    {
        public static int NonQueryRequest(SqlCommand MyCommand)
        { 
            try
            {
                try
                {
                    MyCommand.Connection.Open();
                }
                catch (SqlException e)
                {
                    throw new MyException(e,"DataBase Error", "Erreur de connexion à la base", "DAL");
                }

                return MyCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new MyException(e, "DataBase Error", e.Message, "DAL");
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }  
    
        public static int NonQueryRequest(string StrRequest, SqlConnection MyConnection)
        {
            try
            {
                try
                {
                    MyConnection.Open();
                }
                catch (SqlException e)
                {
                    throw new MyException(e, "DataBase Error", "Erreur de connexion à la base", "DAL");
                }

                SqlCommand MyCommand = new SqlCommand(StrRequest, MyConnection);
                return MyCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête : \n", "DAL");
            }
            finally
            {
                MyConnection.Close();
            }

        }
        public static object ScalarRequest(SqlCommand MyCommand)
        {
            try
            {
                try
                {
                    MyCommand.Connection.Open();
                }
                catch (SqlException e)
                {
                    throw new MyException(e, "DataBase Error", "Erreur de connexion à la base", "DAL");
                }

                return MyCommand.ExecuteScalar();
            }
            catch (SqlException e)
            {
                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête : \n", "DAL");
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }

        public static object ScalarRequest(string StrRequest, SqlConnection MyConnection)
        {
            try
            {
                try
                {
                    MyConnection.Open();
                }
                catch (SqlException e)
                {
                    throw new MyException(e, "DataBase Error", "Erreur de connexion à la base", "DAL");
                }
                SqlCommand MyCommand = new SqlCommand(StrRequest, MyConnection);

                return MyCommand.ExecuteScalar();
            }
            catch (SqlException e)
            {
                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête : \n", "DAL");
            }
            finally
            {
                MyConnection.Close();
            }
        }
        public static DataTable DisconnectedSelectRequest(SqlCommand MyCommand)
        {
            try
            {
                DataTable Table;
                SqlDataAdapter SelectAdapter = new SqlDataAdapter(MyCommand);
                Table = new DataTable();
                SelectAdapter.Fill(Table);
                return Table;
            }
            catch (SqlException e)
            {
                //throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");
                throw new MyException(e, "DataBase Errors", e.Message, "DAL");
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }
        public static DataTable DisconnectedSelectRequest(string StrSelectRequest, SqlConnection MyConnection)
        {
            try
            {
                DataTable Table;
                SqlCommand SelectCommand = new SqlCommand(StrSelectRequest, MyConnection);
                SqlDataAdapter SelectAdapter = new SqlDataAdapter(SelectCommand);
                Table = new DataTable();
                SelectAdapter.Fill(Table);
                return Table;
            }
            catch (SqlException e)
            {
                
                throw new MyException(e, "DataBase Error", "Erreur d'éxecution de la requête de sélection : \n", "DAL");
                 
            }
            finally
            {
                MyConnection.Close();
            }
        }

        public static SqlDataReader ConnectedSelectRequest(SqlCommand MyCommand)
        {
            try
            {
                MyCommand.Connection.Open();
                SqlDataReader dr = MyCommand.ExecuteReader();
                return dr;
                
            }
            catch (SqlException e)
            {
                throw new MyException(e, "DataBase Errors", e.Message, "DAL");
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }

        public static bool CheckKeyUnicity(string TableName, string FieldName, SqlDbType FieldType, object FieldValue)
        {    
            using (var Con = new SqlConnection(Config.GetConnectionString()))
            {
                string StrRequest = "SELECT COUNT(" + FieldName + ") FROM " + TableName + " WHERE (" + FieldName + " = @" + FieldName + ")";
                using (var Command = new SqlCommand(StrRequest,Con))
                {                            
                    Command.Parameters.Add( "@"+FieldName, FieldType).Value = FieldValue;       
                    return  !((int)DataBaseAccessUtilities.ScalarRequest(Command) != 0);  
                }
            }
        }
        
        
    }

    public class MyException : Exception
    {

        string _Level;
        string _MyExceptionTitle;
        string _MyExceptionMessage;


        public string Level
        {
            get
            {
                return this._Level;
            }
        }

        public string MyExceptionTitle
        {
            get
            {
                return this._MyExceptionTitle;
            }
        }

        public string MyExceptionMessage
        {
            get
            {
                return this._MyExceptionMessage.ToString();
            }
        }


        public MyException(string MyExceptionTitle, string MyExceptionMessage, string lev)
        {
            this._Level = lev;
            this._MyExceptionTitle = MyExceptionTitle;
            this._MyExceptionMessage = MyExceptionMessage;
        }

        public MyException(Exception e, string MyExceptionTitle, string MyExceptionMessage, string lev) : base(e.Message)
        {
            this._Level = lev;
            this._MyExceptionTitle = MyExceptionTitle;
            this._MyExceptionMessage = MyExceptionMessage;
        }

    }
}