using OSPABA;
using Simulation;
using Agents.AgentTransition;
using DISS_sem_3.Entities;
using MainLogic;

namespace Agents.AgentTransition.ContinualAssistants
{
	//meta! id="118"
	public class AllRoomTransfer : OSPABA.Process
	{
		public AllRoomTransfer(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentTransition", id="119", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			myMsg.Code = Mc.Finish;
			if (myMsg.Nurse != null) myMsg.Nurse.Activity = StaffActivity.Moving;
			if (myMsg.Doctor != null) myMsg.Doctor.Activity = StaffActivity.Moving;
			Hold(MyAgent.GetAllRoomTransferDuration(), myMsg);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var myMsg = (MyMessage)message;
					myMsg.Addressee = MyAgent;
					AssistantFinished(myMsg);
					break;
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
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