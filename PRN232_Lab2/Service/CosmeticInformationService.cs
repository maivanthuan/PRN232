using BusinessObject;
using Repository;

namespace Service
{
    public class CosmeticInformationService : ICosmeticInformationService
    {
        private readonly ICosmeticInformationRepository _cosmeticInformationRepository;
        public CosmeticInformationService(ICosmeticInformationRepository cosmeticInformationRepository)
        {
            _cosmeticInformationRepository = cosmeticInformationRepository;
        }
        public Task<CosmeticInformation> Add(CosmeticInformation cosmeticInformation)
        {
            return _cosmeticInformationRepository.Add(cosmeticInformation);
        }

        public Task<CosmeticInformation> Delete(string id)
        {
            return _cosmeticInformationRepository.Delete(id);
        }

        public Task<List<CosmeticCategory>> GetAllCategories()
        {
            return _cosmeticInformationRepository.GetAllCategories();
        }

        public Task<List<CosmeticInformation>> GetAllCosmetics()
        {
           return _cosmeticInformationRepository.GetAllCosmetics();
        }

        public Task<CosmeticInformation> GetOne(string id)
        {
            return _cosmeticInformationRepository.GetOne(id);
        }

        public Task<CosmeticInformation> Update(CosmeticInformation cosmeticInformation)
        {
            return _cosmeticInformationRepository.Update(cosmeticInformation);
        }
    }
}
