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
				myMsg.Nurse = nurses[0];
				nurses.RemoveAt(0);

				myMsg.Doctor = doctors[0];
				doctors.RemoveAt(0);

				myMsg.Room = rooms[0];
				myMsg.Room.StartOccupancy();
				rooms.RemoveAt(0);
				// GlobalLogger.PrintLog($"room {myMsg.Room.ToString()}  nurse {myMsg.Nurse.ToString()} doctor {myMsg.Doctor.ToString()}" , MySim.CurrentTime);
			}
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