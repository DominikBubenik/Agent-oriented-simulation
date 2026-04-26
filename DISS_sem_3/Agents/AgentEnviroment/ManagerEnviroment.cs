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
			var myMsg = (MyMessage)message;
			var patient = myMsg.Patient; 
			if (patient.ArrivedByAmbulance)
			{
				MyAgent.TimeInSystemAmbulancePatient.AddSample(MySim.CurrentTime - patient.ArrivalTime);
			}
			else
			{
				MyAgent.TimeInSystemWalkInPatient.AddSample(MySim.CurrentTime - patient.ArrivalTime);
			}
			
			MyAgent.TreatedPatientsCount++;
		}

		//meta! sender="AmbulancePatient", id="18", type="Finish"
		public void ProcessFinishAmbulancePatient(MessageForm message)
		{
			message.Addressee = MyAgent.Parent;
			message.Code = Mc.PatientArrival;
			Notice(message);
			
			var newMessage = (MyMessage)message.CreateCopy();
			Console.WriteLine($"pateint {newMessage.Patient.ToString()}");
		}

		//meta! sender="RegularPatient", id="20", type="Finish"
		public void ProcessFinishRegularPatient(MessageForm message)
		{
			message.Addressee = MyAgent.Parent;
			message.Code = Mc.PatientArrival;
			Notice(message);
			
			var newMessage = (MyMessage)message.CreateCopy();
			Console.WriteLine($"pateint {newMessage.Patient.ToString()}");
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
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