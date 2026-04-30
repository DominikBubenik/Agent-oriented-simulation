using OSPABA;
using Simulation;
using Agents.AgentEnviroment.ContinualAssistants;
using DISS_sem_3.Entities;
using MainLogic;

namespace Agents.AgentEnviroment
{
	//meta! id="5"
	public class AgentEnviroment : OSPABA.Agent
	{
		public SimpleStat TimeInSystem { get; private set; }
		public SimpleStat TimeInSystemWalkInPatient { get; private set; }
		public SimpleStat TimeInSystemAmbulancePatient { get; private set; }

		public Dictionary<string, Patient> AllPatientsInSystem { get; private set; }

		public SimpleStat EntranceWaitingTimeAmbulanceP { get; private set; }
		public SimpleStat EntranceWaitingTimeWalkInP { get; private set; }

		public SimpleStat MedicalTreatWaitingTimePA { get; private set; }
		public SimpleStat MedicalTreatWaitingTimePAB { get; private set; }
		public SimpleStat MedicalTreatWaitingTimePB { get; private set; }
		public int TotalPatientsStats { get; set; }
		public int TotalWalkInPatientsStats { get; set; }
		public int TotalAmbulancedPatientsStats { get; set; }

		private ExponentionalGenerator _walkInGenerator;
		private GammaGenerator _ambulanceGenerator;

		public AgentEnviroment(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
			_walkInGenerator = new ExponentionalGenerator(MyCastSim().Seeder.Next(), 1.0 / (9.57 * 60));
			_ambulanceGenerator = new GammaGenerator(MyCastSim().Seeder.Next(), 7.041, (0.831 * 60));
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
			TimeInSystem = new SimpleStat();
			TimeInSystemWalkInPatient = new SimpleStat();
			TimeInSystemAmbulancePatient = new SimpleStat();
			

			TotalPatientsStats = 0;
			TotalWalkInPatientsStats = 0;
			TotalAmbulancedPatientsStats = 0;

			EntranceWaitingTimeWalkInP = new SimpleStat();
			EntranceWaitingTimeAmbulanceP = new SimpleStat();

			MedicalTreatWaitingTimePA = new SimpleStat();
			MedicalTreatWaitingTimePAB = new SimpleStat();
			MedicalTreatWaitingTimePB = new SimpleStat();

			AllPatientsInSystem = new Dictionary<string, Patient>();
			SchedulePatientArrivals();
		}

		private void SchedulePatientArrivals()
		{
			var message = new MyMessage(MySim);
			message.Addressee = FindAssistant(SimId.WalkInPatient);
			message.Code = Mc.Start;
			MyManager.StartContinualAssistant(message);
			
			message = new MyMessage(MySim);
			message.Addressee = FindAssistant(SimId.AmbulancePatient);
			message.Code = Mc.Start;
			MyManager.StartContinualAssistant(message);
		}

		public void PatientEntered(Patient patient) => AllPatientsInSystem.Add(patient.Name, patient);

		public void PatientExit(Patient patient)
		{
			TimeInSystem.AddSample(MySim.CurrentTime - patient.ArrivalTime);
			if (patient.ArrivedByAmbulance)
			{
				TotalAmbulancedPatientsStats++;
				TimeInSystemAmbulancePatient.AddSample(MySim.CurrentTime - patient.ArrivalTime);
				EntranceWaitingTimeAmbulanceP.AddSample(patient.EntryQueueWaitingTime);
			}
			else
			{
				TotalWalkInPatientsStats++;
				TimeInSystemWalkInPatient.AddSample(MySim.CurrentTime - patient.ArrivalTime);
				EntranceWaitingTimeWalkInP.AddSample(patient.EntryQueueWaitingTime);
			}

			if (patient.Priority < 3)
			{
				MedicalTreatWaitingTimePA.AddSample(patient.MedicalQueueWaitingTime);
			} else if (patient.Priority < 5)
			{
				MedicalTreatWaitingTimePAB.AddSample(patient.MedicalQueueWaitingTime);
			}
			else
			{
				MedicalTreatWaitingTimePB.AddSample(patient.MedicalQueueWaitingTime);
			}

			AllPatientsInSystem.Remove(patient.Name);
			TotalPatientsStats++;
		}

		public double GetNextWalkIn() => _walkInGenerator.Generate();
		public double GetNextAmbulance() => _ambulanceGenerator.Generate();

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerEnviroment(SimId.ManagerEnviroment, MySim, this);
			new WalkInPatient(SimId.WalkInPatient, MySim, this);
			new AmbulancePatient(SimId.AmbulancePatient, MySim, this);
			AddOwnMessage(Mc.PatientExit);
		}
		//meta! tag="end"
		
		private MySimulation MyCastSim() => (MySimulation)MySim;
	}
}