using System;
using System.Collections.Generic;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_ChargeParModule
    {
        public static void Add(ChargeParModule chgEnParModule) =>  Dal_ChargeParModule.Add(chgEnParModule);
           
        public static void Delete(long Id) => Dal_ChargeParModule.Delete(Id);
            
        public static void Update(ChargeParModule chgEnParModule) => Dal_ChargeParModule.Update(chgEnParModule);
            
        public static List<ChargeParModule> SelectAll() => Dal_ChargeParModule.SelectAll();
        public static List<ChargeParModule> SelectAll(long TeacherId) => Dal_ChargeParModule.SelectAll(TeacherId);

        public static ChargeParModule SelectById(long Id) => Dal_ChargeParModule.SelectById(Id);

        public static ChargeParModule SelectTheLast() => Dal_ChargeParModule.SelecttheLast();
             
    }
}