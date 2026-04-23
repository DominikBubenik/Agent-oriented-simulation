using OSPABA;
using Simulation;

namespace Agents.AgentEDepartment
{
	//meta! id="21"
	public class ManagerEDepartment : OSPABA.Manager
	{
		public ManagerEDepartment(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentResources", id="49", type="Response"
		public void ProcessMedicalTreatResources(MessageForm message)
		{
		}

		//meta! sender="AgentBoss", id="23", type="Request"
		public void ProcessTreatPatient(MessageForm message)
		{
			var myMsg = ((MyMessage)message).CreateCopy();
		}

		//meta! sender="AgentEntryExam", id="47", type="Response"
		public void ProcessEntryExamPatient(MessageForm message)
		{
		}

		//meta! sender="AgentResources", id="44", type="Response"
		public void ProcessEntryExamResources(MessageForm message)
		{
			//tuto ho zoberiem z radu lebo az teraz sa priradia resource
		}

		//meta! sender="AgentMedicalTeat", id="45", type="Response"
		public void ProcessMedicalTreatPatient(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentTransition", id="112", type="Response"
		public void ProcessMedicalTreatTransition(MessageForm message)
		{
		}

		//meta! sender="AgentTransition", id="111", type="Response"
		public void ProcessEntryExamTransition(MessageForm message)
		{
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.MedicalTreatResources:
				ProcessMedicalTreatResources(message);
			break;

			case Mc.EntryExamTransition:
				ProcessEntryExamTransition(message);
			break;

			case Mc.MedicalTreatPatient:
				ProcessMedicalTreatPatient(message);
			break;

			case Mc.EntryExamPatient:
				ProcessEntryExamPatient(message);
			break;

			case Mc.EntryExamResources:
				ProcessEntryExamResources(message);
			break;

			case Mc.MedicalTreatTransition:
				ProcessMedicalTreatTransition(message);
			break;

			case Mc.TreatPatient:
				ProcessTreatPatient(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentEDepartment MyAgent
		{
			get
			{
				return (AgentEDepartment)base.MyAgent;
			}
		}
	}
}