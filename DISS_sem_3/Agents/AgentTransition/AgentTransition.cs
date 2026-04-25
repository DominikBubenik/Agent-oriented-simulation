using Agents.AgentTransition.ContinualAssistants;
using DISS_sem_3.Entities;
using MainLogic;
using OSPABA;
using Simulation;

namespace Agents.AgentTransition
{
	//meta! id="104"
	public class AgentTransition : OSPABA.Agent
	{
		private TriangularGenerator _allRoomTransitGenerator;
		
		public AgentTransition(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
			_allRoomTransitGenerator = new TriangularGenerator(MyCastSim().NextSeed(), 15, 45, 20);
		}

		public List<Nurse> MovingNurses { get; set; }

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		public double GetAllRoomTransferDuration() => _allRoomTransitGenerator.Generate();
		
		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerTransition(SimId.ManagerTransition, MySim, this);
			new ExitDepartment(SimId.ExitDepartment, MySim, this);
			new AllRoomTransfer(SimId.AllRoomTransfer, MySim, this);
			new EntryPatient(SimId.EntryPatient, MySim, this);
			AddOwnMessage(Mc.EntranceTransmition);
			AddOwnMessage(Mc.ExitTransmition);
			AddOwnMessage(Mc.BetweenAmbulanceTransition);
		}
		//meta! tag="end"
	
		private MySimulation MyCastSim() => (MySimulation)MySim;
	}
}