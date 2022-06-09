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
    public class CellVM
    {
        // - Session
        public long Id { get; set; }
        public long Id2 { get; set; }
        public long SessionId { get; set; }
        // - Section
        public long SectionId { get; set; }
        public string SectionNiveau { get; set; }

        // - Jour
        public long JourId { get; set; }
        public string JourName { get; set; }
        public string Intitule { get; set; }
        public string Intitule2 { get; set; }
        public string HD { get; set; }
        public string HD2 { get; set; }
        public string HF { get; set; }
        public string HF2 { get; set; }
    }
}
