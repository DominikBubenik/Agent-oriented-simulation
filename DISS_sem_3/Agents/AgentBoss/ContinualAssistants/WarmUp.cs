using Agents.AgentBoss;
using OSPABA;
using Simulation;

namespace Agents.AgentBoss.ContinualAssistants
{
	//meta! id="189"
	public class WarmUp : OSPABA.Process
	{
		public WarmUp(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentBoss", id="190", type="Start"
		public void ProcessStart(MessageForm message)
		{
			message.Code = Mc.Finish;
			Hold(MyAgent.MyCastSim().WarmUpTime, message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					AssistantFinished(message);
					break;
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
		public new AgentBoss MyAgent
		{
			get
			{
				return (AgentBoss)base.MyAgent;
			}
		}
	}
}