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
			var rooms = GetAvailableRooms(myMsg);

			switch (mySim.ResourceAllocatingStrategy)
			{
				case ResourceAllocatingStrategy.Exp0FirstAvailable:
					Exp0FirstAvailable(myMsg, rooms);
					break;
				case ResourceAllocatingStrategy.Exp1LeastUtilized:
					Exp1LeastUtilizedStaff(myMsg, rooms);
					break;
				case ResourceAllocatingStrategy.Exp2KeepOneNOneD:
					Exp2KeepOneNOneD(myMsg, rooms);
					break;
				case ResourceAllocatingStrategy.Exp3KeepJustOneNurse:
					Exp3KeepOneNurse(myMsg, rooms);
					break;
				case ResourceAllocatingStrategy.Exp4WaitAndThen:
					Exp4WaitAndThen(myMsg, rooms);
					break;
				case ResourceAllocatingStrategy.Exp5RoomWithResources:
					Exp5RoomWithResources(myMsg, rooms);
					break;	
				case ResourceAllocatingStrategy.BestVariant:
					BestVariant(myMsg, rooms);
					break;
			}
		}

		private void Exp0FirstAvailable(MyMessage myMsg, List<Room> rooms)
		{
			if (MyAgent.Nurses.Count > 0 && rooms.Count > 0 && MyAgent.Doctors.Count > 0)
			{
				AssignStaff(myMsg);
				MyAgent.AllocateRoom(myMsg, rooms, true);
				NotifyAllocation(myMsg, true);
			}
			else
			{
				NotifyAllocation(myMsg, false);
			}
		}
		
		private void Exp1LeastUtilizedStaff(MyMessage myMsg, List<Room> rooms)
		{
			var nurses = MyAgent.Nurses;
			var doctors = MyAgent.Doctors;
			
			if (nurses.Count > 0 && rooms.Count > 0 && doctors.Count > 0)
			{
				myMsg.Nurse = nurses.MinBy(n => n.GetWorkingUtilization());
				nurses.Remove(myMsg.Nurse);

				myMsg.Doctor = doctors.MinBy(d => d.GetWorkingUtilization());
				doctors.Remove(myMsg.Doctor);

				MyAgent.AllocateRoom(myMsg, rooms, true);
				NotifyAllocation(myMsg, true);
			} else
			{
				NotifyAllocation(myMsg, false);
			}
		}
		
		private void Exp2KeepOneNOneD(MyMessage myMsg, List<Room> rooms)
		{
			if (MyAgent.Nurses.Count == 0 || rooms.Count == 0 || MyAgent.Doctors.Count == 0)
			{
				NotifyAllocation(myMsg, false);
				return;
			}

			var enoughResources = MyAgent.Nurses.Count > 1 && MyAgent.Doctors.Count > 1 && 
			                      (rooms.Count > 1 || (rooms.Count > 0 && !rooms[0].IsTypeA()));
			
			var tooLong =  myMsg.MedicalQueueLengthB >  ((MySimulation)MySim).MaxMedicalQueueLengthCount;
			var isPriority =  myMsg.Patient.Priority < 3;
			
			if (enoughResources || isPriority || tooLong)
			{
				AssignStaff(myMsg);
				MyAgent.AllocateRoom(myMsg, rooms, true);
				NotifyAllocation(myMsg, true);
			}
			else
			{
				NotifyAllocation(myMsg, false);
			}
		}
		
		private void Exp3KeepOneNurse(MyMessage myMsg, List<Room> rooms)
		{
			if (MyAgent.Nurses.Count == 0 || rooms.Count == 0 || MyAgent.Doctors.Count == 0)
			{
				NotifyAllocation(myMsg, false);
				return;
			}
			
			var enoughResources = MyAgent.Nurses.Count > 1 && MyAgent.Doctors.Count > 0 && 
			                      (rooms.Count > 1 || (rooms.Count > 0 && !rooms[0].IsTypeA()));
			
			var tooLong = myMsg.MedicalQueueLengthB >  ((MySimulation)MySim).MaxMedicalQueueLengthCount;
			var isPriority = myMsg.Patient.Priority < 3;
			
			if (enoughResources || isPriority || tooLong)
			{
				AssignStaff(myMsg);
				MyAgent.AllocateRoom(myMsg, rooms, true);
				NotifyAllocation(myMsg, true);
			}
			else
			{
				NotifyAllocation(myMsg, false);
			}
		}
		
		private void Exp4WaitAndThen(MyMessage myMsg, List<Room> rooms)
		{
			if (_lastCheck == 0) _lastCheck = MySim.CurrentTime;

			var enoughResources = MyAgent.Nurses.Count > 1 && MyAgent.Doctors.Count > 1 && 
			                      (rooms.Count > 1 || (rooms.Count > 0 && !rooms[0].IsTypeA()));
			var myCastSim = MySim as MySimulation;
			var timePassed = MyAgent.Nurses.Count > 0 && rooms.Count > 0 && MyAgent.Doctors.Count > 0 && ((MySim.CurrentTime - _lastCheck) >= myCastSim.MaxTimeWaitStrategy);
			var isPriority = MyAgent.Nurses.Count > 0 && rooms.Count > 0 && MyAgent.Doctors.Count > 0 && myMsg.Patient.Priority < 3;
			
			if (enoughResources || isPriority || timePassed)
			{
				_lastCheck = 0;
				AssignStaff(myMsg);
				MyAgent.AllocateRoom(myMsg, rooms, true);
				NotifyAllocation(myMsg, true);
				if (_noticeMsg != null)
				{
					MyAgent.MyManager.BreakContinualAssistant(new MyMessage(MySim));
					_noticeMsg = null;
				}
			}
			else
			{
				NotifyAllocation(myMsg, false);
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
		
		private void Exp5RoomWithResources(MyMessage myMsg, List<Room> rooms)
		{
			if (MyAgent.Nurses.Count > 0 && rooms.Count > 0 && MyAgent.Doctors.Count > 0)
			{
				MyAgent.AllocateRoom(myMsg, rooms, true);
				NotifyAllocation(myMsg, true);
			}
			else
			{
				NotifyAllocation(myMsg, false);
			}
		}
		
		private void BestVariant(MyMessage myMsg, List<Room> rooms)
		{
			var nurses = MyAgent.Nurses;
			var doctors = MyAgent.Doctors;

			var allAvailable = nurses.Count > 0 && doctors.Count > 0 && rooms.Count > 0;
			var enoughResources = nurses.Count > 1 && rooms.Count > 1 && doctors.Count > 1;
			var queueIsTooLong = myMsg.MedicalQueueLengthB > ((MySimulation)MySim).MaxMedicalQueueLengthCount;
			var isPriority = myMsg.Patient.Priority < 3;
			if (allAvailable && (queueIsTooLong || enoughResources || isPriority))
			{
				MyAgent.AllocateRoom(myMsg, rooms, doctorNeeded: true, bestVariant: true);
				NotifyAllocation(myMsg, true);
			}
			else
			{
				NotifyAllocation(myMsg, false);
			}
		}

		private List<Room> GetAvailableRooms(MyMessage myMsg)
		{
			if (myMsg.Patient.Priority < 3)
			{
				return MyAgent.FreeRoomsTypeA;
			}
			if (myMsg.Patient.Priority < 5)
			{
				return MyAgent.FreeRoomsTypeB.Count == 0 ? MyAgent.FreeRoomsTypeA : MyAgent.FreeRoomsTypeB;
			}
			return MyAgent.FreeRoomsTypeB;
		}

		private void NotifyAllocation(MyMessage myMsg, bool success)
		{
			if (MySim is MySimulation sim && sim.ObservationMode)
			{
				if (success)
					sim.NotifyLogger($"MedicalTreat Resources allocated Doctor: {myMsg.Doctor.Id} ,Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
				else
					sim.NotifyLogger("Resources not allocated");
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