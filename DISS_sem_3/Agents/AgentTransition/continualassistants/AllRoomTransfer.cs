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
			var duration = 0.0;
			var maxDuration = duration;
			if (myMsg.Patient != null && !myMsg.Room.Equals(myMsg.Patient.CurrentRoom))
			{
				duration = MyAgent.GetAllRoomTransferDuration();
				maxDuration = duration;
				// myMsg.Patient.CurrentRoom = myMsg.Room;
				myMsg.Patient.PatientStatus = PatientStatus.Moving;
				if (MySim.AnimatorExists)
				{
					var config = Config.GetPathFromAmbulanceToAmbulance(myMsg.Patient.CurrentRoom, myMsg.Room);
					myMsg.Patient.AnimObject.StartAnim(MySim.CurrentTime, duration, config);
				}
			}
			if (myMsg.Nurse != null && !myMsg.Room.Equals(myMsg.Nurse.CurrentRoom))
			{
				myMsg.Nurse.StartTransfer();
				duration = MyAgent.GetAllRoomTransferDuration();
				if (duration > maxDuration) maxDuration = duration; 
				if (MySim.AnimatorExists)
				{
					MyAgent.StartAnimationMedicalStaff(duration, myMsg.Room, myMsg.Nurse);
				}
				if (myMsg.Nurse.CurrentRoom != null) myMsg.Nurse.CurrentRoom.Nurse = null;
				myMsg.Nurse.CurrentRoom = myMsg.Room;
				myMsg.Room.Nurse =  myMsg.Nurse;
			}
			if (myMsg.Doctor != null && !myMsg.Room.Equals(myMsg.Doctor.CurrentRoom))
			{
				duration = MyAgent.GetAllRoomTransferDuration();
				if (duration > maxDuration) maxDuration = duration; 
				if (MySim.AnimatorExists)
				{
					MyAgent.StartAnimationMedicalStaff(duration, myMsg.Room, myMsg.Doctor);
				}
				
				myMsg.Doctor.StartTransfer();
				if (myMsg.Doctor.CurrentRoom != null) myMsg.Doctor.CurrentRoom.Doctor = null; 
				myMsg.Doctor.CurrentRoom = myMsg.Room;
				// myMsg.Doctor.CurrentRoom.Doctor = myMsg.Doctor;
				myMsg.Room.Doctor =  myMsg.Doctor;
			}
			if (MySim is MySimulation sim && sim.ObservationMode) 
				sim.NotifyLogger($"Ambulace transition started P> {myMsg?.Patient.Name}; D> {myMsg?.Doctor?.Id}, N> {myMsg?.Nurse.Id}, R> {myMsg?.Room.Id}");
			Hold(maxDuration, myMsg);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var myMsg = (MyMessage)message;
					myMsg.Addressee = MyAgent;
					myMsg.Patient.CurrentRoom = myMsg.Room;
					
					if (myMsg.Nurse.Activity == StaffActivity.Moving) myMsg.Nurse.StopTransfer();
					myMsg.Nurse.CurrentRoom = myMsg.Room;
					
					if (myMsg.Doctor.Activity == StaffActivity.Moving) myMsg.Doctor.StopTransfer();
					myMsg.Doctor.CurrentRoom = myMsg.Room;
					
					if (MySim.AnimatorExists) MyAgent.SetPositionsInRoom(myMsg);
					if (MySim is MySimulation sim && sim.ObservationMode) 
						sim.NotifyLogger($"Ambulace transition finished P> {myMsg?.Patient.Name}; D> {myMsg?.Doctor?.Id}, N> {myMsg?.Nurse.Id}, R> {myMsg?.Room.Id}");
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