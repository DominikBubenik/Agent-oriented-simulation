using OSPABA;
using Simulation;
using Agents.AgentEnviroment;
using DISS_sem_3.Entities;

namespace Agents.AgentEnviroment.ContinualAssistants
{
	//meta! id="19"
	public class RegularPatient : OSPABA.Scheduler
	{
		public readonly int TOTO_BUDE_NEJAKY_GEN = 5;
		private int patientCounter;
		public RegularPatient(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
			patientCounter = 0;
		}

		//meta! sender="AgentEnviroment", id="20", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			myMsg.Code = Mc.PatientArrival;
			
			Hold(TOTO_BUDE_NEJAKY_GEN, myMsg);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.PatientArrival:
					Hold(TOTO_BUDE_NEJAKY_GEN, message.CreateCopy());
					
					var myMsg = (MyMessage)message;
					myMsg.Patient = new Patient(patientCounter, MySim, MySim.CurrentTime, false);
					myMsg.Addressee = MyAgent;
					myMsg.Code = Mc.Finish;
					patientCounter++;
					Notice(myMsg);
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