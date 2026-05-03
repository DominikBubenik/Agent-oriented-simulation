using OSPABA;
using Simulation;
using Agents.AgentBoss.ContinualAssistants;

namespace Agents.AgentBoss
{
	//meta! id="2"
	public class AgentBoss : OSPABA.Agent
	{
		public AgentBoss(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerBoss(SimId.ManagerBoss, MySim, this);
			new WarmUp(SimId.WarmUp, MySim, this);
			AddOwnMessage(Mc.PatientArrival);
			AddOwnMessage(Mc.PatientTreated);
		}
		//meta! tag="end"
	}
}