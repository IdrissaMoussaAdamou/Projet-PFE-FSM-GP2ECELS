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
    public class UneSection
    {
		public long Id { get; set; }
		public string IntituleFiliere { get; set; }

		public string IntituleParcours { get; set; }

		public int? NbEtudiants { get; set; }
		public string Niveau { get; set; }

		public string CodeAnneeUniv { get; set; }

		public UneSection() { }

		public UneSection(DataRow dataRow)
		{
			Id = Convert.ToInt64(dataRow["Id"]);

			IntituleFiliere = (dataRow["IntituleFiliere"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IntituleFiliere"]);
			
			IntituleParcours = (dataRow["IntituleParcours"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IntituleParcours"]);
			
			NbEtudiants = (dataRow["NbEtudiants"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NbEtudiants"]);
		
			Niveau = Convert.ToString(dataRow["Niveau"]);

			CodeAnneeUniv = Convert.ToString(dataRow["CodeAnneeUniv"]);
		}
	}
}
