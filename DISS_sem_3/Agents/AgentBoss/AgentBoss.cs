using OSPABA;
using Simulation;

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
			AddOwnMessage(Mc.PatienArrival);
			AddOwnMessage(Mc.TreatPatient);
		}
		//meta! tag="end"
	}
}
