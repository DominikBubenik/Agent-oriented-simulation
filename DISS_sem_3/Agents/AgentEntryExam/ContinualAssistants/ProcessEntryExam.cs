using OSPABA;
using Simulation;
using Agents.AgentEntryExam;
using MainLogic;

namespace Agents.AgentEntryExam.ContinualAssistants
{
	//meta! id="52"
	public class ProcessEntryExam : OSPABA.Process
	{
		private ContinuousGenerator _entryExamWalkInDuration;
		public ProcessEntryExam(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			var specsWalkIn = new List<GenSpec>()
			{
				new GenSpec(0.6, 3, 5),
				new GenSpec(0.4, 5, 9)
			};
			_entryExamWalkInDuration = new ContinuousGenerator(new Random(MyCastSim().NextSeed()), specsWalkIn);
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
			myMsg.Room.Patient = myMsg.Patient;
			var duration = _entryExamWalkInDuration.Sample();
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
		
		private MySimulation MyCastSim() => (MySimulation)MySim;
	}
}