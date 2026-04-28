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
			var duration = MyAgent.GetAllRoomTransferDuration();
			if (myMsg.Patient != null)
			{
				myMsg.Patient.PatientStatus = PatientStatus.Moving;
				if (MySim.AnimatorExists)
				{
					myMsg.Patient.AnimObject.StartAnim(MySim.CurrentTime, duration, Config.PATH_ENTRY_QUEUE_TO_ROOM_B[myMsg.Room.Id]);
				}
			}
			if (myMsg.Nurse != null)
			{
				myMsg.Nurse.Activity = StaffActivity.Moving;
				if (MySim.AnimatorExists)
				{
					myMsg.Nurse.AnimObject.StartAnim(MySim.CurrentTime, duration, Config.PATH_MEDICAL_STAFF_TO_ROOM[myMsg.Room.Id]);
				}
			}
			if (myMsg.Doctor != null)
			{
				myMsg.Doctor.Activity = StaffActivity.Moving;
				if (MySim.AnimatorExists)
				{
					myMsg.Doctor.AnimObject.StartAnim(MySim.CurrentTime, duration, Config.PATH_MEDICAL_STAFF_TO_ROOM[myMsg.Room.Id]);
				}
			}
			Hold(duration, myMsg);
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