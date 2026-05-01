using DISS_sem_3.Entities;
using MainLogic;
using OSPABA;
using Simulation;

namespace Agents.AgentResources
{
	//meta! id="34"
	public class ManagerResources : OSPABA.Manager
	{
		public ManagerResources(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentEDepartment", id="79", type="Notice"
		public void ProcessFreeUpResources(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			MyAgent.FreeUpResources(myMsg);
			if (myMsg.MedicalWaitingA != null && MyAgent.FreeResForMedicalA())
			{
				myMsg.Nurse = null;
				myMsg.Room = null;
				myMsg.Doctor = null;
				myMsg.Patient =  myMsg.MedicalWaitingA;
				// GlobalLogger.PrintLog(" looking for some resources", MySim.CurrentTime);
				((Adviser)MyAgent.FindAssistant(SimId.AllocateMedicalTreatRes)).Execute(myMsg);
				if (myMsg.Nurse != null && myMsg.Room != null && myMsg.Doctor != null)
				{
					myMsg.Code = Mc.SendMedicalTreatResources;
					myMsg.Addressee = MyAgent.Parent;
					Notice(myMsg);
				}
			}
			if (myMsg.MedicalWaitingB != null)
			{
				var canContinue = false;
				if (myMsg.MedicalWaitingB.Priority < 5 && MyAgent.FreeResForMedicalAB())
				{
					canContinue = true;
				}

				if (MyAgent.FreeResForMedicalB())
				{
					canContinue = true;
				}

				if (canContinue)
				{
					var initiate = (MyMessage)myMsg.CreateCopy();
					initiate.Nurse = null;
					initiate.Room = null;
					initiate.Doctor = null;
					initiate.Patient = initiate.MedicalWaitingB;
					// GlobalLogger.PrintLog(" looking for some resources", MySim.CurrentTime);
					((Adviser)MyAgent.FindAssistant(SimId.AllocateMedicalTreatRes)).Execute(initiate);
					if (initiate.Nurse != null && initiate.Room != null && initiate.Doctor != null)
					{
						
						initiate.Code = Mc.SendMedicalTreatResources;
						initiate.Addressee = MyAgent.Parent;
						Notice(initiate);
					}	
				}
			}
			if (myMsg.EntryWaiting != null && MyAgent.FreeResForEntryExam())
			{
				var initiate = (MyMessage)myMsg.CreateCopy();
				initiate.Nurse = null;
				initiate.Room = null;
				initiate.Doctor = null;
				initiate.Patient = initiate.EntryWaiting;
				// GlobalLogger.PrintLog(" looking for some resources", MySim.CurrentTime);
				((Adviser)MyAgent.FindAssistant(SimId.AllocateEntryExamRes)).Execute(initiate);
				if (initiate.Nurse != null && initiate.Room != null)
				{
					initiate.Code = Mc.SendEntryExamResources;
					initiate.Addressee = MyAgent.Parent;
					Notice(initiate);
				}	
			}
		}

		//meta! sender="AgentEDepartment", id="49", type="Notice"
		public void ProcessGetMedicalTreatResources(MessageForm message)
		{
			var myMsg = (MyMessage)message;

			myMsg.Nurse = null;
			myMsg.Room = null;
			myMsg.Doctor = null;
			// GlobalLogger.PrintLog(" looking for some resources", MySim.CurrentTime);
			((Adviser)MyAgent.FindAssistant(SimId.AllocateMedicalTreatRes)).Execute(myMsg);
			if (myMsg.Nurse != null && myMsg.Room != null && myMsg.Doctor != null)
			{
				myMsg.Code = Mc.SendMedicalTreatResources;
				myMsg.Addressee = MyAgent.Parent;
				Notice(myMsg);
			}
			else
			{
				// GlobalLogger.PrintLog( " no resources for medical treat", MySim.CurrentTime);
			}
		}

		//meta! sender="AgentEDepartment", id="44", type="Notice"
		public void ProcessGetEntryExamResources(MessageForm message)
		{
			var myMsg = (MyMessage)message;

			myMsg.Nurse = null;
			myMsg.Doctor = null;
			myMsg.Room = null;
			// GlobalLogger.PrintLog(" looking for some resources", MySim.CurrentTime);
			((Adviser)MyAgent.FindAssistant(SimId.AllocateEntryExamRes)).Execute(myMsg);
			if (myMsg.Nurse != null && myMsg.Room != null)
			{
				myMsg.Code = Mc.SendEntryExamResources;
				myMsg.Addressee = MyAgent.Parent;
				Notice(myMsg);
			}
			else
			{
				// GlobalLogger.PrintLog( " no resources", MySim.CurrentTime);
			}
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
			case Mc.GetMedicalTreatResources:
				ProcessGetMedicalTreatResources(message);
			break;

			case Mc.GetEntryExamResources:
				ProcessGetEntryExamResources(message);
			break;

			case Mc.FreeUpResources:
				ProcessFreeUpResources(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentResources MyAgent
		{
			get
			{
				return (AgentResources)base.MyAgent;
			}
		}
	}
}