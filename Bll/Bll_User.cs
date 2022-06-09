using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_User
    {
        public static void Add(RegisterModel User) => Dal_User.Add(User);
        
        public static void Delete(long Id) =>  Dal_User.Delete(Id);
       
        public static void Update(RegisterModel User) => Dal_User.Update(User);
      
        public static List<RegisterModel> SelectAll() => Dal_User.SelectAll();

        public static RegisterModel SelectById(long Id) => Dal_User.SelectById(Id);
        public static RegisterModel SelectByCIN(string CIN) => Dal_User.SelectByCIN(CIN);
        public static RegisterModel SelectByEmail(string Email) => Dal_User.SelectByEmail(Email);

        public static RegisterModel SelectTheLast() => Dal_User.SelectTheLast();
        
        public static RegisterModel Authentificate(string Email, string Password) => Dal_User.Authentificate(Email, Password);

       
        /*public static bool IsForeignKeyInTable(
            string TableName,
            string Code) => Dal_User.IsForeignKeyInTable(TableName, Code);*/
    }
    
}