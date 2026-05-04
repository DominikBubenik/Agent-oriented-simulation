using Agents.AgentBoss;
using Agents.AgentResources;
using OSPABA;
using Agents.AgentEntryExam;
using Agents.AgentEnviroment;
using Agents.AgentMedicalTeat;
using Agents.AgentTransition;
using Agents.AgentEDepartment;
using DISS_sem_3;
using DISS_sem_3.Entities;
using MainLogic;
using OSPAnimator;

namespace Simulation
{
	public class MySimulation : OSPABA.Simulation
	{
		public double EndSimulationTime { get; private set; }
		public SimpleStat TotalPatientCount { get; private set; }
		public SimpleStat TotalWalkInPatientCount { get; private set; }
		public SimpleStat TotalAmbulancePatientCount { get; private set; }
		public SimpleStat TotalTimeInSystem { get; private set; }
		public SimpleStat TotalTimeInSystemWalkInPatient { get; private set; }
		public SimpleStat TotalTimeInSystemAmbulancePatient { get; private set; }
		public SimpleStat TotalTimeFromEntranceToMedicalTreatPriority1 { get; private set; }
		public SimpleStat TotalTimeFromEntranceToMedicalTreatPriority2 { get; private set; }
		public SimpleStat TotalTimeFromEntranceToMedicalTreatPriority3 { get; private set; }
		public SimpleStat TotalTimeFromEntranceToMedicalTreatPriority4 { get; private set; }
		public SimpleStat TotalTimeFromEntranceToMedicalTreatPriority5 { get; private set; }
		public SimpleStat TotalEntryQueueLength { get; private set; }
		// public SimpleStat TotalEntryWaitingTime { get; private set; }
		public SimpleStat TotalEntryWaitingTimeWalkInP { get; private set; }
		public SimpleStat TotalEntryWaitingTimeAmbulanceP { get; private set; }
		public SimpleStat TotalMedicalTreatWaitingTimePatientsA { get; private set; }
		public SimpleStat TotalMedicalTreatWaitingTimePatientsAB { get; private set; }
		public SimpleStat TotalMedicalTreatWaitingTimePatientsB { get; private set; }
		public SimpleStat TotalTimeFromEntryToMedicalTreatWalkIn { get; private set; }
		public SimpleStat TotalTimeFromEntryToMedicalTreatAmbulance { get; private set; }
		public SimpleStat TotalDoctorsUtil { get; private set; }
		public SimpleStat TotalNursesUtil { get; private set; }
		public SimpleStat TotalRoomAUtil { get; private set; }
		public SimpleStat TotalRoomBUtil { get; private set; }
		
		public ResourceAllocatingStrategy ResourceAllocatingStrategy { get; private set; }
		
		
		public int InitDoctorCount { get; set; }
		public int InitNurseCount { get; set; }
		public int InitRoomACount { get; set; } = 5;
		public int InitRoomBCount { get; set; } = 7;
		
		public Random Seeder {get; private set;}
		private readonly int _seed;
		public bool WarmUpSystem {get; set;} 
		public double WarmUpTime {get; set;}
		public bool ObservationMode {get; set;}
		public event Action<string>? OnLoggerOutput;
		public int MaxEntryQueueLengthCount { get; set; } = 3;
		public int MaxMedicalQueueLengthCount { get; set; } = 3;
		
		public MySimulation(int seed, int nursesCount, int doctorsCount, bool warmUpSystem, double warmUpTime, ResourceAllocatingStrategy variant, bool observMode = false)
		{
			_seed = seed;
			Seeder = new Random(_seed);
			InitNurseCount = nursesCount;
			InitDoctorCount = doctorsCount;
			WarmUpSystem = warmUpSystem;
			WarmUpTime = warmUpTime;
			ResourceAllocatingStrategy = variant;
			ObservationMode = observMode;
			Init();
		}

		override public void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
			TotalPatientCount = new SimpleStat();
			TotalWalkInPatientCount = new SimpleStat();
			TotalAmbulancePatientCount = new SimpleStat();
			TotalTimeInSystem = new SimpleStat();
			
