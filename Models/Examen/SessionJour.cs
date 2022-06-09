using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Projet_PFE.Models
{
    public class SessionJour
    {
        public long Id { get; set; }
        public long IdSession { get; set; }
        public DateTime Jour { get; set; }

        public SessionJour() { }

        public SessionJour(DataRow dataRow)
        {
            Id = Convert.ToInt64(dataRow["Id"]);
            IdSession = Convert.ToInt64(dataRow["IdSession"]);
            Jour = Convert.ToDateTime(dataRow["Jour"]);
        }
    }
}
