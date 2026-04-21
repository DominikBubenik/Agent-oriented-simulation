using OSPABA;
using Simulation;

namespace Agents.AgentEDepartment
{
	//meta! id="21"
	public class AgentEDepartment : OSPABA.Agent
	{
		public AgentEDepartment(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerEDepartment(SimId.ManagerEDepartment, MySim, this);
			AddOwnMessage(Mc.MedicalTreatResources);
			AddOwnMessage(Mc.TreatPatient);
			AddOwnMessage(Mc.EntryExamPatient);
			AddOwnMessage(Mc.EntryExamResources);
			AddOwnMessage(Mc.MedicalTreatPatient);
		}
		//meta! tag="end"
	}
}
