using OSPABA;
using Simulation;
using Agents.AgentEnviroment;
using DISS_sem_3.Entities;

namespace Agents.AgentEnviroment.ContinualAssistants
{
	//meta! id="17"
	public class AmbulancePatient : OSPABA.Scheduler
	{
		private int patientCounter;
		public AmbulancePatient(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
			patientCounter = 0;
		}

		//meta! sender="AgentEnviroment", id="18", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			myMsg.Code = Mc.Finish;
			
			Hold(MyAgent.GetNextAmbulance(), myMsg);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					Hold(MyAgent.GetNextAmbulance(), message.CreateCopy());
					
					var myMsg = (MyMessage)message;
					myMsg.Patient = new Patient(patientCounter, MySim, MySim.CurrentTime, true);
					MyAgent.PatientEntered(myMsg.Patient);
					myMsg.Patient.PatientStatus = PatientStatus.Entering;
					myMsg.Addressee = MyAgent;
					patientCounter++;
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
		public new AgentEnviroment MyAgent
		{
			get
			{
				return (AgentEnviroment)base.MyAgent;
			}
		}
	}
}