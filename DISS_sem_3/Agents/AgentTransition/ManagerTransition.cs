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
		
		//meta! sender="AgentEDepartment", id="111", type="Request"
		public void ProcessBetweenAmbulanceTransition(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}
		
		//meta! sender="AllRoomTransfer", id="119", type="Finish"
		public void ProcessFinishAllRoomTransfer(MessageForm message)
		{
		}

		//meta! sender="EntryWalkInPatient", id="129", type="Finish"
		public void ProcessFinishEntryWalkInPatient(MessageForm message)
		{
		}

		//meta! sender="ExitDepartment", id="133", type="Finish"
		public void ProcessFinishExitDepartment(MessageForm message)
		{
		}

		//meta! sender="EntryAmbulancePatient", id="131", type="Finish"
		public void ProcessFinishEntryAmbulancePatient(MessageForm message)
		{
		}

		//meta! sender="AgentEDepartment", id="136", type="Request"
		public void ProcessEntranceTransmition(MessageForm message)
		{
			var msg = (MyMessage)message;
			msg.Addressee = MyAgent.FindAssistant(msg.Patient.ArrivedByAmbulance ? SimId.EntryAmbulancePatient : SimId.EntryWalkInPatient);

			msg.Code = Mc.Start;
			StartContinualAssistant(msg);
		}

		//meta! sender="AgentEDepartment", id="138", type="Request"
		public void ProcessExitTransmition(MessageForm message)
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
			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.EntryWalkInPatient:
					ProcessFinishEntryWalkInPatient(message);
				break;

				case SimId.AllRoomTransfer:
					ProcessFinishAllRoomTransfer(message);
				break;

				case SimId.ExitDepartment:
					ProcessFinishExitDepartment(message);
				break;

				case SimId.EntryAmbulancePatient:
					ProcessFinishEntryAmbulancePatient(message);
				break;
				}
			break;

			case Mc.EntranceTransmition:
				ProcessEntranceTransmition(message);
			break;

			case Mc.ExitTransmition:
				ProcessExitTransmition(message);
			break;

			case Mc.BetweenAmbulanceTransition:
				ProcessBetweenAmbulanceTransition(message);
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