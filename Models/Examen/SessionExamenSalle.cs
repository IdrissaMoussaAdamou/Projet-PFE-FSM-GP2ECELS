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
    public class SessionExamenSalle
    {
        public long Id { get; set; }
        public long IdSession { get; set; }
        public long IdSessionExamen { get; set; }
        public long IdSessionSalle { get; set; }

        public SessionExamenSalle() { }

        public SessionExamenSalle(DataRow dataRow)
        {
            Id = Convert.ToInt64(dataRow["Id"]);
            IdSession = Convert.ToInt64(dataRow["IdSession"]);
            IdSessionExamen = Convert.ToInt64(dataRow["IdSessionExamen"]);
            IdSessionSalle = Convert.ToInt64(dataRow["IdSessionSalle"]);
        }
    }
}