			TotalTimeInSystemAmbulancePatient = new SimpleStat();
			TotalTimeInSystemWalkInPatient = new SimpleStat();

			TotalEntryQueueLength = new SimpleStat();
			TotalEntryWaitingTimeAmbulanceP = new SimpleStat();
			TotalEntryWaitingTimeWalkInP = new SimpleStat();

			TotalMedicalTreatWaitingTimePatientsA = new SimpleStat();
			TotalMedicalTreatWaitingTimePatientsAB = new SimpleStat();
			TotalMedicalTreatWaitingTimePatientsB = new SimpleStat();
			
			TotalTimeFromEntryToMedicalTreatWalkIn = new SimpleStat();
			TotalTimeFromEntryToMedicalTreatAmbulance = new SimpleStat();

			TotalDoctorsUtil = new SimpleStat();
			TotalNursesUtil = new SimpleStat();
			TotalRoomAUtil = new SimpleStat();
			TotalRoomBUtil = new SimpleStat();

			TotalTimeFromEntranceToMedicalTreatPriority1 = new SimpleStat();
			TotalTimeFromEntranceToMedicalTreatPriority2 = new SimpleStat();
			TotalTimeFromEntranceToMedicalTreatPriority3 = new SimpleStat();
			TotalTimeFromEntranceToMedicalTreatPriority4 = new SimpleStat();
			TotalTimeFromEntranceToMedicalTreatPriority5 = new SimpleStat();
			
			WarmUpSystem = true;
			// EndSimulationTime = WarmUpTime + EndSimulationTime;
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...
			
			InitAnimator();
		}

		override public void ReplicationFinished()
		{
			// Collect local statistics into global, update UI, etc...
			base.ReplicationFinished();
			
			// return;
			TotalPatientCount.AddSample(AgentEnviroment.TotalPatientsStats);
			TotalWalkInPatientCount.AddSample(AgentEnviroment.TotalWalkInPatientsStats);
			TotalAmbulancePatientCount.AddSample(AgentEnviroment.TotalAmbulancedPatientsStats);
			
			TotalTimeInSystem.AddSample(AgentEnviroment.TimeInSystem.GetAverage());
			TotalTimeInSystemWalkInPatient.AddSample(AgentEnviroment.TimeInSystemWalkInPatient.GetAverage());
			TotalTimeInSystemAmbulancePatient.AddSample(AgentEnviroment.TimeInSystemAmbulancePatient.GetAverage());

			TotalEntryWaitingTimeWalkInP.AddSample(AgentEnviroment.EntranceWaitingTimeWalkInP.GetAverage());
			TotalEntryWaitingTimeAmbulanceP.AddSample(AgentEnviroment.EntranceWaitingTimeAmbulanceP.GetAverage());

			TotalMedicalTreatWaitingTimePatientsA.AddSample(AgentEnviroment.MedicalTreatWaitingTimePA.GetAverage());
			TotalMedicalTreatWaitingTimePatientsAB.AddSample(AgentEnviroment.MedicalTreatWaitingTimePAB.GetAverage());
			TotalMedicalTreatWaitingTimePatientsB.AddSample(AgentEnviroment.MedicalTreatWaitingTimePB.GetAverage());
			
			TotalDoctorsUtil.AddSample(AgentResources.GetUtilAllDoctors());
			TotalNursesUtil.AddSample(AgentResources.GetUtilAllNurses());
			TotalRoomAUtil.AddSample(AgentResources.GetUtilAllRoomsA());
			TotalRoomBUtil.AddSample(AgentResources.GetUtilAllRoomsB());
			
			TotalTimeFromEntryToMedicalTreatWalkIn.AddSample(AgentEnviroment.TimeFromEntryToMedicalTreatWalkIn.GetAverage());
			TotalTimeFromEntryToMedicalTreatAmbulance.AddSample(AgentEnviroment.TimeFromEntryToMedicalTreatAmbulance.GetAverage());
			
			TotalEntryQueueLength.AddSample(AgentEDepartment.EntryQueue.GetAverageQueueLength(CurrentTime));
			
			TotalTimeFromEntranceToMedicalTreatPriority1.AddSample(AgentEnviroment.TotalTimeFromEntranceToMedicalTreatPriority1.GetAverage());
			TotalTimeFromEntranceToMedicalTreatPriority2.AddSample(AgentEnviroment.TotalTimeFromEntranceToMedicalTreatPriority2.GetAverage());
			TotalTimeFromEntranceToMedicalTreatPriority3.AddSample(AgentEnviroment.TotalTimeFromEntranceToMedicalTreatPriority3.GetAverage());
			TotalTimeFromEntranceToMedicalTreatPriority4.AddSample(AgentEnviroment.TotalTimeFromEntranceToMedicalTreatPriority4.GetAverage());
			TotalTimeFromEntranceToMedicalTreatPriority5.AddSample(AgentEnviroment.TotalTimeFromEntranceToMedicalTreatPriority5.GetAverage());
		}

