using OSPABA;
using Simulation;

namespace Agents.AgentEnviroment
{
	//meta! id="5"
	public class ManagerEnviroment : OSPABA.Manager
	{
		public ManagerEnviroment(int id, OSPABA.Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="AgentBoss", id="15", type="Notice"
		public void ProcessPatientExit(MessageForm message)
		{
			MyAgent.TreatedPatientsCount++;
		}

		//meta! sender="AmbulancePatient", id="18", type="Finish"
		public void ProcessFinishAmbulancePatient(MessageForm message)
		{
		}

		//meta! sender="RegularPatient", id="20", type="Finish"
		public void ProcessFinishRegularPatient(MessageForm message)
		{
			Console.WriteLine("pateint ");
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.PatientArrival:
					// Hold(TOTO_BUDE_NEJAKY_GEN, message.CreateCopy());
					message.Addressee = MyAgent.Parent;
					Notice(message);
					
					var newMessage = (MyMessage)message.CreateCopy();
					var id = newMessage.Patient.ArrivedByAmbulance ? SimId.AmbulancePatient : SimId.RegularPatient;
					newMessage.Addressee = MyAgent.FindAssistant(id);
					StartContinualAssistant(newMessage);
					break;
			}
		}

		//meta! sender="RegularPatient", id="100", type="Notice"
		public void ProcessPatientArrival(MessageForm message)
		{
			message.Addressee = MyAgent.Parent;
			Notice(message);
					
			var newMessage = (MyMessage)message.CreateCopy();
			Console.WriteLine($"pateint {newMessage.Patient.ToString()}");
			var id = newMessage.Patient.ArrivedByAmbulance ? SimId.AmbulancePatient : SimId.RegularPatient;
			newMessage.Addressee = MyAgent.FindAssistant(id);
			StartContinualAssistant(newMessage);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.AmbulancePatient:
					ProcessFinishAmbulancePatient(message);
				break;

				case SimId.RegularPatient:
					ProcessFinishRegularPatient(message);
				break;
				}
			break;

			case Mc.PatientExit:
				ProcessPatientExit(message);
			break;

			case Mc.PatientArrival:
				ProcessPatientArrival(message);
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