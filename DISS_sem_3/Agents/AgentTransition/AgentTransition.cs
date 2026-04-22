using OSPABA;
using Simulation;

namespace Agents.AgentTransition
{
	//meta! id="104"
	public class AgentTransition : OSPABA.Agent
	{
		public AgentTransition(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerTransition(SimId.ManagerTransition, MySim, this);
			AddOwnMessage(Mc.MedicalTreatTransition);
			AddOwnMessage(Mc.EntryExamTransition);
		}
		//meta! tag="end"
	}
}
