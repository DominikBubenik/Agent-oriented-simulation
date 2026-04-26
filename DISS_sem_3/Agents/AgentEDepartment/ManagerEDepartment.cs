using DISS_sem_3.Entities;
using MainLogic;
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
			var myMsg = (MyMessage)message;
			MyAgent.DequeuePatientMedicalTreat(myMsg);
			
			//tu treba pridat ten request response aby bolo jednoznacne odkial idu 
			myMsg.Addressee = MySim.FindAgent(SimId.AgentTransition);
			myMsg.Code = Mc.BetweenAmbulanceTransition;
			Request(myMsg);
		}

		//meta! sender="AgentBoss", id="23", type="Request"
		public void ProcessTreatPatient(MessageForm message)
		{
			var myMsg = (MyMessage)((MyMessage)message).CreateCopy();
			myMsg.Addressee = MySim.FindAgent(SimId.AgentTransition);
			myMsg.Code = Mc.EntranceTransmition;
			Request(myMsg);
		}
		
		//meta! sender="AgentEntryExam", id="47", type="Response"
		public void ProcessEntryExamPatient(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			MyAgent.EnqueueAfterEntryExam(myMsg.Patient);
			var patient = MyAgent.GetWaitingPatientForMedicalTreat(myMsg.Patient);
			myMsg.Addressee = MySim.FindAgent(SimId.AgentResources);
			myMsg.Code = Mc.FreeUpResources;
			Notice(myMsg);

			//tu bude najskor medicaltreat a potom bude entry exam znovu
			if (!MyAgent.EntryQueue.IsEmpty())
			{
				var initiateEntryExam = new MyMessage(MySim);
				initiateEntryExam.Addressee = MySim.FindAgent(SimId.AgentResources);
				initiateEntryExam.Code = Mc.EntryExamResources;
				Request(initiateEntryExam);
			}
			
			var initiateMTResources = new MyMessage(MySim);
			initiateMTResources.Patient = patient;
			initiateMTResources.Addressee = MySim.FindAgent(SimId.AgentResources);
			initiateMTResources.Code = Mc.MedicalTreatResources;
			Request(initiateMTResources);
		}

		//meta! sender="AgentResources", id="44", type="Response"
		public void ProcessEntryExamResources(MessageForm message)
		{
			//tuto ho zoberiem z radu lebo az teraz sa priradia resource
			var myMsg = (MyMessage)message;
			MyAgent.DequeuePatientEntry(myMsg);
			
			myMsg.Addressee = MySim.FindAgent(SimId.AgentTransition);
			myMsg.Code = Mc.BetweenAmbulanceTransition;
			Request(myMsg);
		}

		//meta! sender="AgentMedicalTeat", id="45", type="Response"
		public void ProcessMedicalTreatPatient(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			myMsg.Addressee = MySim.FindAgent(SimId.AgentResources);
			myMsg.Code = Mc.FreeUpResources;
			Notice(myMsg);
			
			var exitMsg = new  MyMessage(MySim);
			exitMsg.Patient = myMsg.Patient;
			exitMsg.Addressee = MySim.FindAgent(SimId.AgentTransition);
			exitMsg.Code = Mc.ExitTransmition;
			Request(exitMsg);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentTransition", id="111", type="Response"
		public void ProcessBetweenAmbulanceTransition(MessageForm message)
		{
			var myMsg = (MyMessage)message; //odstranil som copy
			if (myMsg.Doctor == null)
			{
				myMsg.Addressee = MySim.FindAgent(SimId.AgentEntryExam);
				myMsg.Code = Mc.EntryExamPatient;
			}
			else
			{
				myMsg.Addressee = MySim.FindAgent(SimId.AgentMedicalTeat);
				myMsg.Code = Mc.MedicalTreatPatient;
			}
			Request(myMsg);
		}

		//meta! sender="AgentTransition", id="136", type="Response"
		public void ProcessEntranceTransmition(MessageForm message)
		{
			var myMsg = (MyMessage)((MyMessage)message).CreateCopy();
			MyAgent.EnqueuePatientEntry(myMsg.Patient);
			myMsg.Addressee = MySim.FindAgent(SimId.AgentResources);
			myMsg.Code = Mc.EntryExamResources;
			Request(myMsg);
		}

		//meta! sender="AgentTransition", id="138", type="Response"
		public void ProcessExitTransmition(MessageForm message)
		{
			message.Code = Mc.TreatPatient;
			message.Addressee = MyAgent.Parent;
			Response(message);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.EntryExamResources:
				ProcessEntryExamResources(message);
			break;

			case Mc.TreatPatient:
				ProcessTreatPatient(message);
			break;

			case Mc.MedicalTreatPatient:
				ProcessMedicalTreatPatient(message);
			break;

			case Mc.EntryExamPatient:
				ProcessEntryExamPatient(message);
			break;

			case Mc.EntranceTransmition:
				ProcessEntranceTransmition(message);
			break;

			case Mc.BetweenAmbulanceTransition:
				ProcessBetweenAmbulanceTransition(message);
			break;

			case Mc.MedicalTreatResources:
				ProcessMedicalTreatResources(message);
			break;

			case Mc.ExitTransmition:
				ProcessExitTransmition(message);
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