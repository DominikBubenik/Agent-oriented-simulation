using OSPABA;
using Simulation;
using Agents.AgentEnviroment.ContinualAssistants;

namespace Agents.AgentEnviroment
{
	//meta! id="5"
	public class AgentEnviroment : OSPABA.Agent
	{
		public AgentEnviroment(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
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
