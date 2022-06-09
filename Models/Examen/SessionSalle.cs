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
	public class SessionSalle
	{
		public int? CapaciteEnseignement { get; set; }
		public int? CapaciteExamen { get; set; }
		public string CodeSalle { get; set; }
		public string Etat { get; set; }
		public long Id { get; set; }
		public long IdSession { get; set; }
		public int? NbSurveillants { get; set; }

		public SessionSalle() { }

		public SessionSalle(DataRow dataRow)
		{
			CapaciteEnseignement = (dataRow["CapaciteEnseignement"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CapaciteEnseignement"]);
			CapaciteExamen = (dataRow["CapaciteExamen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CapaciteExamen"]);
			CodeSalle = Convert.ToString(dataRow["CodeSalle"]);
			Etat = (dataRow["Etat"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Etat"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			IdSession = Convert.ToInt64(dataRow["IdSession"]);
			NbSurveillants = (dataRow["NbSurveillants"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NbSurveillants"]);
		}
	}
}
