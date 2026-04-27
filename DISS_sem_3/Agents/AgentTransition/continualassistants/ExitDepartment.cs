using OSPABA;
using Simulation;
using Agents.AgentTransition;

namespace Agents.AgentTransition.ContinualAssistants
{
	//meta! id="132"
	public class ExitDepartment : OSPABA.Process
	{
		public ExitDepartment(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentTransition", id="133", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			var duration = MyAgent.ExitSystemDuration();
			myMsg.Code = Mc.Finish;
			Hold(duration, myMsg);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var myMsg = (MyMessage)message;
					myMsg.Addressee = MyAgent;
					AssistantFinished(myMsg);
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
		public new AgentTransition MyAgent
		{
			get
			{
				return (AgentTransition)base.MyAgent;
			}
		}
	}
}