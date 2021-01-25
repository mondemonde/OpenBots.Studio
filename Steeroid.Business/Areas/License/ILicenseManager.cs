using System.Threading.Tasks;

namespace Steeroid.Business.License
{
    public interface ILicenseManager
    {
      
        Task<LicensedMachine> GetMachine();
        Task<LicensedMachine> Register(string enteredToken);
        void Reset();

    }
}