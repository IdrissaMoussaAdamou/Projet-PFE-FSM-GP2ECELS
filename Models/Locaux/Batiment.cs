using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace Projet_PFE.Models
{
    public class Batiment
    {
        public string Code { get; set; }
        public string Nom { get; set; }

        public Batiment() { }

        public Batiment(DataRow dataRow)
        {
            Code = Convert.ToString(dataRow["Code"]);
            Nom = (dataRow["Nom"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nom"]);
        }
    }
}
