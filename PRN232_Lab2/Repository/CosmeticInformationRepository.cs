using BusinessObject;
using DataAccessObject;

namespace Repository
{
    public class CosmeticInformationRepository : ICosmeticInformationRepository
    {
        public async Task<CosmeticInformation> Add(CosmeticInformation cosmeticInformation)
        {
            return await CosmeticInformationDAO.Instance.AddCosmeticInformation(cosmeticInformation);
        }

        public async Task<CosmeticInformation> Delete(string id)
        {
            return await CosmeticInformationDAO.Instance.Delete(id);
        }

        public Task<List<CosmeticCategory>> GetAllCategories()
        {
            return CosmeticInformationDAO.Instance.GetAllCategories();
        }

        public Task<List<CosmeticInformation>> GetAllCosmetics()
        {
            return CosmeticInformationDAO.Instance.GetAllCosmetics();
        }

        public Task<CosmeticInformation> GetOne(string id)
        {
            return CosmeticInformationDAO.Instance.GetById(id);
        }

        public Task<CosmeticInformation> Update(CosmeticInformation cosmeticInformation)
        {
            return CosmeticInformationDAO.Instance.Update(cosmeticInformation);
        }
    }
}
