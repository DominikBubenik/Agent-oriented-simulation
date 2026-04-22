using OSPABA;

namespace Simulation
{
	public class Mc : OSPABA.IdList
	{
		//meta! userInfo="Generated code: do not modify", tag="begin"
		public const int PatientArrival = 1003;
		public const int EntryExamResources = 1010;
		public const int MedicalTreatPatient = 1011;
		public const int PatientExit = 1006;
		public const int FreeUpResources = 1020;
		public const int EntryExamPatient = 1013;
		public const int EntryExamTransition = 1024;
		public const int MedicalTreatTransition = 1025;
		public const int MedicalTreatResources = 1015;
		public const int TreatPatient = 1008;
		//meta! tag="end"

		// 1..1000 range reserved for user
	}
}