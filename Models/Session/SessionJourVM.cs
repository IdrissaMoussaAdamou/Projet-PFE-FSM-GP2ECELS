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
    public class SessionJourVM
    {
        public List<string> Niveaux { get; set; }
        public List<string> Jours { get; set; }
        public List<List<CellVM>> Cells { get; set; }
    }
}
