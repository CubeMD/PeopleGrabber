namespace Characters
{
    public interface ITeamConvertable
    {
        public bool CanBeConverted(Team team);
        public void Convert(Team team);
    }
}