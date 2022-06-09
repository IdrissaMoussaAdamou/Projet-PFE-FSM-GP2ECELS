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
    public class SessionSection
    {
		public string CodeFiliere { get; set; }
		public string CodeParcours { get; set; }
		public long Id { get; set; }
		public long IdSession { get; set; }
		public string IntituleFiliere { get; set; }
		public string IntituleFiliereAbrg { get; set; }
		public string IntituleParcours { get; set; }
		public string IntituleParcoursAbrg { get; set; }
		public int? NbEtudiants { get; set; }
		public string Niveau { get; set; }
		public string Niveaudyplo { get; set; }
		public int? Periode { get; set; }
		public string TypeDiplome { get; set; }

		public SessionSection() { }

		public SessionSection(DataRow dataRow)
		{
			CodeFiliere = (dataRow["CodeFiliere"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CodeFiliere"]);
			CodeParcours = (dataRow["CodeParcours"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CodeParcours"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			IdSession = Convert.ToInt64(dataRow["IdSession"]);
			IntituleFiliere = (dataRow["IntituleFiliere"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IntituleFiliere"]);
			IntituleFiliereAbrg = (dataRow["IntituleFiliereAbrg"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IntituleFiliereAbrg"]);
			IntituleParcours = (dataRow["IntituleParcours"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IntituleParcours"]);
			IntituleParcoursAbrg = (dataRow["IntituleParcoursAbrg"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IntituleParcoursAbrg"]);
			NbEtudiants = (dataRow["NbEtudiants"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NbEtudiants"]);
			Niveau = (dataRow["Niveau"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Niveau"]);
			Niveaudyplo = (dataRow["Niveaudyplo"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Niveaudyplo"]);
			Periode = (dataRow["Periode"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Periode"]);
			TypeDiplome = Convert.ToString(dataRow["TypeDiplome"]);

		}
	}
}
