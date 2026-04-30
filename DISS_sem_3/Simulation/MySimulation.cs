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
		public SimpleStat TotalTimeInSystemPriority1 { get; private set; }
		public SimpleStat TotalTimeInSystemPriority2 { get; private set; }
		public SimpleStat TotalTimeInSystemPriority3 { get; private set; }
		public SimpleStat TotalTimeInSystemPriority4 { get; private set; }
		public SimpleStat TotalTimeInSystemPriority5 { get; private set; }
		public SimpleStat TotalEntryQueueLength { get; private set; }
		// public SimpleStat TotalEntryWaitingTime { get; private set; }
		public SimpleStat TotalEntryWaitingTimeWalkInP { get; private set; }
		public SimpleStat TotalEntryWaitingTimeAmbulanceP { get; private set; }
		public SimpleStat TotalMedicalTreatWaitingTimeAmbulanceP { get; private set; }
		public SimpleStat TotalMedicalTreatWaitingTimeWalkInP { get; private set; }
		public ResourceAllocatingStrategy ResourceAllocatingStrategy { get; private set; }
		
		public AnimTextItem SimTimeAnimObject { get; set;}
		public AnimTextItem ObjednavokAnimObject { get; set; }
		public AnimTextItem KucharovPracAnimObject { get; set; }
		public AnimTextItem KucharovNepracAnimObject { get; set; }
		public AnimTextItem CasnikovPracAnimObject { get; set; }
		public AnimTextItem CasnikovNepracAnimObject { get; set; }

		public int InitDoctorCount { get; set; } = 5;
		public int InitNurseCount { get; set; } = 10;
		public int InitRoomACount { get; set; } = 5;
		public int InitRoomBCount { get; set; } = 7;
		
		public Random Seeder {get; private set;}
		private readonly int _seed;
		
		public MySimulation(int seed)
		{
			_seed = seed;
			Seeder = new Random(_seed);
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

			TotalMedicalTreatWaitingTimeAmbulanceP = new SimpleStat();
			TotalMedicalTreatWaitingTimeWalkInP = new SimpleStat();

			ResourceAllocatingStrategy = ResourceAllocatingStrategy.Exp0FirstAvailable;
			
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

			TotalPatientCount.AddSample(AgentEnviroment.TotalPatientsStats);
			TotalWalkInPatientCount.AddSample(AgentEnviroment.TotalWalkInPatientsStats);
			TotalAmbulancePatientCount.AddSample(AgentEnviroment.TotalAmbulancedPatientsStats);
			
			TotalTimeInSystem.AddSample(AgentEnviroment.TimeInSystem.GetAverage());
			TotalTimeInSystemWalkInPatient.AddSample(AgentEnviroment.TimeInSystemWalkInPatient.GetAverage());
			TotalTimeInSystemAmbulancePatient.AddSample(AgentEnviroment.TimeInSystemAmbulancePatient.GetAverage());

			TotalEntryWaitingTimeWalkInP.AddSample(AgentEnviroment.EntranceWaitingTimeWalkInP.GetAverage());
			TotalEntryWaitingTimeAmbulanceP.AddSample(AgentEnviroment.EntranceWaitingTimeAmbulanceP.GetAverage());

			TotalMedicalTreatWaitingTimeAmbulanceP.AddSample(AgentEnviroment.MedicalTreatWaitingTimeAmbulanceP.GetAverage());
			TotalMedicalTreatWaitingTimeWalkInP.AddSample(AgentEnviroment.MedicalTreatWaitingTimeWalkInP.GetAverage());
			
			TotalEntryQueueLength.AddSample(AgentEDepartment.EntryQueue.GetAverageQueueLength(CurrentTime));
		}

		override public void SimulationFinished()
		{
			// Display simulation results
			base.SimulationFinished();

			Console.WriteLine("Simulation finished");
		}

		public int NextSeed() => Seeder.Next();
		
		
		private void InitAnimator()
		{
			if (!AnimatorExists) return;

			UsporiadajSkupinu(AgentResources.AllDoctors.Cast<MedicalStaff>().ToList(), Config.BASE_POSITION_DOCTORS);

			// Config.Gui.SetSimSpeed();
		}
		
		void UsporiadajSkupinu(List<MedicalStaff> skupina, PointF startPozicia)
		{
			int poradie = 0;
			foreach (MedicalStaff pracovnik in skupina)
			{
				PointF pozicia = new PointF(startPozicia.X, startPozicia.Y);
				pozicia.X = pozicia.X + poradie * 50;

				if (AnimatorExists) pracovnik.AnimObject.SetPosition(pozicia);
				poradie++;
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
		
		public void SetEndTime(double  endTime) => EndSimulationTime = endTime;


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