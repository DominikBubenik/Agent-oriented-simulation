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

		//meta! sender="AgentBoss", id="23", type="Notice"
		public void ProcessTreatPatient(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			myMsg.Addressee = MySim.FindAgent(SimId.AgentTransition);
			myMsg.Code = Mc.EntranceTransition;
			Request(myMsg);
		}
		
		//meta! sender="AgentEntryExam", id="47", type="Response"
		public void ProcessEntryExamPatient(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			MyAgent.EnqueueAfterEntryExam(myMsg.Patient);
			
			// var patient = MyAgent.GetWaitingPatientForMedicalTreat(myMsg.Patient);
			myMsg.MedicalWaitingA = MyAgent.MedicalTreatQueueA.IsEmpty() ? null : MyAgent.MedicalTreatQueueA.Pop();
			myMsg.MedicalWaitingB = MyAgent.MedicalTreatQueueB.IsEmpty() ? null : MyAgent.MedicalTreatQueueB.Pop();
			myMsg.EntryWaiting = MyAgent.EntryQueue.IsEmpty() ? null : MyAgent.EntryQueue.Pop();
			myMsg.EntryQueueLength =  MyAgent.EntryQueue.Count;
			myMsg.MedicalQueueLengthB = MyAgent.MedicalTreatQueueB.Count;
			myMsg.Addressee = MySim.FindAgent(SimId.AgentResources);
			myMsg.Code = Mc.FreeUpResources;
			Notice(myMsg);
			
			//TODO premyslet
			
			// var initiateMTResources = (MyMessage)myMsg.CreateCopy();//new MyMessage(MySim);
			// initiateMTResources.Patient = patient;
			// initiateMTResources.Code = Mc.GetMedicalTreatResources;
			// initiateMTResources.Addressee = MySim.FindAgent(SimId.AgentResources);
			// Notice(initiateMTResources);
			//
			// //tu bude najskor medicaltreat a potom bude entry exam znovu
			// if (!MyAgent.EntryQueue.IsEmpty())
			// {
			// 	var initiateEntryExam = (MyMessage)myMsg.CreateCopy();//new MyMessage(MySim);
			// 	initiateEntryExam.Code = Mc.GetEntryExamResources;
			// 	initiateEntryExam.Addressee = MySim.FindAgent(SimId.AgentResources);
			// 	Notice(initiateEntryExam);
			// }
		
			
		}

		//meta! sender="AgentMedicalTeat", id="45", type="Response"
		public void ProcessMedicalTreatPatient(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			myMsg.EntryWaiting = MyAgent.EntryQueue.IsEmpty() ? null : MyAgent.EntryQueue.Pop();
			myMsg.MedicalWaitingA = MyAgent.MedicalTreatQueueA.IsEmpty() ? null : MyAgent.MedicalTreatQueueA.Pop();
			myMsg.MedicalWaitingB = MyAgent.MedicalTreatQueueB.IsEmpty() ? null : MyAgent.MedicalTreatQueueB.Pop();
			myMsg.EntryQueueLength =  MyAgent.EntryQueue.Count;
			myMsg.MedicalQueueLengthB = MyAgent.MedicalTreatQueueB.Count;
			myMsg.Addressee = MySim.FindAgent(SimId.AgentResources);
			myMsg.Code = Mc.FreeUpResources;
			Notice(myMsg);

			// if (!MyAgent.MedicalTreatQueueA.IsEmpty())
			// {
			// 	var initiateNext = (MyMessage)myMsg.CreateCopy();
			// 	initiateNext.Patient = MyAgent.MedicalTreatQueueA.Pop();
			// 	initiateNext.Code = Mc.GetMedicalTreatResources;
			// 	Notice(initiateNext);				
			// } 
			// if (!MyAgent.MedicalTreatQueueB.IsEmpty())
			// {
			// 	var initiateNext = (MyMessage)myMsg.CreateCopy();
			// 	initiateNext.Patient = MyAgent.MedicalTreatQueueB.Pop();
			// 	initiateNext.Code = Mc.GetMedicalTreatResources;
			// 	Notice(initiateNext);	
			// }
			// if (!MyAgent.EntryQueue.IsEmpty())
			// {
			// 	var initiateNext = (MyMessage)myMsg.CreateCopy();
			// 	// initiateNext.Patient = MyAgent.MedicalTreatQueueB.Pop();
			// 	initiateNext.Code = Mc.GetEntryExamResources;
			// 	Notice(initiateNext);
			// }
			
			var exitMsg = (MyMessage)myMsg.CreateCopy();
			exitMsg.Patient = myMsg.Patient;
			exitMsg.Addressee = MySim.FindAgent(SimId.AgentTransition);
			exitMsg.Code = Mc.ExitTransition;
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
		public void ProcessEntranceTransition(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			MyAgent.EnqueuePatientEntry(myMsg.Patient);
			myMsg.Code = Mc.GetEntryExamResources;
			myMsg.Addressee = MySim.FindAgent(SimId.AgentResources);
			Notice(myMsg);
		}

		//meta! sender="AgentTransition", id="138", type="Response"
		public void ProcessExitTransition(MessageForm message)
		{
			message.Code = Mc.PatientExit;
			message.Addressee = MyAgent.Parent;
			Notice(message);
		}

		//meta! sender="AgentResources", id="181", type="Notice"
		public void ProcessSendMedicalTreatResources(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			MyAgent.DequeuePatientMedicalTreat(myMsg);
			
			//tu treba pridat ten request response aby bolo jednoznacne odkial idu 
			myMsg.Addressee = MySim.FindAgent(SimId.AgentTransition);
			myMsg.Code = Mc.BetweenAmbulanceTransition;
			Request(myMsg);
		}

		//meta! sender="AgentResources", id="180", type="Notice"
		public void ProcessSendEntryExamResources(MessageForm message)
		{
			//tuto ho zoberiem z radu lebo az teraz sa priradia resource
			var myMsg = (MyMessage)message;
			MyAgent.DequeuePatientEntry(myMsg);
			
			myMsg.Addressee = MySim.FindAgent(SimId.AgentTransition);
			myMsg.Code = Mc.BetweenAmbulanceTransition;
			Request(myMsg);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.BetweenAmbulanceTransition:
				ProcessBetweenAmbulanceTransition(message);
			break;

			case Mc.SendEntryExamResources:
				ProcessSendEntryExamResources(message);
			break;

			case Mc.ExitTransition:
				ProcessExitTransition(message);
			break;

			case Mc.SendMedicalTreatResources:
				ProcessSendMedicalTreatResources(message);
			break;

			case Mc.EntranceTransition:
				ProcessEntranceTransition(message);
			break;

			case Mc.EntryExamPatient:
				ProcessEntryExamPatient(message);
			break;

			case Mc.TreatPatient:
				ProcessTreatPatient(message);
			break;

			case Mc.MedicalTreatPatient:
				ProcessMedicalTreatPatient(message);
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