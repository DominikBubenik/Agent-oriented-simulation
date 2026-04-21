using OSPABA;
using Simulation;
using Agents.AgentResources.InstantAssistants;

namespace Agents.AgentResources
{
	//meta! id="34"
	public class AgentResources : OSPABA.Agent
	{
		public AgentResources(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerResources(SimId.ManagerResources, MySim, this);
			new AllocateEntryExamRes(SimId.AllocateEntryExamRes, MySim, this);
			new AllocateMedicalTreatRes(SimId.AllocateMedicalTreatRes, MySim, this);
			AddOwnMessage(Mc.FreeUpResources);
			AddOwnMessage(Mc.MedicalTreatResources);
			AddOwnMessage(Mc.EntryExamResources);
		}
		//meta! tag="end"
	}
}
