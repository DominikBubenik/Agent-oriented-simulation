using OSPABA;
using Simulation;
using Agents.AgentEnviroment;

namespace Agents.AgentEnviroment.ContinualAssistants
{
	//meta! id="17"
	public class AmbulancePatient : OSPABA.Scheduler
	{
		public AmbulancePatient(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentEnviroment", id="18", type="Start"
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
		public new AgentEnviroment MyAgent
		{
			get
			{
				return (AgentEnviroment)base.MyAgent;
			}
		}
	}
}