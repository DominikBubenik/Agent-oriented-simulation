using Agents.AgentTransition.ContinualAssistants;
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
			new ExitDepartment(SimId.ExitDepartment, MySim, this);
			new AllRoomTransfer(SimId.AllRoomTransfer, MySim, this);
			new EntryWalkInPatient(SimId.EntryWalkInPatient, MySim, this);
			new EntryAmbulancePatient(SimId.EntryAmbulancePatient, MySim, this);
			AddOwnMessage(Mc.EntranceTransmition);
			AddOwnMessage(Mc.ExitTransmition);
			AddOwnMessage(Mc.BetweenAmbulanceTransition);
		}
		//meta! tag="end"
	}
}