		override public void SimulationFinished()
		{
			// Display simulation results
			base.SimulationFinished();
			Console.WriteLine(DateTime.Now);

			Console.WriteLine("Simulation finished");
		}

		public int NextSeed() => Seeder.Next();
		
		
		private void InitAnimator()
		{
			if (!AnimatorExists) return;

			InitMedicalStaff(AgentResources.AllDoctors.Cast<MedicalStaff>().ToList(), Config.BASE_POSITION_DOCTORS);
			InitMedicalStaff(AgentResources.AllNurses.Cast<MedicalStaff>().ToList(), Config.BASE_POSITION_NURSES);
		}
		
		/**
		 * Metoda prevzata z prikladu Restauracia
		 */
		void InitMedicalStaff(List<MedicalStaff> skupina, PointF startPozicia)
		{
			int order = 0;
			foreach (MedicalStaff pracovnik in skupina)
			{
				PointF position = new PointF(startPozicia.X, startPozicia.Y);
				position.X = position.X + order * 50;

				if (AnimatorExists) pracovnik.AnimObject.SetPosition(position);
				order++;
			}
		}
		
		// private AnimTextItem CreateTextAnimObject(PointF pos, string text, OSPAnimator. AnimTypeface font, int size)
		// {
		// 	AnimTextItem animObject = new AnimTextItem(text);
		//
		// 	animObject.Font = (font);
		// 	animObject.Size = (size);
		// 	animObject.SetPosition(pos);
		// 	return animObject;
		// }

		public void ResetStatistics()
		{
			AgentEnviroment.Reset();
			AgentEDepartment.Reset();
			AgentResources.Reset();
		}

		public void SetEndTime(double  endTime) => EndSimulationTime = endTime;

		public void NotifyLogger(string message)
		{
			OnLoggerOutput?.Invoke(message);
		}
		

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentBoss = new AgentBoss(SimId.AgentBoss, this, null);
			AgentEnviroment = new AgentEnviroment(SimId.AgentEnviroment, this, AgentBoss);
			AgentEDepartment = new AgentEDepartment(SimId.AgentEDepartment, this, AgentBoss);
			AgentResources = new AgentResources(SimId.AgentResources, this, AgentEDepartment);
			AgentEntryExam = new AgentEntryExam(SimId.AgentEntryExam, this, AgentEDepartment);
			AgentTransition = new AgentTransition(SimId.AgentTransition, this, AgentEDepartment);
			AgentMedicalTeat = new AgentMedicalTeat(SimId.AgentMedicalTeat, this, AgentEDepartment);
		}
		public AgentBoss AgentBoss
		{ get; set; }
		public AgentEnviroment AgentEnviroment
		{ get; set; }
		public AgentEDepartment AgentEDepartment
		{ get; set; }
		public AgentResources AgentResources
		{ get; set; }
		public AgentEntryExam AgentEntryExam
		{ get; set; }
		public AgentTransition AgentTransition
		{ get; set; }
		public AgentMedicalTeat AgentMedicalTeat
		{ get; set; }
		//meta! tag="end"
	}
}