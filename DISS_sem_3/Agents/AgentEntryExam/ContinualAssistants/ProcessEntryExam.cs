using OSPABA;
using Simulation;
using Agents.AgentEntryExam;

namespace Agents.AgentEntryExam.ContinualAssistants
{
	//meta! id="52"
	public class ProcessEntryExam : OSPABA.Process
	{
		public ProcessEntryExam(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentEntryExam", id="53", type="Start"
		public void ProcessStart(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentEntryExam MyAgent
		{
			get
			{
				return (AgentEntryExam)base.MyAgent;
			}
		}
	}
}