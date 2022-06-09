using System;
using Projet_PFE.Models;
using System.Collections.Generic;

namespace ChargeEnseignement
{
    class BLL_CalculCharges
    {

        public const float NbHoursToTeachForProf = 5.5f;
        public const float Conv_TDToCoursForProf = 0.75f;
        public const float Conv_TPToCoursForProf = 0.5f;

        public const float NbHoursToTeachForMC = 5.5f;
        public const float Conv_TDToCoursForMC = 0.75f;
        public const float Conv_TPToCoursForMC = 0.5f;


        public const float NbTDHoursToTeachForMA = 9.5f;
        public const float NbTPHoursToTeachForMA = 14f;
        public const float Conv_CoursToTDForMA = 1.83f;
        public const float Conv_CoursToTPForMA = 2.75f;
        public const float Conv_TDToTPForMA = 1.47f;
        public const float Conv_TPToTDForMA = 0.67f;


        public const float NbTDHoursToTeachForAss = 11f;
        public const float NbTPHoursToTeachForAss = 15f;
        public const float Conv_CoursToTDForAss = 1.83f;
        public const float Conv_CoursToTPForAss = 2.75f;
        public const float Conv_TDToTPForAss = 1.36f;
        public const float Conv_TPToTDForAss = 0.73f;


        public const float NbTDHoursToTeachForDemiAss = 5.5f;
        public const float NbTPHoursToTeachForDemiAss = 7.5f;
        public const float Conv_CoursToTDForDemiAss = 1.83f;
        public const float Conv_CoursToTPForDemiAss = 2.75f;
        public const float Conv_TDToTPForDemiAss = 1.36f;
        public const float Conv_TPToTDForDemiAss = 0.73f;


        public const float NbHoursToTeachForPES = 14f;
        public const float Conv_CoursToTDForPES = 1;
        public const float Conv_CoursToTPForPES = 1;
        public const float Conv_TDToTPForPES = 1;
        public const float Conv_TPToTDForPES = 1;

