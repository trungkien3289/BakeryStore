using OnlineStore.Infractructure.Utility;
using OnlineStore.Model.ViewModel;
using OnlineStore.Service.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Service.Interfaces
{
    public interface IDisplayProductService:IDisposable
    {
        void RefreshAll();
        IEnumerable<ProductSummaryView> GetTopProductsByCategoryId(int categoryId, int top);
        GetProductsByCategoryResponse GetProductsByCategory(GetProductsByCategoryRequest request);
        ProductDetailsView GetProductDetails(int id);
        IEnumerable<ProductSummaryView> GetAllNewProduct();
        IEnumerable<ProductSummaryView> GetAllBestSellProduct();
        IEnumerable<ProductSummaryView> GetListHighPriorityOrderProduct();
        IList<SummaryCategoryViewModel> GetCategoryChildrenById(int parentId);
        IEnumerable<SummaryCategoryViewModel> GetTopCategories();
        SearchProductResponse SearchByProductName(SearchProductRequest request, SearchType searchType);
        IEnumerable<SummaryCategoryTreeViewItemModel> GetCategoryTreeViewData();
    }
}
