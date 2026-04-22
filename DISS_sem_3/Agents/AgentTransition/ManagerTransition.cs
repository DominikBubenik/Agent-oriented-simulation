using OSPABA;
using Simulation;

namespace Agents.AgentTransition
{
	//meta! id="104"
	public class ManagerTransition : OSPABA.Manager
	{
		public ManagerTransition(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentEDepartment", id="112", type="Request"
		public void ProcessMedicalTreatTransition(MessageForm message)
		{
		}

		//meta! sender="AgentEDepartment", id="111", type="Request"
		public void ProcessEntryExamTransition(MessageForm message)
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
			case Mc.MedicalTreatTransition:
				ProcessMedicalTreatTransition(message);
			break;
			
			case Mc.EntryExamTransition:
				ProcessEntryExamTransition(message);
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