        private static void CalculateChargeSupForProfesseur(ChargeEnseignementParPeriode CEP)
        {
            if (CEP.VolumeCours >= NbHoursToTeachForProf) // Cours is sufficient
            {
                CEP.VolumeSuppCours = CEP.VolumeCours - NbHoursToTeachForProf;
                CEP.VolumeSuppTD = CEP.VolumeTD;
                CEP.VolumeSuppTP = CEP.VolumeTP;
            }
            else  // Cours is not sufficient
            {
                float CoursHoursToComplete = 0;
                // Complete from TD
                CoursHoursToComplete = NbHoursToTeachForProf - CEP.VolumeCours;
                float TDConvertedToCours = CEP.VolumeTD * Conv_TDToCoursForProf;
                if (TDConvertedToCours < CoursHoursToComplete) // TD is not sufficient
                {
                    // Complete from TP
                    CoursHoursToComplete = CoursHoursToComplete - TDConvertedToCours;
                    float TPConvertedToCours = CEP.VolumeTP * Conv_TPToCoursForProf;
                    if (TPConvertedToCours < CoursHoursToComplete) // TP is not sufficient
                    {
                        // the due is not reached
                        CEP.VolumeSuppCours = TPConvertedToCours - CoursHoursToComplete;
                        CEP.VolumeSuppTD = 0;
                        CEP.VolumeSuppTP = 0;
                    }
                    else // the due is reatched. ChargeSup from TP only
                    {
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = 0;
                        CEP.VolumeSuppTP = (TPConvertedToCours - CoursHoursToComplete) / Conv_TPToCoursForProf;
                    }
                }
                else // TD is sufficient
                {
                    // the due is reatched ChargeSup from TD and TP
                    CEP.VolumeSuppCours = 0;
                    CEP.VolumeSuppTD = (TDConvertedToCours - CoursHoursToComplete) / Conv_TDToCoursForProf;
                    CEP.VolumeSuppTP = CEP.VolumeTP;
                }
            }
        }
        private static void CalculateChargeSupForMaitreConference(ChargeEnseignementParPeriode CEP)
        {

            if (CEP.VolumeCours >= NbHoursToTeachForMC) // Cours is sufficient
            {
                CEP.VolumeSuppCours = CEP.VolumeCours - NbHoursToTeachForMC;
                CEP.VolumeSuppTD = CEP.VolumeTD;
                CEP.VolumeSuppTP = CEP.VolumeTP;
            }
            else  // Cours is not sufficient
            {
                float CoursHoursToComplete = 0;
                // Complete from TD
                CoursHoursToComplete = NbHoursToTeachForMC - CEP.VolumeCours;
                float TDConvertedToCours = CEP.VolumeTD * Conv_TDToCoursForMC;
                if (TDConvertedToCours < CoursHoursToComplete) // TD is not sufficient
                {
                    // Complete from TP
                    CoursHoursToComplete = CoursHoursToComplete - TDConvertedToCours;
                    float TPConvertedToCours = CEP.VolumeTP * Conv_TPToCoursForMC;
                    if (TPConvertedToCours < CoursHoursToComplete) // TP is not sufficient
                    {
                        // the due is not reached
                        CEP.VolumeSuppCours = TPConvertedToCours - CoursHoursToComplete;
                        CEP.VolumeSuppTD = 0;
                        CEP.VolumeSuppTP = 0;
                    }
                    else // the due is reatched. ChargeSup from TP only
                    {
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = 0;
                        CEP.VolumeSuppTP = (TPConvertedToCours - CoursHoursToComplete) / Conv_TPToCoursForMC;
                    }
                }
                else // TD is sufficient
                {
                    // the due is reatched ChargeSup from TD and TP
                    CEP.VolumeSuppCours = 0;
                    CEP.VolumeSuppTD = (TDConvertedToCours - CoursHoursToComplete) / Conv_TDToCoursForMC;
                    CEP.VolumeSuppTP = CEP.VolumeTP;
                }
            }
        }
        private static void TDBasedConversionForMaitreAssistant(ChargeEnseignementParPeriode CEP)
        {
            // Start with TD
            if (CEP.VolumeTD >= NbTDHoursToTeachForMA) // TD is sufficient
            {
                CEP.VolumeSuppCours = CEP.VolumeCours;
                CEP.VolumeSuppTD = CEP.VolumeTD - NbTDHoursToTeachForMA; ;
                CEP.VolumeSuppTP = CEP.VolumeTP;
            }
            else  // TD is not sufficient
            {
                float TDHoursToComplete = 0;
                TDHoursToComplete = NbTDHoursToTeachForMA - CEP.VolumeTD;
                // Complete from Cours
                float TDConvertedFromCours = CEP.VolumeCours * Conv_CoursToTDForMA;
                if (TDConvertedFromCours < TDHoursToComplete) // Cours is not sufficient
                {
                    // Complete from TP
                    TDHoursToComplete = TDHoursToComplete - TDConvertedFromCours;
                    float TDConvertedFromTP = CEP.VolumeTP * Conv_TPToTDForMA;
                    if (TDConvertedFromTP < TDHoursToComplete) // TP is not sufficient
                    {
                        // the due is not reached
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = TDConvertedFromTP - TDHoursToComplete;
                        CEP.VolumeSuppTP = 0;
                    }
                    else // the due is reatched. ChargeSup from TP only
                    {
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = 0;
                        CEP.VolumeSuppTP = (TDConvertedFromTP - TDHoursToComplete) / Conv_TPToTDForMA;
                    }
                }
                else // Cours is sufficient
                {
                    // the due is reatched. ChargeSup from TD and TP
                    CEP.VolumeSuppCours = (TDConvertedFromCours - TDHoursToComplete) / Conv_CoursToTDForMA;
                    CEP.VolumeSuppTD = 0;
                    CEP.VolumeSuppTP = CEP.VolumeTP;
                }
            }
        }
        private static void TPBasedConversionForMaitreAssistant(ChargeEnseignementParPeriode CEP)
        {
            // Start with TP
            if (CEP.VolumeTP >= NbTPHoursToTeachForMA) // TP is sufficient
            {
                CEP.VolumeSuppCours = CEP.VolumeCours;
                CEP.VolumeSuppTD = CEP.VolumeTD;
                CEP.VolumeSuppTP = CEP.VolumeTP - NbTPHoursToTeachForMA; ;
            }
            else  // TP is not sufficient
            {
                float TPHoursToComplete = 0;

                TPHoursToComplete = NbTPHoursToTeachForMA - CEP.VolumeTP;
                // Complete from Cours
                float TPConvertedFromCours = CEP.VolumeCours * Conv_CoursToTPForMA;
                if (TPConvertedFromCours < TPHoursToComplete) // Cours is not sufficient
                {
                    // Complete from TD
                    TPHoursToComplete = TPHoursToComplete - TPConvertedFromCours;
                    float TPConvertedFromTD = CEP.VolumeTD * Conv_TDToTPForMA;
                    if (TPConvertedFromTD < TPHoursToComplete) // TD is not sufficient
                    {
                        // the due is not reached
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = 0;
                        CEP.VolumeSuppTP = TPConvertedFromTD - TPHoursToComplete; ;
                    }
                    else // the due is reatched. ChargeSup from TD only
                    {
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = (TPConvertedFromTD - TPHoursToComplete) / Conv_TDToTPForMA;
                        CEP.VolumeSuppTP = 0;
                    }
                }
                else // Cours is sufficient
                {
                    // the due is reatched. ChargeSup from Cours and TD
                    CEP.VolumeSuppCours = (TPConvertedFromCours - TPHoursToComplete) / Conv_CoursToTPForMA;
                    CEP.VolumeSuppTD = CEP.VolumeTD;
                    CEP.VolumeSuppTP = 0;
                }
            }
        }
        private static void CalculateChargeSupForMaitreAssistant(ChargeEnseignementParPeriode CEP)
        {
            Console.WriteLine("Charge MA");
            if (CEP.VolumeTD >= CEP.VolumeTP) // Conversion based on TD
                TDBasedConversionForMaitreAssistant(CEP);
            else
                TPBasedConversionForMaitreAssistant(CEP);
        }
        private static void TDBasedConversionForAssistant(ChargeEnseignementParPeriode CEP)
        {
            // Start with TD
            if (CEP.VolumeTD >= NbTDHoursToTeachForAss) // TD is sufficient
            {
                CEP.VolumeSuppCours = CEP.VolumeCours;
                CEP.VolumeSuppTD = CEP.VolumeTD - NbTDHoursToTeachForAss; ;
                CEP.VolumeSuppTP = CEP.VolumeTP;
            }
            else  // TD is not sufficient
            {
                float TDHoursToComplete = 0;
                TDHoursToComplete = NbTDHoursToTeachForAss - CEP.VolumeTD;
                // Complete from Cours
                float TDConvertedFromCours = CEP.VolumeCours * Conv_CoursToTDForAss;
                if (TDConvertedFromCours < TDHoursToComplete) // Cours is not sufficient
                {
                    // Complete from TP
                    TDHoursToComplete = TDHoursToComplete - TDConvertedFromCours;
                    float TDConvertedFromTP = CEP.VolumeTP * Conv_TPToTDForAss;
                    if (TDConvertedFromTP < TDHoursToComplete) // TP is not sufficient
                    {
                        // the due is not reached
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = TDConvertedFromTP - TDHoursToComplete;
                        CEP.VolumeSuppTP = 0;
                    }
                    else // the due is reatched. ChargeSup from TP only
                    {
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = 0;
                        CEP.VolumeSuppTP = (TDConvertedFromTP - TDHoursToComplete) / Conv_TPToTDForAss;
                    }
                }
                else // Cours is sufficient
                {
                    // the due is reatched. ChargeSup from TD and TP
                    CEP.VolumeSuppCours = (TDConvertedFromCours - TDHoursToComplete) / Conv_CoursToTDForAss;
                    CEP.VolumeSuppTD = 0;
                    CEP.VolumeSuppTP = CEP.VolumeTP;
                }
            }
        }
        private static void TPBasedConversionForAssistant(ChargeEnseignementParPeriode CEP)
        {
            // Start with TP
            if (CEP.VolumeTP >= NbTPHoursToTeachForAss) // TP is sufficient
            {
                CEP.VolumeSuppCours = CEP.VolumeCours;
                CEP.VolumeSuppTD = CEP.VolumeTD;
                CEP.VolumeSuppTP = CEP.VolumeTP - NbTPHoursToTeachForAss; ;
            }
            else  // TP is not sufficient
            {
                float TPHoursToComplete = 0;

                TPHoursToComplete = NbTPHoursToTeachForAss - CEP.VolumeTP;
                // Complete from Cours
                float TPConvertedFromCours = CEP.VolumeCours * Conv_CoursToTPForAss;
                if (TPConvertedFromCours < TPHoursToComplete) // Cours is not sufficient
                {
                    // Complete from TD
                    TPHoursToComplete = TPHoursToComplete - TPConvertedFromCours;
                    float TPConvertedFromTD = CEP.VolumeTD * Conv_TDToTPForAss;
                    if (TPConvertedFromTD < TPHoursToComplete) // TD is not sufficient
                    {
                        // the due is not reached
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = 0;
                        CEP.VolumeSuppTP = TPConvertedFromTD - TPHoursToComplete; ;
                    }
                    else // the due is reatched. ChargeSup from TD only
                    {
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = (TPConvertedFromTD - TPHoursToComplete) / Conv_TDToTPForAss;
                        CEP.VolumeSuppTP = 0;
                    }
                }
                else // Cours is sufficient
                {
                    // the due is reatched. ChargeSup from Cours and TD
                    CEP.VolumeSuppCours = (TPConvertedFromCours - TPHoursToComplete) / Conv_CoursToTPForAss;
                    CEP.VolumeSuppTD = CEP.VolumeTD;
                    CEP.VolumeSuppTP = 0;
                }
            }
        }
        private static void CalculateChargeSupForAssistant(ChargeEnseignementParPeriode CEP)
        {
            if (CEP.VolumeTD > CEP.VolumeTP) // Conversion based on TD
                TDBasedConversionForAssistant(CEP);
            else
                TPBasedConversionForAssistant(CEP);
        }
        private static void TDBasedConversionForDemiAssistant(ChargeEnseignementParPeriode CEP)
        {
            // Start with TD
            if (CEP.VolumeTD >= NbTDHoursToTeachForDemiAss) // TD is sufficient
            {
                CEP.VolumeSuppCours = CEP.VolumeCours;
                CEP.VolumeSuppTD = CEP.VolumeTD - NbTDHoursToTeachForDemiAss; ;
                CEP.VolumeSuppTP = CEP.VolumeTP;
            }
            else  // TD is not sufficient
            {
                float TDHoursToComplete = 0.0f;
                TDHoursToComplete = NbTDHoursToTeachForDemiAss - CEP.VolumeTD;
                // Complete from Cours
                float TDConvertedFromCours = CEP.VolumeCours * Conv_CoursToTDForDemiAss;
                if (TDConvertedFromCours < TDHoursToComplete) // Cours is not sufficient
                {
                    // Complete from TP
                    TDHoursToComplete = TDHoursToComplete - TDConvertedFromCours;
                    float TDConvertedFromTP = CEP.VolumeTP * Conv_TPToTDForDemiAss;
                    if (TDConvertedFromTP < TDHoursToComplete) // TP is not sufficient
                    {
                        // the due is not reached
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = TDConvertedFromTP - TDHoursToComplete;
                        CEP.VolumeSuppTP = 0;
                    }
                    else // the due is reatched. ChargeSup from TP only
                    {
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = 0;
                        CEP.VolumeSuppTP = (TDConvertedFromTP - TDHoursToComplete) / Conv_TPToTDForDemiAss;
                    }
                }
                else // Cours is sufficient
                {
                    // the due is reatched. ChargeSup from TD and TP
                    CEP.VolumeSuppCours = (TDConvertedFromCours - TDHoursToComplete) / Conv_CoursToTDForDemiAss;
                    CEP.VolumeSuppTD = 0;
                    CEP.VolumeSuppTP = CEP.VolumeTP;
                }
            }
        }
        private static void TPBasedConversionForDemiAssistant(ChargeEnseignementParPeriode CEP)
        {
            // Start with TP
            if (CEP.VolumeTP >= NbTPHoursToTeachForDemiAss) // TP is sufficient
            {
                CEP.VolumeSuppCours = CEP.VolumeCours;
                CEP.VolumeSuppTD = CEP.VolumeTD;
                CEP.VolumeSuppTP = CEP.VolumeTP - NbTPHoursToTeachForDemiAss; ;
            }
            else  // TP is not sufficient
            {
                float TPHoursToComplete = 0;

                TPHoursToComplete = NbTPHoursToTeachForDemiAss - CEP.VolumeTP;
                // Complete from Cours
                float TPConvertedFromCours = CEP.VolumeCours * Conv_CoursToTPForDemiAss;
                if (TPConvertedFromCours < TPHoursToComplete) // Cours is not sufficient
                {
                    // Complete from TD
                    TPHoursToComplete = TPHoursToComplete - TPConvertedFromCours;
                    float TPConvertedFromTD = CEP.VolumeTD * Conv_TDToTPForDemiAss;
                    if (TPConvertedFromTD < TPHoursToComplete) // TD is not sufficient
                    {
                        // the due is not reached
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = 0;
                        CEP.VolumeSuppTP = TPConvertedFromTD - TPHoursToComplete; ;
                    }
                    else // the due is reatched. ChargeSup from TD only
                    {
                        CEP.VolumeSuppCours = 0;
                        CEP.VolumeSuppTD = (TPConvertedFromTD - TPHoursToComplete) / Conv_TDToTPForDemiAss;
                        CEP.VolumeSuppTP = 0;
                    }
                }
                else // Cours is sufficient
                {
                    // the due is reatched. ChargeSup from Cours and TD
                    CEP.VolumeSuppCours = (TPConvertedFromCours - TPHoursToComplete) / Conv_CoursToTPForDemiAss;
                    CEP.VolumeSuppTD = CEP.VolumeTD;
                    CEP.VolumeSuppTP = 0;
                }
            }
        }
        private static void CalculateChargeSupForDemiAssistant(ChargeEnseignementParPeriode CEP)
        {
            if (CEP.VolumeTD > CEP.VolumeTP) // Conversion based on TD
                TDBasedConversionForDemiAssistant(CEP);
            else
                TPBasedConversionForDemiAssistant(CEP);
        }
        private static void CalculateChargeSupForPES(ChargeEnseignementParPeriode CEP)
        {
            CEP.VolumeSuppCours = 0;
            CEP.VolumeSuppTD = CEP.VolumeCours + CEP.VolumeTD + CEP.VolumeTP - NbHoursToTeachForPES;
            CEP.VolumeSuppTP = 0;
        }
        public static void CalculateChargeSup(ChargeEnseignementParPeriode CEP)
        {
            // Calcul de la charge  et de la charge supplémentaire


            TeacherGrades TG = EnumsManager.GetEnumTeacherGrade(CEP.Grade);
            switch (TG)
            {
                case TeacherGrades.PROFESSEUR:
                    {
                        CalculateChargeSupForProfesseur(CEP);
                        break;
                    }
                case TeacherGrades.MAITRE_CONFERENCE:
                    {
                        CalculateChargeSupForMaitreConference(CEP);
                        break;
                    }
                case TeacherGrades.MAITRE_ASSISTANT:
                    {
                        CalculateChargeSupForMaitreAssistant(CEP);
                        break;
                    }
                case TeacherGrades.ASSISTANT:
                    {
                        CalculateChargeSupForAssistant(CEP);
                        break;
                    }

                case TeacherGrades.ASSITANT_DEMI:
                    {
                        CalculateChargeSupForDemiAssistant(CEP);
                        break;
                    }
                case TeacherGrades.PES: //TeacherGrades.PES
                    {
                        CalculateChargeSupForPES(CEP);
                        break;
                    }

            }

            // Arrondissement des valeurs calculées
            CEP.VolumeSuppCours = (float)Math.Round(CEP.VolumeSuppCours, 2);
            CEP.VolumeSuppTD = (float)Math.Round(CEP.VolumeSuppTD, 2);
            CEP.VolumeSuppTP = (float)Math.Round(CEP.VolumeSuppTP, 2);



        }

