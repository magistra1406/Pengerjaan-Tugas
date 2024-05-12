

using PercobaanAPI_2048.Entities;
using PercobaanAPI_2048.Repositories;
using PercobaanApi1.Repositories;

namespace PercobaanAPI_2048.Service
{
    public class RegionService
    {
        private RegionRepository regionRepository;
        public RegionService(RegionRepository regionRepository)
        {
            this.regionRepository = regionRepository;
        }
        public List<Region> findAll()
        {
            return regionRepository.findAll();
        }
        public Region findById(int id)
        {
            Region region = regionRepository.findById(id);
            return region != null ? region : null;
        }
        public Region create(Region entity)
        {
            Region region = new Region();
            region.name = entity.name;
            return regionRepository.create(region);
        }
        public Region update(int id, Region entity)
        {
            Region region = new Region();
            region.region_id = id;
            region.name = entity.name;
            return regionRepository.update(region);
        }
        public Region delete(int id)
        {
            Region region = new Region();
            region.region_id = id;
            return regionRepository.delete(region);
        }
    }
}