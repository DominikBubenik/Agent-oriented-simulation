using Agents.AgentBoss;
using Agents.AgentResources;
using OSPABA;
using Agents.AgentEntryExam;
using Agents.AgentEnviroment;
using Agents.AgentMedicalTeat;
using Agents.AgentEDepartment;

namespace Simulation
{
	public class MySimulation : OSPABA.Simulation
	{
		public MySimulation()
		{
			Init();
		}

		override public void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
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
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentBoss = new AgentBoss(SimId.AgentBoss, this, null);
			AgentEnviroment = new AgentEnviroment(SimId.AgentEnviroment, this, AgentBoss);
			AgentEDepartment = new AgentEDepartment(SimId.AgentEDepartment, this, AgentBoss);
			AgentResources = new AgentResources(SimId.AgentResources, this, AgentEDepartment);
			AgentEntryExam = new AgentEntryExam(SimId.AgentEntryExam, this, AgentEDepartment);
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
		public AgentMedicalTeat AgentMedicalTeat
		{ get; set; }
		//meta! tag="end"
	}
}