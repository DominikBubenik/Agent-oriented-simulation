using Agents.AgentResources;
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