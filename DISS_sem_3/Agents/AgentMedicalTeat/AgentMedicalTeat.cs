using OSPABA;
using Simulation;
using Agents.AgentMedicalTeat.ContinualAssistants;

namespace Agents.AgentMedicalTeat
{
	//meta! id="41"
	public class AgentMedicalTeat : OSPABA.Agent
	{
		public AgentMedicalTeat(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerMedicalTeat(SimId.ManagerMedicalTeat, MySim, this);
			new ProcessMedicalTreatFinished(SimId.ProcessMedicalTreatFinished, MySim, this);
			new ProcessMedicalTreat(SimId.ProcessMedicalTreat, MySim, this);
			AddOwnMessage(Mc.MedicalTreatPatient);
		}
		//meta! tag="end"
	}
}
