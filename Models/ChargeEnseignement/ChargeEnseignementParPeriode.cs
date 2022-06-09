namespace Projet_PFE.Models
{
    public class ChargeEnseignementParPeriode
    {
        public long Id { get; set; }
        public string Grade { get; set; }
        public string Statut { get; set; }
        public string Periode { get; set; }
        public int NumPeriodeDansAnnee { get; set; }
        public float NbSemainesPeriode { get; set; }
        public string UniteVolume { get; set; }
        public float VolumeCours  { get; set; }
        public float VolumeTD  { get; set; }
        public float VolumeTP { get; set; }
        public float VolumeSuppCours { get; set; }
        public float VolumeSuppTD { get; set; }
        public float VolumeSuppTP { get; set; }

        public string CodeAnneeUniv { get; set; }
        public long IdAUEnseignant  { get; set; }

        
        public ChargeEnseignementParPeriode()
        {
            VolumeCours = VolumeTD = VolumeTP = 0;
            VolumeSuppCours = VolumeSuppTD = VolumeSuppTP = 0;
        }
    }
}