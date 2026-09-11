using OSPABA;

namespace Simulation
{
	public class Mc : OSPABA.IdList
	{
		//meta! userInfo="Generated code: do not modify", tag="begin"
		public const int EntranceTransition = 1026;
		public const int PatientArrival = 1003;
		public const int ExitTransition = 1028;
		public const int PatientExit = 1006;
		public const int FreeUpResources = 1020;
		public const int TreatPatient = 1008;
		public const int GetEntryExamResources = 1010;
		public const int MedicalTreatPatient = 1011;
		public const int EntryExamPatient = 1013;
		public const int BetweenAmbulanceTransition = 1024;
		public const int GetMedicalTreatResources = 1015;
		public const int PatientTreated = 1029;
		public const int SendEntryExamResources = 1030;
		public const int SendMedicalTreatResources = 1031;
		//meta! tag="end"

		// 1..1000 range reserved for user
	}
}