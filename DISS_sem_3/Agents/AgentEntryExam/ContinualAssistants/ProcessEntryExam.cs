using OSPABA;
using Simulation;
using Agents.AgentEntryExam;
using DISS_sem_3.Entities;
using MainLogic;

namespace Agents.AgentEntryExam.ContinualAssistants
{
	//meta! id="52"
	public class ProcessEntryExam : OSPABA.Process
	{
		public ProcessEntryExam(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentEntryExam", id="53", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			myMsg.Room.Nurse = myMsg.Nurse;
			myMsg.Nurse.StartWork();
			
			myMsg.Room.Patient = myMsg.Patient;
			myMsg.Room.Patient.PatientStatus = PatientStatus.EntryExam;
			
			var duration = myMsg.Patient.ArrivedByAmbulance ? MyAgent.GetAmbulanceExamDuration() : MyAgent.GetWalkInExamDuration();
			myMsg.Code = Mc.Finish;
			Hold(duration, myMsg);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var myMsg = (MyMessage)message;
					MyAgent.AssignPriority(myMsg.Patient);
					myMsg.Patient.PatientStatus = PatientStatus.MedicalWait;
					myMsg.Nurse.StopWork();
					myMsg.Room.StopOccupancy();
					MyAgent.SetPositionAfterExam(myMsg);
					// myMsg.Addressee = MyAgent; asi zbytocneee
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
		public new AgentEntryExam MyAgent
		{
			get
			{
				return (AgentEntryExam)base.MyAgent;
			}
		}
	}
}