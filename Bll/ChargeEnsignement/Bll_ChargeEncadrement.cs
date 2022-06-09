using System.Collections.Generic;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_ChargeEncadrement
    {
        public static void Add(ChargeEncadrement chrgEncadrement) =>  Dal_ChargeEncadrement.Add(chrgEncadrement);
           
        public static void Delete(long Id) => Dal_ChargeEncadrement.Delete(Id);
            
        public static void Update(ChargeEncadrement chrgEncadrement) => Dal_ChargeEncadrement.Update(chrgEncadrement);
            
        public static List<ChargeEncadrement> SelectAll() => Dal_ChargeEncadrement.SelectAll();
        public static List<ChargeEncadrement> SelectAll(long TeacherId) => Dal_ChargeEncadrement.SelectAll(TeacherId);

        public static ChargeEncadrement SelectById(long Id) => Dal_ChargeEncadrement.SelectById(Id);
        public static ChargeEncadrement SelectTheLast() => Dal_ChargeEncadrement.SelectTheLast();

             
    }
}