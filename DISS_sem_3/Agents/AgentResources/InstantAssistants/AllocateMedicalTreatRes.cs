using System.Collections.Specialized;
using Agents.AgentResources;
using DISS_sem_3;
using DISS_sem_3.Entities;
using MainLogic;
using OSPABA;
using Simulation;

namespace Agents.AgentResources.InstantAssistants
{
	//meta! id="84"
	public class AllocateMedicalTreatRes : OSPABA.Adviser
	{
		public bool StartToWait { get; set; }
		private double _lastCheck = 0;
		private MyMessage? _noticeMsg;
		public AllocateMedicalTreatRes(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void Execute(MessageForm message)
		{
			var myMsg = (MyMessage)message;
			var mySim = (MySimulation)MySim;
			switch (mySim.ResourceAllocatingStrategy)
			{
				case ResourceAllocatingStrategy.Exp0FirstAvailable:
					Exp0FirstAvailable(myMsg);
					break;
				case ResourceAllocatingStrategy.Exp1LeastUtilized:
					Exp1LeastUtilizedStaff(myMsg);
					break;
				case ResourceAllocatingStrategy.Exp2KeepOneNOneD:
					Exp2KeepOneNOneD(myMsg);
					break;
				case ResourceAllocatingStrategy.Exp3KeepJustOneNurse:
					Exp3KeepOneNurse(myMsg);
					break;
				case ResourceAllocatingStrategy.Exp4WaitAndThen:
					Exp4WaitAndThen(myMsg);
					break;
				case ResourceAllocatingStrategy.Exp5RoomWithResources:
					Exp5RoomWithResources(myMsg);
					break;	
				case ResourceAllocatingStrategy.BestVariant:
					BestVariant(myMsg);
					break;
			}
		}

		private void Exp0FirstAvailable(MyMessage myMsg)
		{
			var nurses = MyAgent.Nurses;
			var doctors = MyAgent.Doctors;
			List<Room> rooms;
			if (myMsg.Patient.Priority < 3)
			{
				rooms = MyAgent.FreeRoomsTypeA;
			} else if (myMsg.Patient.Priority < 5)
			{
				rooms = MyAgent.FreeRoomsTypeB.Count == 0 ? MyAgent.FreeRoomsTypeA : MyAgent.FreeRoomsTypeB;
			}
			else
			{
				rooms = MyAgent.FreeRoomsTypeB;
			}
			
			if (nurses.Count > 0 && rooms.Count > 0 && doctors.Count > 0)
			{
				AssignStaff(myMsg);

				MyAgent.AllocateRoom(myMsg, rooms, true);
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"MedicalTreat Resources allocated Doctor: {myMsg.Doctor.Id} ,Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
			}
			else
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
			}
		}
		
		private void Exp1LeastUtilizedStaff(MyMessage myMsg)
		{
			var nurses = MyAgent.Nurses;
			var doctors = MyAgent.Doctors;
			List<Room> rooms;
			if (myMsg.Patient.Priority < 3)
			{
				rooms = MyAgent.FreeRoomsTypeA;
			} else if (myMsg.Patient.Priority < 5)
			{
				rooms = MyAgent.FreeRoomsTypeB.Count == 0 ? MyAgent.FreeRoomsTypeA : MyAgent.FreeRoomsTypeB;
			}
			else
			{
				rooms = MyAgent.FreeRoomsTypeB;
			}
			
			if (nurses.Count > 0 && rooms.Count > 0 && doctors.Count > 0)
			{
				myMsg.Nurse = nurses.MinBy(n => n.GetWorkingUtilization());
				nurses.Remove(myMsg.Nurse);

				myMsg.Doctor = doctors.MinBy(d => d.GetWorkingUtilization());
				doctors.Remove(myMsg.Doctor);

				MyAgent.AllocateRoom(myMsg, rooms, true);
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"MedicalTreat Resources allocated Doctor: {myMsg.Doctor.Id} ,Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
			} else
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
			}
		}
		
