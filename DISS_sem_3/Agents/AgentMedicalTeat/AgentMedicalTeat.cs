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
				new(0.1,10 * 60,12 * 60),
				new(0.6,12 * 60,14 * 60),
				new(0.3,14 * 60,18 * 60)
			};
			_walkInPatientDuration = new ContinuousGenerator(new Random(MyCastSim().Seeder.Next()), specs);
			_ambulancedPatientDuration = new ContinuousGenerator(new Random(MyCastSim().Seeder.Next()),
				[new(1, 15 * 60, 30 * 60)]);
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