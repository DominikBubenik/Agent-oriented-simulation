using OSPABA;
using Simulation;
using Agents.AgentMedicalTeat;
using DISS_sem_3.Entities;

namespace Agents.AgentMedicalTeat.ContinualAssistants
{
	//meta! id="55"
	public class ProcessMedicalTreat : OSPABA.Process
	{
		public ProcessMedicalTreat(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentMedicalTeat", id="56", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			myMsg.Room.Nurse = myMsg.Nurse;
			// myMsg.Room.Patient = myMsg.Patient;
			// myMsg.Room.Doctor = myMsg.Doctor;
			
			myMsg.Nurse.StartWork();
			myMsg.Doctor.StartWork();
			
			myMsg.Patient.PatientStatus = PatientStatus.MedicalExam;
			if (MySim is MySimulation sim && sim.ObservationMode) 
				sim.NotifyLogger($"Medical treat started P> {myMsg.Patient.Name}; D> {myMsg.Doctor.Id}, N> {myMsg.Nurse.Id}, R> {myMsg.Room.Id}");
			
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
					myMsg.Addressee = MyAgent;
					myMsg.Nurse.StopWork();
					myMsg.Doctor.StopWork();
					myMsg.Patient.PatientStatus = PatientStatus.Exiting;
					myMsg.Room.StopOccupancy();
					if (MySim is MySimulation sim && sim.ObservationMode) 
						sim.NotifyLogger($"Medical treat finished P> {myMsg.Patient.Name}; D> {myMsg.Doctor.Id}, N> {myMsg.Nurse.Id}, R> {myMsg.Room.Id}");
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
		public new AgentMedicalTeat MyAgent
		{
			get
			{
				return (AgentMedicalTeat)base.MyAgent;
			}
		}
	}
}