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
			message.Code = Mc.TreatPatient;
			message.Addressee = MySimInstance.FindAgent(SimId.AgentEDepartment);
			Notice(message);
		}

		//meta! userInfo="Removed from model"
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

		//meta! sender="AgentEDepartment", id="179", type="Notice"
		public void ProcessPatientTreated(MessageForm message)
		{
			message.Code = Mc.PatientExit;
			message.Addressee = MySimInstance.YellowPages.FindFirstAgent(Mc.PatientExit);
			Notice(message);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.PatientArrival:
				ProcessPatientArrival(message);
			break;

			case Mc.PatientTreated:
				ProcessPatientTreated(message);
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
		
		public MySimulation MySimInstance => (MySimulation)MySim;
	}
}