        /*----------------------------------------------------------------------------------------------------------------
         * Cette fonction peut calculer à la fois la charge par période : semestre, trimestre, quarter ou même par année
         * il suffit de lui passer la charge adéquate et le nombre de semaines de la période souhaitée
         ----------------------------------------------------------------------------------------------------------------*/
        public static ChargeEnseignementParPeriode CalculChargeEnseignementTotaleParPeriode(string pTypePeriode, float pNbSemainesPeriode, string pGrade, string pStatut, List<ChargeParModule> LesChargesParModule, List<ChargeDiverse> LesChargesDiverses)
        {

            //---------------------------------------------------------
            // Calcul du total des charges d'enseignement des modules
            // le calcul doit se faire en tenant compte du nombre de semaines d'enseignement du module
            float VolumeTotalEnseignementCours = 0;
            float VolumeTotalEnseignementTD = 0;
            float VolumeTotalEnseignementTP = 0;
            foreach (ChargeParModule CPM in LesChargesParModule)
            {
                switch(EnumsManager.GetEnumTeachingNature(CPM.NatureEnseignement))
                {
                    case TeachingNatures.COURS:
                        VolumeTotalEnseignementCours = VolumeTotalEnseignementCours + CPM.VolumeTotal;
                        break;
                    case TeachingNatures.TD:
                        VolumeTotalEnseignementTD = VolumeTotalEnseignementTD + CPM.VolumeTotal;
                        break;
                    case TeachingNatures.TP:
                        VolumeTotalEnseignementTP = VolumeTotalEnseignementTP + CPM.VolumeTotal;
                        break;
                }
            }

            //---------------------------------------------------------
            // Calcul du total des charges divers
            // Le calcul doit se faire en tenant compte du nombre de semaines spécifié dans l'objet ChargeDivers
            float VolumeTotalChargeDiversCours = 0;
            float VolumeTotalChargeDiversTD = 0;
            float VolumeTotalChargeDiversTP = 0;
            float VolumeCharge = 0;

            foreach (ChargeDiverse CD in LesChargesDiverses)
            {
                // La charge entrante peut être hebdo ou par période, mais dans les calculs elle doit être par période
                if (EnumsManager.GetEnumUniteVolume(CD.UniteVolume) == UnitesVolume.HEBDO)
                {
                    VolumeCharge = (float)(CD.Volume * CD.NbSemainesPeriode);
                }
                else // par période
                {
                    VolumeCharge = (float)CD.Volume;
                }


                switch (EnumsManager.GetEnumTeachingNature(CD.NatureCharge))
                {
                    case TeachingNatures.COURS:
                        VolumeTotalChargeDiversCours = VolumeTotalChargeDiversCours + VolumeCharge;
                        break;

                    case TeachingNatures.TD:
                        VolumeTotalChargeDiversTD = VolumeTotalChargeDiversTD + VolumeCharge;
                        break;

                    case TeachingNatures.TP:
                        VolumeTotalChargeDiversTP = VolumeTotalChargeDiversTP + VolumeCharge;
                        break;
                    
                }
            }

            ChargeEnseignementParPeriode CEP = new ChargeEnseignementParPeriode();
            CEP.Id = LesChargesParModule[0].Id;
            CEP.CodeAnneeUniv = LesChargesParModule[0].CodeAnneeUniv;
            CEP.IdAUEnseignant = LesChargesParModule[0].IdAUEnseignant;
            CEP.Periode = pTypePeriode;
            CEP.Grade = pGrade;
            CEP.Statut = pStatut;

            // Calcul du volume hébdomadaire total
            CEP.UniteVolume = EnumsManager.GetStringUniteVolume(UnitesVolume.HEBDO); ;
            CEP.VolumeCours = (VolumeTotalEnseignementCours + VolumeTotalChargeDiversCours)/pNbSemainesPeriode;
            CEP.VolumeTD = (VolumeTotalEnseignementTD + VolumeTotalChargeDiversTD)/ pNbSemainesPeriode;
            CEP.VolumeTP = (VolumeTotalEnseignementTP + VolumeTotalChargeDiversTP)/ pNbSemainesPeriode;
            
            // Calcul de la charge d'enseignement supplémentaires dans le cas d'un permanent ou contractuel            
            TeacherStatus Statut = EnumsManager.GetEnumTeacherStatus(CEP.Statut);
            if (Statut == TeacherStatus.PERMANENT || Statut == TeacherStatus.CONTRACTUEL)
            {
                CalculateChargeSup(CEP);
            }
            return CEP; 
         }

