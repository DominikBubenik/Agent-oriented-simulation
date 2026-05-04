using Agents.AgentResources;
using DISS_sem_3;
using DISS_sem_3.Entities;
using MainLogic;
using OSPABA;
using Simulation;

namespace Agents.AgentResources.InstantAssistants
{
	/*!
	 * otazka tu pomocou nich alokojem zdroje napriklad tie ktore su najmenej vytazene??
	 */
	//meta! id="82"
	public class AllocateEntryExamRes : OSPABA.Adviser
	{
		public AllocateEntryExamRes(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
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
			}
		}

		private void Exp0FirstAvailable(MyMessage myMsg)
		{
			var nurses = MyAgent.Nurses;
			var rooms = MyAgent.FreeRoomsTypeB;
			if (nurses.Count > 0 && rooms.Count > 0)
			{
				myMsg.Nurse = nurses[0];
				nurses.RemoveAt(0);
				myMsg.Room = rooms[0];
				myMsg.Room.StartOccupancy();
				rooms.RemoveAt(0);
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"EntryExam Resources allocated Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
			}
			else
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
			}
		}
		
		private void Exp1LeastUtilizedStaff(MyMessage myMsg)
		{
			var nurses = MyAgent.Nurses;
			var rooms = MyAgent.FreeRoomsTypeB;
			
			if (nurses.Count > 0 && rooms.Count > 0 )
			{
				myMsg.Nurse = nurses.MinBy(n => n.GetWorkingUtilization());
				nurses.Remove(myMsg.Nurse);

				myMsg.Room = rooms[0];
				myMsg.Room.StartOccupancy();
				rooms.RemoveAt(0);
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"EntryExam Resources allocated Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
			}
			else
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
			}
		}
		
		private void Exp2KeepOneNOneD(MyMessage myMsg)
		{
			var nurses = MyAgent.Nurses;
			var rooms = MyAgent.FreeRoomsTypeB;
			
			if ((nurses.Count > 1 || myMsg.EntryQueueLength > ((MySimulation)MySim).MaxEntryQueueLengthCount  || (nurses.Count > 0 && myMsg.Patient.ArrivedByAmbulance)) && rooms.Count > 0)
			{
				myMsg.Nurse = nurses[0];
				nurses.RemoveAt(0);
				myMsg.Room = rooms[0];
				myMsg.Room.StartOccupancy();
				rooms.RemoveAt(0);
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"EntryExam Resources allocated Nurse: {myMsg.Nurse.Id}, Room {myMsg.Room.Id}");
			}
			else
			{
				if (MySim is MySimulation sim && sim.ObservationMode) sim.NotifyLogger($"Resources not allocated");
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