using OSPABA;
using Simulation;
using Agents.AgentEntryExam.ContinualAssistants;

namespace Agents.AgentEntryExam
{
	//meta! id="38"
	public class AgentEntryExam : OSPABA.Agent
	{
		public AgentEntryExam(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerEntryExam(SimId.ManagerEntryExam, MySim, this);
			new ProcessEntryExam(SimId.ProcessEntryExam, MySim, this);
			AddOwnMessage(Mc.EntryExamPatient);
		}
		//meta! tag="end"
	}
}