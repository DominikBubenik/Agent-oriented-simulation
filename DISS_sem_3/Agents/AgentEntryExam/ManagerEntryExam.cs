using OSPABA;
using Simulation;

namespace Agents.AgentEntryExam
{
	//meta! id="38"
	public class ManagerEntryExam : OSPABA.Manager
	{
		public ManagerEntryExam(int id, OSPABA.Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="AgentEDepartment", id="47", type="Request"
		public void ProcessEntryExamPatient(MessageForm message)
		{
		}

		//meta! sender="ProcessEntryFinished", id="71", type="Finish"
		public void ProcessFinishProcessEntryFinished(MessageForm message)
		{
		}

		//meta! sender="ProcessEntryExam", id="53", type="Finish"
		public void ProcessFinishProcessEntryExam(MessageForm message)
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
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.ProcessEntryFinished:
					ProcessFinishProcessEntryFinished(message);
				break;

				case SimId.ProcessEntryExam:
					ProcessFinishProcessEntryExam(message);
				break;
				}
			break;

			case Mc.EntryExamPatient:
				ProcessEntryExamPatient(message);
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
