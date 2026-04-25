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
		public List<Room> FreeRoomsTypeA { get; set; }
		public List<Room> AllRoomsTypeA { get; set; }
		public List<Room> FreeRoomsTypeB { get; set; }
		public List<Room> AllRoomsTypeB { get; set; }
		
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
			FreeRoomsTypeA = new List<Room>();
			AllRoomsTypeA = new List<Room>();
			for (int i = 0; i < MyCastSim().InitRoomACount; i++)
			{
				var room = new Room(i, MySim, 'A');
				FreeRoomsTypeA.Add(room);
				AllRoomsTypeA.Add(room);
			}
			FreeRoomsTypeB = new List<Room>();
			AllRoomsTypeB = new List<Room>();
			for (int i = 0; i < MyCastSim().InitRoomBCount; i++)
			{
				var room = new Room(i, MySim, 'B');
				FreeRoomsTypeB.Add(room);
				AllRoomsTypeB.Add(room);
			}
		}
		
		public void FreeUpResources(MyMessage myMsg)
		{
			if (myMsg.Nurse != null) Nurses.Add(myMsg.Nurse);
			if (myMsg.Doctor != null) Doctors.Add(myMsg.Doctor);
			if (myMsg.Room != null)
			{
				if (myMsg.Room.Type == 'A')
				{
					FreeRoomsTypeA.Add(myMsg.Room);
				}
				else
				{
					FreeRoomsTypeB.Add(myMsg.Room);
				}
				myMsg.Room.Nurse = null;
				myMsg.Room.Doctor = null;
				myMsg.Room.Patient = null;
			}
			
			myMsg.Nurse = null;
			myMsg.Room = null;
			myMsg.Doctor = null;
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