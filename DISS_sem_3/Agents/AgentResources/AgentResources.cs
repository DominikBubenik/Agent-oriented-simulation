using OSPABA;
using Simulation;
using Agents.AgentResources.InstantAssistants;
using Agents.AgentResources.ContinualAssistants;
using DISS_sem_3;
using DISS_sem_3.Entities;
using OpenTK.Platform.Windows;

namespace Agents.AgentResources
{
	//meta! id="34"
	public class AgentResources : OSPABA.Agent
	{
		public List<Doctor> Doctors { get; set; }
		public List<Doctor> AllDoctors { get; set; }
		public List<Nurse> Nurses { get; set; }
		public List<Nurse> AllNurses { get; set; }
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
			AllDoctors = new List<Doctor>();
			for (int i = 0; i < MyCastSim().InitDoctorCount; i++)
			{
				var doctor = new Doctor(i, MySim); 
				Doctors.Add(doctor);
				AllDoctors.Add(doctor);
			}

			Nurses = new List<Nurse>();
			AllNurses = new List<Nurse>();
			for (int i = 0; i < MyCastSim().InitNurseCount; i++)
			{
				var nurse = new Nurse(i, MySim); 
				Nurses.Add(nurse);
				AllNurses.Add(nurse);
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
			if (myMsg.Nurse != null)
			{
				myMsg.Nurse.Activity = StaffActivity.Not_Working;
				Nurses.Add(myMsg.Nurse);
			}

			if (myMsg.Doctor != null)
			{
				myMsg.Doctor.Activity = StaffActivity.Not_Working;
				Doctors.Add(myMsg.Doctor);
			}
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
				// myMsg.Room.Nurse = null;
				// myMsg.Room.Doctor = null;
				myMsg.Room.Patient = null;
				myMsg.Room.CurrentStatus = RoomStatus.Free;
			}
			
			myMsg.Nurse = null;
			myMsg.Room = null;
			myMsg.Doctor = null;
		}

		public double GetUtilAllDoctors()
		{
			var cumulativeUtil = 0.0;
			foreach (var doc in AllDoctors)
			{
				cumulativeUtil += doc.GetWorkingUtilization();
			}
			return cumulativeUtil / AllDoctors.Count;
		}
		
		public double GetUtilAllNurses()
		{
			var cumulativeUtil = 0.0;
			foreach (var nurse in AllNurses)
			{
				cumulativeUtil += nurse.GetWorkingUtilization();
			}
			return cumulativeUtil / AllNurses.Count;
		}
		
		public double GetUtilAllRoomsA()
		{
			var cumulativeUtil = 0.0;
			foreach (var room in AllRoomsTypeA)
			{
				cumulativeUtil += room.GetUtilization();
			}
			return cumulativeUtil / AllRoomsTypeA.Count;
		}
		
		public double GetUtilAllRoomsB()
		{
			var cumulativeUtil = 0.0;
			foreach (var room in AllRoomsTypeB)
			{
				cumulativeUtil += room.GetUtilization();
			}
			return cumulativeUtil / AllRoomsTypeB.Count;
		}
		
		public bool FreeResForMedicalA()
		{
			return Doctors.Count > 0 && Nurses.Count > 0 && FreeRoomsTypeA.Count > 0;
		}

		public bool FreeResForMedicalAB()
		{
			return Doctors.Count > 0 && Nurses.Count > 0 && (FreeRoomsTypeA.Count > 0 || FreeRoomsTypeB.Count > 0);
		}

		public bool FreeResForMedicalB()
		{
			return Doctors.Count > 0 && Nurses.Count > 0 && FreeRoomsTypeB.Count > 0;
		}
		
		public bool FreeResForEntryExam()
		{
			return Nurses.Count > 0 && FreeRoomsTypeB.Count > 0;
		}
		
		public void AllocateRoom(MyMessage myMsg, List<Room> rooms, bool doctorNeeded = false, bool bestVariant = false)
		{
			if (MyCastSim().ResourceAllocatingStrategy == ResourceAllocatingStrategy.Exp5RoomWithResources || bestVariant)//MyCastSim().AllocateRoomWithResources || 
			{
				var bestRoomIndex = 0;
				var score = 0;
				var bestScore = 0;
				if (doctorNeeded)
				{
					for (int i = 0; i < rooms.Count; i++)
					{
						var room = rooms[i];
						if (room.Nurse != null && room.Doctor != null)
						{
							bestRoomIndex = i;
							break;
						}
						if (room.Nurse != null) score = 1;
						if (room.Doctor != null) score = 2;
						
						if (score > bestScore)
						{
							bestRoomIndex = i;
							bestScore = score;
						}
					}
					myMsg.Room = rooms[bestRoomIndex];
					myMsg.Room.StartOccupancy();
					rooms.RemoveAt(bestRoomIndex);
					if (myMsg.Room.Nurse != null)
					{
						myMsg.Nurse = myMsg.Room.Nurse;
						Nurses.Remove(myMsg.Nurse);
					}
					else
					{
						myMsg.Nurse = Nurses[0];
						Nurses.RemoveAt(0);
					}

					if (myMsg.Room.Doctor != null)
					{
						myMsg.Doctor = myMsg.Room.Doctor;
						Doctors.Remove(myMsg.Doctor);
					}
					else
					{
						myMsg.Doctor = Doctors[0];
						Doctors.RemoveAt(0);
					}
				}
				else
				{
					for (int i = 0; i < rooms.Count; i++)
					{
						var room = rooms[i];
						if (room.Nurse != null)
						{
							bestRoomIndex = i;
							break;
						}
					}
					myMsg.Room = rooms[bestRoomIndex];
					myMsg.Room.StartOccupancy();
					rooms.RemoveAt(bestRoomIndex);
					if (myMsg.Room.Nurse != null)
					{
						myMsg.Nurse = myMsg.Room.Nurse;
						Nurses.Remove(myMsg.Nurse);
					}
					else
					{
						myMsg.Nurse = Nurses[0];
						Nurses.RemoveAt(0);
					}
				}
			}
			else
			{
				myMsg.Room = rooms[0];
				myMsg.Room.StartOccupancy();
				rooms.RemoveAt(0);
			}
		}
		
		public void Reset()
		{
			foreach (var doctor in AllDoctors)
			{
				doctor.Reset();
			}

			foreach (var nurse in AllNurses)
			{
				nurse.Reset();
			}

			foreach (var room in AllRoomsTypeA)
			{
				room.Reset();
			}
			foreach (var room in AllRoomsTypeB)
			{
				room.Reset();
			}
		}

		private MySimulation MyCastSim() => (MySimulation)MySim; 

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerResources(SimId.ManagerResources, MySim, this);
			new Exp4WaitAndThen(SimId.Exp4WaitAndThen, MySim, this);
			new AllocateMedicalTreatRes(SimId.AllocateMedicalTreatRes, MySim, this);
			new AllocateEntryExamRes(SimId.AllocateEntryExamRes, MySim, this);
			AddOwnMessage(Mc.FreeUpResources);
			AddOwnMessage(Mc.GetMedicalTreatResources);
			AddOwnMessage(Mc.GetEntryExamResources);
		}
		//meta! tag="end"
	}
}