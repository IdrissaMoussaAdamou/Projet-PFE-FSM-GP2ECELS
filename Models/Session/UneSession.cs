using System.Collections.Generic;

namespace Projet_PFE.Models
{
    public class UneSession
    {
        public Session Session;
        public List<SessionSeance> ListSeance { get; set; }
        public List<SessionJour> ListJournée { get; set; }
        public List<SessionSurveillant> ListSurveillants { get; set; }
        public List<SessionSection> ListSection { get; set; }
        public List<SessionSalle> ListSalles { get; set; }
    }
}
