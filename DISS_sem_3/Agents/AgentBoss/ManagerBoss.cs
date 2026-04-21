using OSPABA;
using Simulation;

namespace Agents.AgentBoss
{
	//meta! id="2"
	public class ManagerBoss : OSPABA.Manager
	{
		public ManagerBoss(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentEnviroment", id="9", type="Notice"
		public void ProcessPatientArrival(MessageForm message)
		{
		}

		//meta! sender="AgentEDepartment", id="23", type="Response"
		public void ProcessTreatPatient(MessageForm message)
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
			case Mc.TreatPatient:
				ProcessTreatPatient(message);
			break;

			case Mc.PatientArrival:
				ProcessPatientArrival(message);
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