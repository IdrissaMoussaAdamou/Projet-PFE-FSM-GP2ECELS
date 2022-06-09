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
    public class Salle
    {
		public int? CapaciteEnseignement { get; set; }
		public int? CapaciteExamen { get; set; }

		[Required(ErrorMessage = "Champs Obligatoire non saisi")]
		[Remote(action: "VerifyCode", controller: "Salle", AdditionalFields = "OldCode")]
		public string Code { get; set; }
		public string CodeBatiment { get; set; }
		public int? Etage { get; set; }
		public string Etat { get; set; }
		public int? NbSurveillants { get; set; }
		public string Type { get; set; }

		public Salle() { }

		public Salle(DataRow dataRow)
		{
			CapaciteEnseignement = (dataRow["CapaciteEnseignement"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CapaciteEnseignement"]);
			CapaciteExamen = (dataRow["CapaciteExamen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CapaciteExamen"]);
			Code = Convert.ToString(dataRow["Code"]);
			CodeBatiment = (dataRow["CodeBatiment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CodeBatiment"]);
			Etage = (dataRow["Etage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Etage"]);
			Etat = (dataRow["Etat"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Etat"]);
			NbSurveillants = (dataRow["NbSurveillants"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NbSurveillants"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
		}
	}
}
