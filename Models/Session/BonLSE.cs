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
    public class BonLSE
    {
        public SessionExamen SE { get; set; }
        public int? nbc { get; set; }
    }
}
