using OSPABA;
using Simulation;
using Agents.AgentTransition;
using MainLogic;

namespace Agents.AgentTransition.ContinualAssistants
{
	//meta! id="128"
	public class EntryPatient : OSPABA.Process
	{
		private TriangularGenerator _walkInDuration;
		private ContinuousGenerator _ambulanceDuration;
		public EntryPatient(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			_walkInDuration = new TriangularGenerator(MyCastSim().NextSeed(), 120, 300, 150);
			_ambulanceDuration = new ContinuousGenerator(new Random(MyCastSim().NextSeed()), [new GenSpec(1, 90, 200)]);
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
			var duration = myMsg.Patient.ArrivedByAmbulance ? _ambulanceDuration.Sample() : _walkInDuration.Generate();
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
