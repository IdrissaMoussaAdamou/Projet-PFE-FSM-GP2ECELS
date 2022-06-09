using System.Collections.Generic;
using Projet_PFE.Models;
using Projet_PFE.Dal;
using ChargeEnseignement;
using System.Linq;

using System;
namespace Projet_PFE.Bll
{
    public class Bll_ChargeEnseignementParPeriode
    {
        public static void Add(ChargeEnseignementParPeriode ChrgEnParPeriode) =>  Dal_ChargeEnseignementParPeriode.Add(ChrgEnParPeriode);
           
        public static void Delete(long Id) => Dal_ChargeEnseignementParPeriode.Delete(Id);
            
        public static void Update(ChargeEnseignementParPeriode ChrgEnParPeriode) => Dal_ChargeEnseignementParPeriode.Update(ChrgEnParPeriode);
            
        public static List<ChargeEnseignementParPeriode> SelectAll(long TeacherId) => Dal_ChargeEnseignementParPeriode.SelectAll(TeacherId);

        public static ChargeEnseignementParPeriode SelectById(long Id) => Dal_ChargeEnseignementParPeriode.SelectById(Id);

        public static void ManageTeacherChargeParPeriode(AnneeUniversitaireEnseignant Teacher)
        {
            var LesChargesEnseignements = Bll_ChargeParModule.SelectAll(Teacher.Id);
            var LesChargesDiverses = Bll_ChargeDiverse.SelectAll(Teacher.Id);
            var LesChargesEncadrements = Bll_ChargeEncadrement.SelectAll(Teacher.Id);

            //les charges  d'enseignement par période 
            var ChargeSemestre1 = new ChargeEnseignementParPeriode();
            var ChargeSemestre2 = new ChargeEnseignementParPeriode();
            var ChargeAnnuelle = new ChargeEnseignementParPeriode();

            // initialiser certains champs
            ChargeSemestre1.Grade = ChargeSemestre2.Grade = ChargeAnnuelle.Grade = Teacher.Grade;
            ChargeSemestre1.Statut = ChargeSemestre2.Statut = ChargeAnnuelle.Statut = Teacher.Statut;
            ChargeSemestre1.CodeAnneeUniv = ChargeSemestre2.CodeAnneeUniv = ChargeAnnuelle.CodeAnneeUniv = Teacher.CodeAnneeUniv;

            //  Construire les charges d'enseignement par période
            ConstructChargeEnseignementParNumeroPeriode("Semestre", LesChargesEnseignements,LesChargesDiverses,ChargeSemestre1, 1);
            ConstructChargeEnseignementParNumeroPeriode("Semestre", LesChargesEnseignements,LesChargesDiverses,ChargeSemestre2, 2);
            //pour la charge annuelle le numéro de la période n'est pas considéré
            ConstructChargeEnseignementParNumeroPeriode("Annee", LesChargesEnseignements,LesChargesDiverses,ChargeAnnuelle, 100);
            
            ChargeSemestre1.IdAUEnseignant = ChargeSemestre2.IdAUEnseignant = ChargeAnnuelle.IdAUEnseignant = Teacher.Id;
             
            //les charges  d'encadrement par période
            var ChargeEncadS1 = new ChargeEncadrementTotale();
            var ChargeEncadS2 = new ChargeEncadrementTotale();
            var ChargeEncaAnnuelle = new ChargeEncadrementTotale();

            // Construire les charges d'encadrement par période
            ConstructChargeEncadrementParNumeroPeriode("Semestre", LesChargesEncadrements, ChargeEncadS1, 1);
            ConstructChargeEncadrementParNumeroPeriode("Semestre", LesChargesEncadrements, ChargeEncadS2, 2); 
            ConstructChargeEncadrementParNumeroPeriode("Annee", LesChargesEncadrements, ChargeEncaAnnuelle, 100); 

            //additionner les chargees d'encadrement aux charges d'enseignement
            AddChargeEncadrementToChargeEnseignement(ChargeEncadS1, ChargeSemestre1);
            AddChargeEncadrementToChargeEnseignement(ChargeEncadS2, ChargeSemestre2);
            AddChargeEncadrementToChargeEnseignement(ChargeEncaAnnuelle, ChargeAnnuelle);

            var ListeChargeParPeriode = new List<ChargeEnseignementParPeriode>(3);
            ListeChargeParPeriode.Add(ChargeSemestre1);
            ListeChargeParPeriode.Add(ChargeSemestre2);
            ListeChargeParPeriode.Add(ChargeAnnuelle);
            

            var LesChargesParPeriode = Bll_ChargeEnseignementParPeriode.SelectAll(Teacher.Id);
            if( LesChargesParPeriode != null && LesChargesParPeriode.Count > 0) // la charge sera modifiée
            {
                for(int i = 0; i > 3; i ++)
                {
                    ListeChargeParPeriode[i].Id = LesChargesParPeriode[i].Id;
                }
                // update
                foreach(var CEP in ListeChargeParPeriode)
                {
                    Update(CEP);
                }
            }
            else
            {
                //la charge est nouvellement ajoutée
                foreach(var CEP in ListeChargeParPeriode)
                {
                    Add(CEP);
                }
            }
        }

