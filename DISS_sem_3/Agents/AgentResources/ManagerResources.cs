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
		}

		//meta! sender="AgentEDepartment", id="49", type="Request"
		public void ProcessMedicalTreatResources(MessageForm message)
		{
			var myMsg = (MyMessage)message;

			myMsg.Nurse = null;
			myMsg.Room = null;
			myMsg.Doctor = null;
			GlobalLogger.PrintLog(" looking for some resources", MySim.CurrentTime);
			((Adviser)MyAgent.FindAssistant(SimId.AllocateMedicalTreatRes)).Execute(myMsg);
			if (myMsg.Nurse != null && myMsg.Room != null && myMsg.Doctor != null)
			{
				myMsg.Code = Mc.MedicalTreatResources;
				Response(myMsg);
			}
			else
			{
				GlobalLogger.PrintLog( " no resources for medical treat", MySim.CurrentTime);
			}
		}

		//meta! sender="AgentEDepartment", id="44", type="Request"
		public void ProcessEntryExamResources(MessageForm message)
		{
			var myMsg = (MyMessage)message;

			myMsg.Nurse = null;
			myMsg.Doctor = null;
			myMsg.Room = null;
			GlobalLogger.PrintLog(" looking for some resources", MySim.CurrentTime);
			((Adviser)MyAgent.FindAssistant(SimId.AllocateEntryExamRes)).Execute(myMsg);
			if (myMsg.Nurse != null && myMsg.Room != null)
			{
				myMsg.Code = Mc.EntryExamResources;
				Response(myMsg);
			}
			else
			{
				GlobalLogger.PrintLog( " no resources", MySim.CurrentTime);
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
			case Mc.MedicalTreatResources:
				ProcessMedicalTreatResources(message);
			break;

			case Mc.EntryExamResources:
				ProcessEntryExamResources(message);
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