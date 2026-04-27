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

		public int TreatedPatientsCount { get; set; }

		public SimpleStat EntranceWaitingTimeAmbulanceP { get; private set; }
		public SimpleStat EntranceWaitingTimeWalkInP { get; private set; }

		public SimpleStat MedicalTreatWaitingTimeAmbulanceP { get; private set; }
		public SimpleStat MedicalTreatWaitingTimeWalkInP { get; private set; }

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

			TreatedPatientsCount = 0;

			EntranceWaitingTimeAmbulanceP = new SimpleStat();
			EntranceWaitingTimeWalkInP = new SimpleStat();

			MedicalTreatWaitingTimeAmbulanceP = new SimpleStat();
			MedicalTreatWaitingTimeWalkInP = new SimpleStat();

			SchedulePatientArrivals();
		}

		private void SchedulePatientArrivals()
		{
			var message = new MyMessage(MySim);
			message.Addressee = FindAssistant(SimId.RegularPatient);
			message.Code = Mc.Start;
			MyManager.StartContinualAssistant(message);
			
			message = new MyMessage(MySim);
			message.Addressee = FindAssistant(SimId.AmbulancePatient);
			message.Code = Mc.Start;
			MyManager.StartContinualAssistant(message);
		}
		
		public void PatientExit(Patient patient)
		{
			TimeInSystem.AddSample(MySim.CurrentTime - patient.ArrivalTime);
			if (patient.ArrivedByAmbulance)
			{
				TimeInSystemAmbulancePatient.AddSample(MySim.CurrentTime - patient.ArrivalTime);
			}
			else
			{
				TimeInSystemWalkInPatient.AddSample(MySim.CurrentTime - patient.ArrivalTime);
			}
			
			TreatedPatientsCount++;
		}

		public double GetNextWalkIn() => _walkInGenerator.Generate();
		public double GetNextAmbulance() => _ambulanceGenerator.Generate();

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerEnviroment(SimId.ManagerEnviroment, MySim, this);
			new RegularPatient(SimId.RegularPatient, MySim, this);
			new AmbulancePatient(SimId.AmbulancePatient, MySim, this);
			AddOwnMessage(Mc.PatientExit);
		}
		//meta! tag="end"
		
		private MySimulation MyCastSim() => (MySimulation)MySim;
	}
}