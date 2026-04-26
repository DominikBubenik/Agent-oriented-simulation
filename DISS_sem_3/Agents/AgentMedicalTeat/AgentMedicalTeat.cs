using OSPABA;
using Simulation;
using Agents.AgentMedicalTeat.ContinualAssistants;
using MainLogic;

namespace Agents.AgentMedicalTeat
{
	//meta! id="41"
	public class AgentMedicalTeat : OSPABA.Agent
	{
		private ContinuousGenerator _walkInPatientDuration;
		private ContinuousGenerator _ambulancedPatientDuration;

		public AgentMedicalTeat(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();

			var specs = new List<GenSpec>()
			{
				new(0.1,10,12),
				new(0.6,12,14),
				new(0.3,14,18)
			};
			_walkInPatientDuration = new ContinuousGenerator(new Random(MyCastSim().Seeder.Next()), specs);
			_ambulancedPatientDuration = new ContinuousGenerator(new Random(MyCastSim().Seeder.Next()),
				[new(1, 15, 30)]);
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		public double GetWalkInExamDuration() => _walkInPatientDuration.Sample();
		public double GetAmbulanceExamDuration() => _ambulancedPatientDuration.Sample();

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerMedicalTeat(SimId.ManagerMedicalTeat, MySim, this);
			new ProcessMedicalTreat(SimId.ProcessMedicalTreat, MySim, this);
			AddOwnMessage(Mc.MedicalTreatPatient);
		}
		//meta! tag="end"
		
		private MySimulation MyCastSim() => (MySimulation)MySim;
	}
}