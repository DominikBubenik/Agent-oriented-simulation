using Agents.AgentTransition.ContinualAssistants;
using OSPABA;
using Simulation;
using DISS_sem_3.Entities;
using MainLogic;

namespace Agents.AgentTransition
{
	//meta! id="104"
	public class AgentTransition : OSPABA.Agent
	{
		private TriangularGenerator _allRoomTransitGenerator;
		private TriangularGenerator _walkInDuration;
		private Random _ambulanceDuration;
		private Random _exitSystemDuration;
		public AgentTransition(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
			_walkInDuration = new TriangularGenerator(MyCastSim().NextSeed(), 120, 300, 150);
			_ambulanceDuration = new Random(MyCastSim().NextSeed());
			_allRoomTransitGenerator = new TriangularGenerator(MyCastSim().NextSeed(), 15, 45, 20);
			_exitSystemDuration = new Random(MyCastSim().NextSeed());
		}

		public List<Nurse> MovingNurses { get; set; }

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		public double EntranceWalkInPatientDuration() => _walkInDuration.Generate();
		public double EntranceAmbulancePatientDuration() => _ambulanceDuration.NextDouble() * (200 - 90) + 90;
		public double GetAllRoomTransferDuration() => _allRoomTransitGenerator.Generate();
		public double ExitSystemDuration() => _exitSystemDuration.NextDouble() * (240 - 150) + 150;
		
		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerTransition(SimId.ManagerTransition, MySim, this);
			new ExitDepartment(SimId.ExitDepartment, MySim, this);
			new EntryPatient(SimId.EntryPatient, MySim, this);
			new AllRoomTransfer(SimId.AllRoomTransfer, MySim, this);
			AddOwnMessage(Mc.EntranceTransition);
			AddOwnMessage(Mc.BetweenAmbulanceTransition);
			AddOwnMessage(Mc.ExitTransition);
		}
		//meta! tag="end"
	
		private MySimulation MyCastSim() => (MySimulation)MySim;

		public PointF[][] GetConfigForPatientTransfer(Room? room, Patient patient)
		{
			if (room == null) throw new Exception("Room is null");
			if (patient.Priority < 3)
			{
				return Config.PATH_MEDICAL_A_QUEUE_TO_ROOM_A;
			} 
			if (patient.Priority < 5 && room.IsTypeA())
			{
				return Config.PATH_MEDICAL_B_QUEUE_TO_ROOM_A;
			}
			return Config.PATH_MEDICAL_B_QUEUE_TO_ROOM_B;
		}
	}
}