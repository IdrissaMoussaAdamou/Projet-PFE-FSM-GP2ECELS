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
    public class SessionSeance
    {
		public string Designation { get; set; }
		public string DesignationAbregee { get; set; }
		public string HeureDebut { get; set; }
		public string HeureFin { get; set; }
		public long Id { get; set; }
		public long IdSession { get; set; }

		public SessionSeance() { }

		public SessionSeance(DataRow dataRow)
		{
			Designation = Convert.ToString(dataRow["Designation"]);
			DesignationAbregee = (dataRow["DesignationAbregee"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DesignationAbregee"]);
			HeureDebut = (dataRow["HeureDebut"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HeureDebut"]);
			HeureFin = (dataRow["HeureFin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HeureFin"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			IdSession = Convert.ToInt64(dataRow["IdSession"]);
		}
	}
}
