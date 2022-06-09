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
    public class SessionExamen
    {
		public string CodeFiliere { get; set; }
		public string CodeModule { get; set; }
		public string CodeParcours { get; set; }
		public string HeureDebut { get; set; }
		public string HeureFin { get; set; }
		public long Id { get; set; }
		public long IdSession { get; set; }
		public long IdSessionJour { get; set; }
		public long IdSessionSeance { get; set; }
		public string Intitule { get; set; }
		public string Nature { get; set; }
		public int? NbEtudiants { get; set; }
		public string Niveau { get; set; }
		public int? Periode { get; set; }

		public int numcell { get; set; }

		public SessionExamen() { }

		public SessionExamen(DataRow dataRow)
		{
			CodeFiliere = Convert.ToString(dataRow["CodeFiliere"]);
			CodeModule = Convert.ToString(dataRow["CodeModule"]);
			CodeParcours = Convert.ToString(dataRow["CodeParcours"]);
			HeureDebut = Convert.ToString(dataRow["HeureDebut"]);
			HeureFin = Convert.ToString(dataRow["HeureFin"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			numcell = Convert.ToInt32(dataRow["numcell"]);
			IdSession = Convert.ToInt64(dataRow["IdSession"]);
			IdSessionJour = Convert.ToInt64(dataRow["IdSessionJour"]);
			IdSessionSeance = Convert.ToInt64(dataRow["IdSessionSeance"]);
			Intitule = Convert.ToString(dataRow["Intitule"]);
			Nature = Convert.ToString(dataRow["Nature"]);
			NbEtudiants = (dataRow["NbEtudiants"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NbEtudiants"]);
			Niveau = (dataRow["Niveau"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Niveau"]);
			Periode = (dataRow["Periode"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Periode"]);
		}
	}
}
