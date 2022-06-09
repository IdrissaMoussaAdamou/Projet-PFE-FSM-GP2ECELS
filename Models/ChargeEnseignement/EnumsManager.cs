
namespace ChargeEnseignement
{
    public enum UnitesVolume {HEBDO, PERIOD};
    public enum TeacherGrades { PROFESSEUR, MAITRE_CONFERENCE, MAITRE_ASSISTANT, ASSISTANT, ASSITANT_DEMI, PES };
    public enum TeachingNatures { COURS, TD, TP };
    public enum TeachingPeriods { YEAR, SEMESTER, TRIMESTER, QUARTER };

    public enum TeacherStatus {PERMANENT, CONTRACTUEL, VACATAIRE, EXPERT}


    public class EnumsManager
    {
        public static TeacherGrades GetEnumTeacherGrade(string TG)
        {
            switch (TG)
            {
                case "Professeur":
                    return TeacherGrades.PROFESSEUR;
                case "Maître de conférence":
                    return TeacherGrades.MAITRE_CONFERENCE;
                case "Maître assistant":
                    return TeacherGrades.MAITRE_ASSISTANT;
                case "Assistant (Permanent/Contractuel)":
                    return TeacherGrades.ASSISTANT;
                case "Assitant (Demi-contrat)":
                    return TeacherGrades.ASSITANT_DEMI;
                default:
                    return TeacherGrades.PES;
            }
        }

        public static string GetStringTeacherGrade(TeacherGrades TG)
        {
            switch (TG)
            {
                case TeacherGrades.PROFESSEUR:
                    return "Professeur";
                case TeacherGrades.MAITRE_CONFERENCE:
                    return "Maître de conférence";
                case TeacherGrades.MAITRE_ASSISTANT:
                    return "Maître assistant";
                case TeacherGrades.ASSISTANT:
                    return "Assistant (Permanent/Contractuel)";
                case TeacherGrades.ASSITANT_DEMI:
                    return "Assitant (Demi-contrat)";
                default:
                    return "PES";
            }
        }
        public static TeacherStatus GetEnumTeacherStatus(string TS)
        {
            switch (TS)
            {
                case "Permanent":
                    return TeacherStatus.PERMANENT;
                case "Contractuel":
                    return TeacherStatus.CONTRACTUEL;
                case "Vacataire":
                    return TeacherStatus.VACATAIRE;
                
                default:
                    return TeacherStatus.EXPERT;
            }
        }
        public static string GetStringTeacherStatus(TeacherStatus TS)
        {
            switch (TS)
            {
                case TeacherStatus.PERMANENT:
                    return "Permanent";
                case TeacherStatus.CONTRACTUEL:
                    return "Contractuel";
                case TeacherStatus.VACATAIRE:
                    return "Vacataire";

                default:
                    return "Expert";
            }
        }
        public static TeachingPeriods GetEnumTeachingPeriod(string TeachPer)
        {
            switch (TeachPer)
            {

                case "Semestre":
                    return TeachingPeriods.SEMESTER;

                case "Trimestre":
                    return TeachingPeriods.TRIMESTER;

                case "Quarter":
                    return TeachingPeriods.QUARTER;

                default:
                    return TeachingPeriods.YEAR;
            }
        }
        public static TeachingNatures GetEnumTeachingNature(string TN)
        {
            switch (TN)
            {
                case "Cours":
                    return TeachingNatures.COURS;
                case "TD":
                    return TeachingNatures.TD;                
                default:
                    return TeachingNatures.TP;
            }

        }
        public static UnitesVolume GetEnumUniteVolume(string UV)
        {
            switch (UV)
            {
                case "Hebdomadaire":
                    return UnitesVolume.HEBDO;                
                default:
                    return UnitesVolume.PERIOD;
            }

        }
        public static string GetStringUniteVolume(UnitesVolume UV)
        {
            switch (UV)
            {
                case UnitesVolume.HEBDO:
                    return "Hebdomadaire";
                default:
                    return "Période";
            }

        }
    }
}
