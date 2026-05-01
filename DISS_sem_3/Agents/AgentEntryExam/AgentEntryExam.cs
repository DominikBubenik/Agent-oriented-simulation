using OSPABA;
using Simulation;
using Agents.AgentEntryExam.ContinualAssistants;
using DISS_sem_3.Entities;
using MainLogic;

namespace Agents.AgentEntryExam
{
	//meta! id="38"
	public class AgentEntryExam : OSPABA.Agent
	{
		private ContinuousGenerator _entryExamWalkInDuration;
		private DiscreteGenerator _entryAmbulanceInDuration;
		private Random _walkInPriority;
		private Random _ambulancePriority;

		private double[] _walkInPriorityProbability = [0.1, 0.2, 0.15, 0.25, 0.3];
		private double[] _ambulancePriorityProbability = [0.3, 0.25, 0.2, 0.15, 0.1];
		public AgentEntryExam(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
			
			var specsWalkIn = new List<GenSpec>()
			{
				new GenSpec(0.6, 3 * 60, 5 * 60),
				new GenSpec(0.4, 5 * 60, 9 * 60)
			};
			_entryExamWalkInDuration = new ContinuousGenerator(new Random(MyCastSim().NextSeed()), specsWalkIn);
			_entryAmbulanceInDuration = new DiscreteGenerator(new  Random(MyCastSim().NextSeed()), new (){new (1,4 * 60, 481)});//9 lebo <4, 8>
			_walkInPriority = new Random(MyCastSim().NextSeed());
			_ambulancePriority = new Random(MyCastSim().NextSeed());
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		public double GetWalkInExamDuration() => _entryExamWalkInDuration.Sample();
		
		public double GetAmbulanceExamDuration() => _entryAmbulanceInDuration.Sample();
		
		public void AssignPriority(Patient patient)
		{
			patient.Priority = patient.ArrivedByAmbulance ? 
				GetPriority(_ambulancePriority.NextDouble(), _ambulancePriorityProbability) :
				GetPriority(_walkInPriority.NextDouble(), _walkInPriorityProbability);
		}

		private int GetPriority(double value, double[] probability)
		{
			var cumulate = 0.0;
			var priority = 1;
			for (int i = 0; i < probability.Length; i++)
			{
				cumulate += probability[i];
				if (value < cumulate)
				{
					return priority;
				}
				priority++;
			}
			return priority;
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerEntryExam(SimId.ManagerEntryExam, MySim, this);
			new ProcessEntryExam(SimId.ProcessEntryExam, MySim, this);
			AddOwnMessage(Mc.EntryExamPatient);
		}
		//meta! tag="end"
		
		private MySimulation MyCastSim() => (MySimulation)MySim;

		public void SetPositionAfterExam(MyMessage myMsg)
		{
			myMsg.Patient.AnimObject.SetPosition(MySim.CurrentTime, Config.ROOMS_B_AFTER_EXAM_POSITION[myMsg.Room.Id]);
		}
	}
}