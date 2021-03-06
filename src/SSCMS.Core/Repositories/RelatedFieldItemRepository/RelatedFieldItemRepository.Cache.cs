using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSCMS.Abstractions;
using SSCMS.Abstractions.Dto;

namespace SSCMS.Core.Repositories.RelatedFieldItemRepository
{
    public partial class RelatedFieldItemRepository
    {
        private async Task<int> GetMaxTaxisAsync(int siteId, int parentId)
        {
            var list = await GetAllAsync(siteId);
            return list.Where(x => x.ParentId == parentId).Select(o => o.Taxis)
                .DefaultIfEmpty().Max();
        }

        public async Task<List<RelatedFieldItem>> GetListAsync(int siteId, int relatedFieldId, int parentId)
        {
            var list = await GetAllAsync(siteId);
            return list.Where(x => x.RelatedFieldId == relatedFieldId && x.ParentId == parentId).OrderBy(x => x.Taxis).ToList();
        }

        public async Task<RelatedFieldItem> GetAsync(int siteId, int id)
        {
            var list = await GetAllAsync(siteId);
            return list.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<Cascade<int>>> GetCascadesAsync(int siteId, int relatedFieldId, int parentId)
        {
            var list = new List<Cascade<int>>();

            var items = await GetListAsync(siteId, relatedFieldId, parentId);

            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    var cascade = new Cascade<int>
                    {
                        Label = item.Label,
                        Value = item.Id,
                        Children = await GetCascadesAsync(siteId, relatedFieldId, item.Id)
                    };
                    cascade["ItemValue"] = item.Value;
                    list.Add(cascade);
                }
            }

            return list;
        }
    }
}