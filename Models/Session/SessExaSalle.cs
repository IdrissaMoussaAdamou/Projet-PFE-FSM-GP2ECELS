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
    public class SessExaSalle
    {
        public List<BonLSE> LSE { get; set; }
        public List<SessionSalle> LSS { get; set; }
        public List<SessionExamenSalle> LSES { get; set; }
    }
}
