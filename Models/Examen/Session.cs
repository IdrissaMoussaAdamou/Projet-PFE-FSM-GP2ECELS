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
    public class Session
    {
		public string AnneeUniversitaire { get; set; }
		public DateTime DateBebut { get; set; }
		public DateTime DateFin { get; set; }
		public string Designation { get; set; }
		public string Etat { get; set; }
		public long Id { get; set; }
		public string Periode { get; set; }
		public string TypeSession { get; set; }

		public Session() { }

		public Session(DataRow dataRow)
		{
			AnneeUniversitaire = Convert.ToString(dataRow["AnneeUniversitaire"]);
			DateBebut = Convert.ToDateTime(dataRow["DateBebut"]);
			DateFin = Convert.ToDateTime(dataRow["DateFin"]);
			Designation = Convert.ToString(dataRow["Designation"]);
			Etat = Convert.ToString(dataRow["Etat"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			Periode = Convert.ToString(dataRow["Periode"]);
			TypeSession = Convert.ToString(dataRow["TypeSession"]);
		}
	}
}
