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
    public class SessionSurveillant
    {
		public string CIN { get; set; }
		public string CodeDepartement { get; set; }
		public string Email1 { get; set; }
		public string Email2 { get; set; }
		public string Grade { get; set; }
		public long Id { get; set; }
		public long IdSession { get; set; }
		public long IdEnseignant { get; set; }
		public string Nom { get; set; }
		public string Prenom { get; set; }
		public string SituationAdministrative { get; set; }
		public string Statut { get; set; }
		public string Telephone1 { get; set; }
		public string Telephone2 { get; set; }

		public SessionSurveillant() { }

		public SessionSurveillant(DataRow dataRow)
		{
			CIN = Convert.ToString(dataRow["CIN"]);
			CodeDepartement = Convert.ToString(dataRow["CodeDepartement"]);
			Email1 = (dataRow["Email1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email1"]);
			Email2 = (dataRow["Email2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email2"]);
			Grade = (dataRow["Grade"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grade"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			IdSession = Convert.ToInt64(dataRow["IdSession"]);
			IdEnseignant = Convert.ToInt64(dataRow["IdEnseignant"]);
			Nom = (dataRow["Nom"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nom"]);
			Prenom = (dataRow["Prenom"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prenom"]);
			SituationAdministrative = Convert.ToString(dataRow["SituationAdministrative"]);
			Statut = (dataRow["Statut"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Statut"]);
			Telephone1 = (dataRow["Telephone1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telephone1"]);
			Telephone2 = (dataRow["Telephone2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telephone2"]);
		}
	}
}