        private static void ConstructChargeEnseignementParNumeroPeriode(string Periode, List<ChargeParModule> LesChargesEnseignements, List<ChargeDiverse> LesChargesDiverses, ChargeEnseignementParPeriode Charge, int NumeroPeriode)
        {
            if(LesChargesEnseignements != null && LesChargesEnseignements.Count > 0)
            {
                if(LesChargesDiverses == null)
                    LesChargesDiverses = new List<ChargeDiverse>();
                
                if(Periode == "Semestre")
                {
                    var LesChargesEnseignementsParNumeroPeriode = LesChargesEnseignements.Where(CPM => CPM.NumPeriodeDansAnnee == NumeroPeriode);
                    if(LesChargesEnseignementsParNumeroPeriode != null && LesChargesEnseignementsParNumeroPeriode.ToList().Count > 0)
                    {
                        var LesChargesDiversesParNumeroPeriode  = LesChargesDiverses.Where(CD => CD.NumPeriodeDansAnnee == NumeroPeriode);
                        if(LesChargesDiversesParNumeroPeriode == null)
                            LesChargesDiversesParNumeroPeriode = new List<ChargeDiverse>();

                        var ChargeCalculee = BLL_CalculCharges.CalculChargeEnseignementTotaleParPeriode("Semestre", 14f, Charge.Grade, Charge.Statut,LesChargesEnseignementsParNumeroPeriode.ToList(),LesChargesDiversesParNumeroPeriode.ToList());
                        CopyCharge(Charge, ChargeCalculee);
                        Charge.NumPeriodeDansAnnee = NumeroPeriode;
                        Charge.NbSemainesPeriode = 14;
                    }
                }
                else
                {
                    var ChargeCalculee = BLL_CalculCharges.CalculChargeEnseignementTotaleParPeriode("Annee", 28f, Charge.Grade, Charge.Statut,LesChargesEnseignements, LesChargesDiverses);
                    CopyCharge(Charge, ChargeCalculee);
                   
                }   
            }

            if( Periode == "Semestre" )
            {
                Charge.NumPeriodeDansAnnee = NumeroPeriode;
                Charge.NbSemainesPeriode = 14;
                if( Charge.IdAUEnseignant <=0 )
                {
                    Charge.Periode = "Semestre";
                    Charge.NumPeriodeDansAnnee = NumeroPeriode;
                    Charge.UniteVolume = "unkown";
                }
            }
            else
            {
                Charge.NumPeriodeDansAnnee = NumeroPeriode;
                Charge.NbSemainesPeriode = 28;

                 if( Charge.IdAUEnseignant <=0 )
                {
                    Charge.Periode = "Annee";
                    Charge.NumPeriodeDansAnnee = NumeroPeriode;
                    Charge.UniteVolume = "unkown";
                }
            }
        }

        private static void CopyCharge( ChargeEnseignementParPeriode Initcharge, ChargeEnseignementParPeriode chargeCalculee )
        {
            Initcharge.Periode = chargeCalculee.Periode;
            Initcharge.UniteVolume = chargeCalculee.UniteVolume;
            Initcharge.VolumeCours = chargeCalculee.VolumeCours;
            Initcharge.VolumeTD = chargeCalculee.VolumeTD;
            Initcharge.VolumeTP = chargeCalculee.VolumeTP;
            Initcharge.IdAUEnseignant = chargeCalculee.IdAUEnseignant;
            Initcharge.VolumeSuppCours = chargeCalculee.VolumeSuppCours;
            Initcharge.VolumeSuppTD = chargeCalculee.VolumeSuppTD;
            Initcharge.VolumeSuppTP = chargeCalculee.VolumeSuppTP;
            Initcharge.VolumeSuppCours = chargeCalculee.VolumeSuppCours;
            Initcharge.Grade = chargeCalculee.Grade;
            Initcharge.Statut =  chargeCalculee.Statut;

        }

        private static void ConstructChargeEncadrementParNumeroPeriode(string Periode, List<ChargeEncadrement> LesChargesEncad, ChargeEncadrementTotale charge, int NumeroPeriode)
        {
            if(LesChargesEncad != null && LesChargesEncad.Count > 0)
            {
                if(Periode == "Semestre")
                {
                    var LesChargesEncadParPeriode = LesChargesEncad.Where(CE => CE.TypeEncad.NumPeriodeDansAnnee == NumeroPeriode);
                    if(LesChargesEncadParPeriode != null && LesChargesEncadParPeriode.ToList().Count > 0)
                    {
                        var ChargeCalculee = BLL_CalculCharges.CalculChargeEncadrementParPeriode("Semestre", 14, LesChargesEncadParPeriode.ToList());
                        charge.VolumeCours = ChargeCalculee.VolumeCours;
                        charge.VolumeTD = ChargeCalculee.VolumeTD;
                        charge.VolumeTP = ChargeCalculee.VolumeTP;
                    }
                }
                else
                {
                    var ChargeCalculee = BLL_CalculCharges.CalculChargeEncadrementParPeriode("Annee", 28, LesChargesEncad);
                    charge.VolumeCours = ChargeCalculee.VolumeCours;
                    charge.VolumeTD = ChargeCalculee.VolumeTD;
                    charge.VolumeTP = ChargeCalculee.VolumeTP;
                }
            }
        }

        private static void AddChargeEncadrementToChargeEnseignement( ChargeEncadrementTotale CET, ChargeEnseignementParPeriode CEP)
        {
            CEP.VolumeCours += CET.VolumeCours;
            CEP.VolumeTD += CET.VolumeTD;
            CEP.VolumeTP += CET.VolumeTP;
        }
    }
}