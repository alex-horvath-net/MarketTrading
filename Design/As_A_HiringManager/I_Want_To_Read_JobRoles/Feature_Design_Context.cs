using HiringManager.ReadJobRoles.Feature;

namespace Design.As_A_HiringManager.I_Want_To_Read_JobRoles
{
    public class Feature_Design_Context
    {
        public Request GetRequest()
        {
            var request = new Request("Aladar");
            return request;
        }

        public CancellationToken GetToken()
        {
            var token = CancellationToken.None;
            return token;
        }

        public Feature GetUnit()
        {
            var unit = new Feature();
            return unit;
        }
    }
}