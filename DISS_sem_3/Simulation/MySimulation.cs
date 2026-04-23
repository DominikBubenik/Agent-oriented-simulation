using Agents.AgentResources;
using Agents.AgentBoss;
using OSPABA;
using Agents.AgentEntryExam;
using Agents.AgentEnviroment;
using Agents.AgentMedicalTeat;
using Agents.AgentTransition;
using Agents.AgentEDepartment;
using MainLogic;

namespace Simulation
{
	public class MySimulation : OSPABA.Simulation
	{
		public SimpleStat TotalTimeInSystemAmbulancePatient { get; private set; }
		public SimpleStat TotalTimeInSystemWalkInPatient { get; private set; }
		public SimpleStat TotalEntranceWaitingTimeAmbulanceP { get; private set; }
		public SimpleStat TotalEntranceWaitingTimeWalkInP { get; private set; }
		public SimpleStat TotalEntryExamWaitingTimeAmbulanceP { get; private set; }
		public SimpleStat TotalEntryExamWaitingTimeWalkInP { get; private set; }
		public SimpleStat TotalMedicalTreatWaitingTimeAmbulanceP { get; private set; }
		public SimpleStat TotalMedicalTreatWaitingTimeWalkInP { get; private set; }
		public MySimulation()
		{
			Init();
		}

		override public void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
			TotalTimeInSystemAmbulancePatient = new SimpleStat();
			TotalTimeInSystemWalkInPatient = new SimpleStat();

			TotalEntranceWaitingTimeAmbulanceP = new SimpleStat();
			TotalEntranceWaitingTimeWalkInP = new SimpleStat();

			TotalEntryExamWaitingTimeAmbulanceP = new SimpleStat();
			TotalEntryExamWaitingTimeWalkInP = new SimpleStat();

			TotalMedicalTreatWaitingTimeAmbulanceP = new SimpleStat();
			TotalMedicalTreatWaitingTimeWalkInP = new SimpleStat();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...
		}

		override public void ReplicationFinished()
		{
			// Collect local statistics into global, update UI, etc...
			base.ReplicationFinished();
		}

		override public void SimulationFinished()
		{
			// Display simulation results
			base.SimulationFinished();

			TotalTimeInSystemAmbulancePatient.AddSample(AgentEnviroment.TimeInSystemAmbulancePatient.GetAverage());
			TotalTimeInSystemWalkInPatient.AddSample(AgentEnviroment.TimeInSystemWalkInPatient.GetAverage());

			TotalEntranceWaitingTimeAmbulanceP.AddSample(AgentEnviroment.EntranceWaitingTimeAmbulanceP.GetAverage());
			TotalEntranceWaitingTimeWalkInP.AddSample(AgentEnviroment.EntranceWaitingTimeWalkInP.GetAverage());

			TotalEntryExamWaitingTimeAmbulanceP.AddSample(AgentEnviroment.EntryExamWaitingTimeAmbulanceP.GetAverage());
			TotalEntryExamWaitingTimeWalkInP.AddSample(AgentEnviroment.EntryExamWaitingTimeWalkInP.GetAverage());

			TotalMedicalTreatWaitingTimeAmbulanceP.AddSample(AgentEnviroment.MedicalTreatWaitingTimeAmbulanceP.GetAverage());
			TotalMedicalTreatWaitingTimeWalkInP.AddSample(AgentEnviroment.MedicalTreatWaitingTimeWalkInP.GetAverage());

			Console.WriteLine("Simulation finished");
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentBoss = new AgentBoss(SimId.AgentBoss, this, null);
			AgentEnviroment = new AgentEnviroment(SimId.AgentEnviroment, this, AgentBoss);
			AgentEDepartment = new AgentEDepartment(SimId.AgentEDepartment, this, AgentBoss);
			AgentResources = new AgentResources(SimId.AgentResources, this, AgentEDepartment);
			AgentEntryExam = new AgentEntryExam(SimId.AgentEntryExam, this, AgentEDepartment);
			AgentTransition = new AgentTransition(SimId.AgentTransition, this, AgentEDepartment);
			AgentMedicalTeat = new AgentMedicalTeat(SimId.AgentMedicalTeat, this, AgentEDepartment);
		}
		public AgentBoss AgentBoss
		{ get; set; }
		public AgentEnviroment AgentEnviroment
		{ get; set; }
		public AgentEDepartment AgentEDepartment
		{ get; set; }
		public AgentResources AgentResources
		{ get; set; }
		public AgentEntryExam AgentEntryExam
		{ get; set; }
		public AgentTransition AgentTransition
		{ get; set; }
		public AgentMedicalTeat AgentMedicalTeat
		{ get; set; }
		//meta! tag="end"
	}
}