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
			var msg = (MyMessage)message;
			msg.Addressee = MyAgent.FindAssistant(SimId.AllRoomTransfer);
			msg.Code = Mc.Start;
			StartContinualAssistant(msg);
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
			var myMsg = (MyMessage)message;
			myMsg.Addressee = MyAgent.Parent;
			myMsg.Code = Mc.BetweenAmbulanceTransition;
			Response(myMsg);
		}

		//meta! sender="EntryPatient", id="129", type="Finish"
		public void ProcessFinishEntryPatient(MessageForm message)
		{
			var myMsg =  (MyMessage)message;
			myMsg.Code = Mc.EntranceTransition;
			Response(myMsg);
		}

		//meta! sender="ExitDepartment", id="133", type="Finish"
		public void ProcessFinishExitDepartment(MessageForm message)
		{
			var myMsg =  (MyMessage)message;
			myMsg.Code = Mc.ExitTransition;
			myMsg.Addressee = MyAgent.Parent;
			Response(myMsg);
		}

		//meta! sender="AgentEDepartment", id="136", type="Request"
		public void ProcessEntranceTransition(MessageForm message)
		{
			var msg = (MyMessage)message;
			msg.Addressee = MyAgent.FindAssistant(SimId.EntryPatient);
			msg.Code = Mc.Start;
			StartContinualAssistant(msg);
		}

		//meta! sender="AgentEDepartment", id="138", type="Request"
		public void ProcessExitTransition(MessageForm message)
		{
			var msg = (MyMessage)message;
			msg.Addressee = MyAgent.FindAssistant(SimId.ExitDepartment);
			msg.Code = Mc.Start;
			StartContinualAssistant(msg);
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
				case SimId.EntryPatient:
					ProcessFinishEntryPatient(message);
				break;

				case SimId.ExitDepartment:
					ProcessFinishExitDepartment(message);
				break;

				case SimId.AllRoomTransfer:
					ProcessFinishAllRoomTransfer(message);
				break;
				}
			break;

			case Mc.BetweenAmbulanceTransition:
				ProcessBetweenAmbulanceTransition(message);
			break;

			case Mc.ExitTransition:
				ProcessExitTransition(message);
			break;

			case Mc.EntranceTransition:
				ProcessEntranceTransition(message);
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