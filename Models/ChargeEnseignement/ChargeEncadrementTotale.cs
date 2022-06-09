using System;
using System.Collections.Generic;
using System.Text;

namespace ChargeEnseignement
{
    class ChargeEncadrementTotale
    {
        public long Id { get; set; }
        public string CodeAnneeUniv { get; set; }
        public long IdAUEnseignant { get; set; }

        public string Periode { get; set; }
        public float NbSemainesPeriode { get; set; }

        public string UniteVolume { get; set; }
        public float VolumeCours { get; set; }
        public float VolumeTD { get; set; }
        public float VolumeTP { get; set; }       

    }
}
