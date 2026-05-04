using OSPABA;
using Simulation;

namespace Agents.AgentMedicalTeat
{
	//meta! id="41"
	public class ManagerMedicalTeat : OSPABA.Manager
	{
		public ManagerMedicalTeat(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentEDepartment", id="45", type="Request"
		public void ProcessMedicalTreatPatient(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			myMsg.Code = Mc.Start;
			myMsg.Addressee = MyAgent.FindAssistant(SimId.ProcessMedicalTreat);
			StartContinualAssistant(myMsg);
		}

		//meta! sender="ProcessMedicalTreat", id="56", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			myMsg.Code = Mc.MedicalTreatPatient;
			myMsg.Addressee = MyAgent.Parent;
			Response(myMsg);
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
			case Mc.MedicalTreatPatient:
				ProcessMedicalTreatPatient(message);
			break;

			case Mc.Finish:
				ProcessFinish(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentMedicalTeat MyAgent
		{
			get
			{
				return (AgentMedicalTeat)base.MyAgent;
			}
		}
	}
}