        public static ChargeEncadrementTotale CalculChargeEncadrementParPeriode(string pTypePeriode, float pNbSemainesPeriode, List<ChargeEncadrement> LesEncadrements)
        {
            float VolumeTotalEncadrementCours = 0;
            float VolumeTotalEncadrementTD = 0;
            float VolumeTotalEncadrementTP = 0;
            float VolumeChargeEncad = 0;
            foreach(ChargeEncadrement Encad in LesEncadrements)
            {
                // Le volume total d'un type donné d'encadrement 
                VolumeChargeEncad = (float)(Encad.TypeEncad.VolumeHebdoCharge * Encad.NbEncadrements * Encad.TypeEncad.NbSemainesPeriode);
                switch (EnumsManager.GetEnumTeachingNature(Encad.TypeEncad.NatureCharge))
                {
                   
                    
                    case TeachingNatures.COURS :
                        VolumeTotalEncadrementCours = VolumeTotalEncadrementCours + VolumeChargeEncad;
                    break;

                    case TeachingNatures.TD :
                        VolumeTotalEncadrementTD = VolumeTotalEncadrementTD + VolumeChargeEncad;
                    break;

                    case TeachingNatures.TP :
                        VolumeTotalEncadrementTP = VolumeTotalEncadrementTP + VolumeChargeEncad;
                    break;

                }

            }

            ChargeEncadrementTotale CET = new ChargeEncadrementTotale();
            CET.Id = LesEncadrements[0].Id;
            CET.CodeAnneeUniv = LesEncadrements[0].CodeAnneeUniv;
            CET.IdAUEnseignant = LesEncadrements[0].IdAUEnseignant;
            CET.Periode = pTypePeriode;
            CET.NbSemainesPeriode = pNbSemainesPeriode;
            CET.UniteVolume = EnumsManager.GetStringUniteVolume(UnitesVolume.HEBDO);
            CET.VolumeCours = VolumeTotalEncadrementCours / pNbSemainesPeriode;
            CET.VolumeTD = VolumeTotalEncadrementTD / pNbSemainesPeriode;
            CET.VolumeTP = VolumeTotalEncadrementTP / pNbSemainesPeriode;


            return CET;

        }        
        

    }

}
