using OSPABA;
using Simulation;
using Agents.AgentEnviroment.ContinualAssistants;
using MainLogic;

namespace Agents.AgentEnviroment
{
	//meta! id="5"
	public class AgentEnviroment : OSPABA.Agent
	{
		public SimpleStat TimeInSystem { get; private set; }
		public int TreatedPatientsCount { get; set; }
		public AgentEnviroment(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			TimeInSystem = new SimpleStat();
			TreatedPatientsCount = 0;

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

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerEnviroment(SimId.ManagerEnviroment, MySim, this);
			new RegularPatient(SimId.RegularPatient, MySim, this);
			new AmbulancePatient(SimId.AmbulancePatient, MySim, this);
			AddOwnMessage(Mc.PatientExit);
		}
		//meta! tag="end"
	}
}