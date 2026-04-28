using OSPABA;
using Simulation;
using Agents.AgentTransition;
using DISS_sem_3.Entities;
using MainLogic;

namespace Agents.AgentTransition.ContinualAssistants
{
	//meta! id="128"
	public class EntryPatient : OSPABA.Process
	{
		public EntryPatient(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentTransition", id="129", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			var duration = myMsg.Patient.ArrivedByAmbulance ? MyAgent.EntranceAmbulancePatientDuration() : MyAgent.EntranceWalkInPatientDuration();
			myMsg.Code = Mc.Finish;
			if (myMsg.Patient != null)
			{
				myMsg.Patient.PatientStatus = PatientStatus.Entering;
				if (MySim.AnimatorExists)
				{
					var config = myMsg.Patient.ArrivedByAmbulance ? Config.PATH_AMBULANCE_ENTRY_TO_QUEUE : Config.PATH_WALK_IN_ENTRY_TO_QUEUE;
					myMsg.Patient.AnimObject.StartAnim(MySim.CurrentTime, duration, config);
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

		private MySimulation MyCastSim() => (MySimulation)MySim;
	}
}