		private void Exp2KeepOneNOneD(MyMessage myMsg)
		{
			var nurses = MyAgent.Nurses;
			var doctors = MyAgent.Doctors;
			List<Room> rooms;
			if (myMsg.Patient.Priority < 3)
			{
				rooms = MyAgent.FreeRoomsTypeA;
			} else if (myMsg.Patient.Priority < 5)
			{
				rooms = MyAgent.FreeRoomsTypeB.Count == 0 ? MyAgent.FreeRoomsTypeA : MyAgent.FreeRoomsTypeB;
			}
			else
			{
				rooms = MyAgent.FreeRoomsTypeB;
			}

			if (nurses.Count == 0 || rooms.Count == 0 || doctors.Count == 0)
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
				return;
			}

			var enoughResources = nurses.Count > 1 && doctors.Count > 1 && 
			                      (rooms.Count > 1 || (rooms.Count > 0 && !rooms[0].IsTypeA()));
			
			var tooLong =  myMsg.MedicalQueueLengthB >  ((MySimulation)MySim).MaxMedicalQueueLengthCount;
			
			var isPriority =  myMsg.Patient.Priority < 3;
			
			if (enoughResources || isPriority || tooLong)
			{
				AssignStaff(myMsg);

				MyAgent.AllocateRoom(myMsg, rooms, true);
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"MedicalTreat Resources allocated Doctor: {myMsg.Doctor.Id} ,Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
			}
			else
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
			}
		}
		
		private void Exp3KeepOneNurse(MyMessage myMsg)
		{
			var nurses = MyAgent.Nurses;
			var doctors = MyAgent.Doctors;
			List<Room> rooms;
			if (myMsg.Patient.Priority < 3)
			{
				rooms = MyAgent.FreeRoomsTypeA;
			} else if (myMsg.Patient.Priority < 5)
			{
				rooms = MyAgent.FreeRoomsTypeB.Count == 0 ? MyAgent.FreeRoomsTypeA : MyAgent.FreeRoomsTypeB;
			}
			else
			{
				rooms = MyAgent.FreeRoomsTypeB;
			}

			if (nurses.Count == 0 || rooms.Count == 0 || doctors.Count == 0)
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
				return;
			}
			
			var enoughResources = nurses.Count > 1 && doctors.Count > 0 && 
			                      (rooms.Count > 1 || (rooms.Count > 0 && !rooms[0].IsTypeA()));
			
			var tooLong = myMsg.MedicalQueueLengthB >  ((MySimulation)MySim).MaxMedicalQueueLengthCount;
			
			var isPriority = myMsg.Patient.Priority < 3;
			
			if (enoughResources || isPriority || tooLong)
			{
				AssignStaff(myMsg);

				MyAgent.AllocateRoom(myMsg, rooms, true);
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"MedicalTreat Resources allocated Doctor: {myMsg.Doctor.Id} ,Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
			}
			else
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
			}
		}
		
		private void Exp4WaitAndThen(MyMessage myMsg)
		{
			var nurses = MyAgent.Nurses;
			var doctors = MyAgent.Doctors;
			List<Room> rooms;

			if (myMsg.Patient.Priority < 3)
			{
				rooms = MyAgent.FreeRoomsTypeA;
			} else if (myMsg.Patient.Priority < 5)
			{
				rooms = MyAgent.FreeRoomsTypeB.Count == 0 ? MyAgent.FreeRoomsTypeA : MyAgent.FreeRoomsTypeB;
			}
			else
			{
				rooms = MyAgent.FreeRoomsTypeB;
			}

			if (_lastCheck == 0) _lastCheck = MySim.CurrentTime;

			var enoughResources = nurses.Count > 1 && doctors.Count > 1 && 
			                      (rooms.Count > 1 || (rooms.Count > 0 && !rooms[0].IsTypeA()));
			var myCastSim = MySim as MySimulation;
			// var tooLong = nurses.Count > 0 && rooms.Count > 0 && doctors.Count > 0 && myMsg.MedicalQueueLengthB >  myCastSim.MaxMedicalQueueLengthCount;
			var timePassed = nurses.Count > 0 && rooms.Count > 0 && doctors.Count > 0 && ((MySim.CurrentTime - _lastCheck) >= myCastSim.MaxTimeWaitStrategy);
			var isPriority = nurses.Count > 0 && rooms.Count > 0 && doctors.Count > 0 && myMsg.Patient.Priority < 3;
			
			if (enoughResources || isPriority || timePassed)
			{
				_lastCheck = 0;
				AssignStaff(myMsg);
			
				MyAgent.AllocateRoom(myMsg, rooms, true);
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"MedicalTreat Resources allocated Doctor: {myMsg.Doctor.Id} ,Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
				if (_noticeMsg != null)
				{
					MyAgent.MyManager.BreakContinualAssistant(new MyMessage(MySim));
					_noticeMsg = null;
				}
			}
			else
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
				if (_noticeMsg == null)
				{
					_noticeMsg = (MyMessage)myMsg.CreateCopy();
					_noticeMsg.Patient = myMsg.Patient;
					_noticeMsg.Addressee = MyAgent.FindAssistant(SimId.Exp4WaitAndThen);
					_noticeMsg.Code = Mc.Start;
					MyAgent.MyManager.StartContinualAssistant(_noticeMsg);
				}
			}
		}
		
		private void Exp5RoomWithResources(MyMessage myMsg)
		{
			List<Room> rooms;
			if (myMsg.Patient.Priority < 3)
			{
				rooms = MyAgent.FreeRoomsTypeA;
			} else if (myMsg.Patient.Priority < 5)
			{
				rooms = MyAgent.FreeRoomsTypeB.Count == 0 ? MyAgent.FreeRoomsTypeA : MyAgent.FreeRoomsTypeB;
			}
			else
			{
				rooms = MyAgent.FreeRoomsTypeB;
			}
			
			if (MyAgent.Nurses.Count > 0 && rooms.Count > 0 && MyAgent.Doctors.Count > 0)
			{
				// AssignStaff(myMsg);

				MyAgent.AllocateRoom(myMsg, rooms, true);
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"MedicalTreat Resources allocated Doctor: {myMsg.Doctor.Id} ,Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
			}
			else
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
			}
		}
		
		private void BestVariant(MyMessage myMsg)
		{
			var nurses = MyAgent.Nurses;
			var doctors = MyAgent.Doctors;
			List<Room> rooms;
			if (myMsg.Patient.Priority < 3)
			{
				rooms = MyAgent.FreeRoomsTypeA;
			} else if (myMsg.Patient.Priority < 5)
			{
				rooms = MyAgent.FreeRoomsTypeB.Count == 0 ? MyAgent.FreeRoomsTypeA : MyAgent.FreeRoomsTypeB;
			}
			else
			{
				rooms = MyAgent.FreeRoomsTypeB;
			}

			var allAvailable = nurses.Count > 0 && doctors.Count > 0 && rooms.Count > 0;//(rooms.Count > 1 || (rooms.Count > 0 && !rooms[0].IsTypeA()));
			var enoughResources = nurses.Count > 1 && rooms.Count > 1 && doctors.Count > 1;
			var queueIsTooLong = myMsg.MedicalQueueLengthB > ((MySimulation)MySim).MaxMedicalQueueLengthCount;
			var isPriority = myMsg.Patient.Priority < 3;
			if (allAvailable && (queueIsTooLong || enoughResources || isPriority))
			{
				MyAgent.AllocateRoom(myMsg, rooms, doctorNeeded: true, bestVariant: true);
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"MedicalTreat Resources allocated  Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
			}
			else
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
			}
		}

		private void AssignStaff(MyMessage myMsg)
		{
			myMsg.Nurse = MyAgent.Nurses[0];
			MyAgent.Nurses.RemoveAt(0);

			myMsg.Doctor = MyAgent.Doctors[0];
			MyAgent.Doctors.RemoveAt(0);
		}

		public new AgentResources MyAgent
		{
			get
			{
				return (AgentResources)base.MyAgent;
			}
		}
	}
}