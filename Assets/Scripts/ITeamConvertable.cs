public interface ITeamConvertable
{
    public bool CanBeConverted(int walkersAmount);
    public void Convert(AgentTeam team);
}