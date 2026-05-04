using OSPABA;
using Simulation;
using Agents.AgentTransition;
using DISS_sem_3.Entities;

namespace Agents.AgentTransition.ContinualAssistants
{
	//meta! id="132"
	public class ExitDepartment : OSPABA.Process
	{
		public ExitDepartment(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentTransition", id="133", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			var duration = MyAgent.ExitSystemDuration();
			myMsg.Code = Mc.Finish;
			myMsg.Patient.PatientStatus = PatientStatus.Exiting;
			PointF[] config;
			if (MySim.AnimatorExists)
			{
				if (myMsg.Room != null)
				{
					var id = myMsg.Room.Id;
					config = myMsg.Room.IsTypeA() ? Config.PATH_ROOM_A_EXIT[id] : Config.PATH_ROOM_B_EXIT[id];
				}
				else
				{
					config = new[]
						{ myMsg.Patient.AnimObject.GetPosition(MySim.CurrentTime), Config.EXIT_ENTRANCE_POSITION };
				}

				myMsg.Patient.AnimObject.StartAnim(MySim.CurrentTime, duration, config);
			}
			if (MySim is MySimulation sim && sim.ObservationMode) 
				sim.NotifyLogger($"Patient {myMsg?.Patient.Name} exiting the system");
			Hold(duration, myMsg);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					var myMsg = (MyMessage)message;
					if (MySim.AnimatorExists) myMsg.Patient.AnimObject.Remove();
					myMsg.Addressee = MyAgent;
					if (MySim is MySimulation sim && sim.ObservationMode) 
						sim.NotifyLogger($"Patient {myMsg?.Patient.Name} Exit the system");
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