using OSPABA;
using Simulation;
using Agents.AgentResources.InstantAssistants;
using DISS_sem_3.Entities;

namespace Agents.AgentResources
{
	//meta! id="34"
	public class AgentResources : OSPABA.Agent
	{
		public List<Doctor>  Doctors { get; set; }
		public List<Nurse> Nurses { get; set; }
		public List<Room> RoomsTypeA { get; set; }
		public List<Room> RoomsTypeB { get; set; }
		
		public Queue<MyMessage> WaitingForEntryExam { get; set; }
		
		public AgentResources(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
			Doctors = new List<Doctor>();
			for (int i = 0; i < MyCastSim().InitDoctorCount; i++)
			{
				Doctors.Add(new Doctor(i, MySim));
			}

			Nurses = new List<Nurse>();
			for (int i = 0; i < MyCastSim().InitNurseCount; i++)
			{
				Nurses.Add(new Nurse(i, MySim));
			}
			RoomsTypeA = new List<Room>();
			for (int i = 0; i < MyCastSim().InitRoomACount; i++)
			{
				RoomsTypeA.Add(new Room(i, MySim, 'A'));
			}
			RoomsTypeB = new List<Room>();
			for (int i = 0; i < MyCastSim().InitRoomBCount; i++)
			{
				RoomsTypeB.Add(new Room(i, MySim, 'B'));
			}
			
			WaitingForEntryExam = new Queue<MyMessage>();
		}

		private MySimulation MyCastSim() => (MySimulation)MySim; 

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerResources(SimId.ManagerResources, MySim, this);
			new AllocateMedicalTreatRes(SimId.AllocateMedicalTreatRes, MySim, this);
			new AllocateEntryExamRes(SimId.AllocateEntryExamRes, MySim, this);
			AddOwnMessage(Mc.FreeUpResources);
			AddOwnMessage(Mc.MedicalTreatResources);
			AddOwnMessage(Mc.EntryExamResources);
		}
		//meta! tag="end"
	}
}