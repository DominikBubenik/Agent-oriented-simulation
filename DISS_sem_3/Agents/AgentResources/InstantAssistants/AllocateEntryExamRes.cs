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
			}
		}

		private void Exp0FirstAvailable(MyMessage myMsg)
		{
			var nurses = MyAgent.Nurses;
			var rooms = MyAgent.FreeRoomsTypeB;
			if (nurses.Count > 0 && rooms.Count > 0)
			{
				myMsg.Nurse = nurses[0];
				// myMsg.Nurse.StartTransfer();
				// myMsg.Nurse.Activity = StaffActivity.Working;
				nurses.RemoveAt(0);
				myMsg.Room = rooms[0];
				myMsg.Room.StartOccupancy();
				rooms.RemoveAt(0);
				GlobalLogger.PrintLog($"room {myMsg.Room.ToString()}  nurse {myMsg.Nurse.ToString()}" , MySim.CurrentTime);
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