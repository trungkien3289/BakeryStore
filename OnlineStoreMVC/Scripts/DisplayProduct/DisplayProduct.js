var ProductSortEnum = {
        PriceHighToLow : 1,
        PriceLowToHigh : 2,
        ProductNameAToZ : 3,
        ProductNameZToA : 4 
}
var GetProductsByCategoryRequest = function(categoryId,index){
    this.CategoryId = categoryId || null;
    this.BrandIds = [];
    this.SortBy = ProductSortEnum.ProductNameAToZ;
    this.BeginPrice = null; // decimal?
    this.EndPrice = null; // decimal?
    this.Index = index || 0; // int
    this.SearchString  = ""; // string
    this.NumberOfResultsPerPage = 10; // int
}
var DisplayProductManagement = {
    model:{
        CategoryId: null, // int
        BrandIds:[], // List<int>
        SortBy: ProductSortEnum.ProductNameAToZ,// int 
        BeginPrice: null, // decimal?
        EndPrice: null, // decimal?
        Index: 0, // int
        SearchString : "", // string
        NumberOfResultsPerPage: 10 // int
    },
    controls:{
        priceRange: null,
        spin:null
    },
    init: function (categoryId, numberItems, index) {
        // Init spin
        this.controls.spin = new Spinner({
            lines: 15 // The number of lines to draw
            , length: 0 // The length of each line
            , width: 14 // The line thickness
            , radius: 28 // The radius of the inner circle
            , scale: 1 // Scales overall size of the spinner
            , corners: 0 // Corner roundness (0..1)
            , color: '#630a34' // #rgb or #rrggbb or array of colors
            , opacity: 0.25 // Opacity of the lines
            , rotate: 32 // The rotation offset
            , direction: 1 // 1: clockwise, -1: counterclockwise
            , speed: 1.7 // Rounds per second
            , trail: 64 // Afterglow percentage
            , fps: 20 // Frames per second when using setTimeout() as a fallback for CSS
            , zIndex: 2e9 // The z-index (defaults to 2000000000)
            , className: 'spinner' // The CSS class to assign to the spinner
            , top: '50%' // Top position relative to parent
            , left: '50%' // Left position relative to parent
            , shadow: true // Whether to render a shadow
            , hwaccel: false // Whether to use hardware acceleration
            , position: 'fixed' // Element positioning
        }).spin();
        // assign model
        this.model = new GetProductsByCategoryRequest(categoryId, index);
        // init paging control
        this.initPagingControl(numberItems, this.model.NumberOfResultsPerPage);
        // Init price range control
        var priceRangeElement = $('#Sl_PriceRange').slider({
            from: 5000,
            to: 150000,
            heterogeneity: ['50/50000'],
            step: 1000,
            dimension: '&nbsp;$',
            onstatechange: function (value) { console.log(value) }
        });
        this.controls.priceRange = priceRangeElement.data("slider");

        //Init layout
        if (numberItems != null && numberItems == 0) {
            $("ul.product_list.grid.row").append(this.getNoResultMessage());
        }

        this.bindEvents();
    },
    initPagingControl: function (numberItems, itemsOnPage) {
        $(".paging-control-filtercontent").empty();
        $(".paging-control-filtercontent").pagination({
            items: numberItems || 0,
            itemsOnPage: itemsOnPage,
            cssStyle: 'light-theme',
            prevText: '<',
            nextText:'>',
            onPageClick: onPaging,
            currentPage: DisplayProductManagement.model.Index
        });

        function onPaging(pageNumber, event) {
            DisplayProductManagement.updatePaging(pageNumber);
        }
    },
    bindEvents: function () {
        // brand checkboxs
        $("#layered_block_left input:checkbox.ckb-brand-filtercontent").unbind("change").bind("change", function (sender) {
            var isCheck = $(this).is(":checked");
            var brandId = $(this).data("id");
            DisplayProductManagement.updateSelectedBrandList(brandId, isCheck);
        });
        // filter by price
        $("#Btn_FilterAfterPrice").unbind("click").bind("click", function () {
            var priceRange = DisplayProductManagement.controls.priceRange.value;
            var minPrice = priceRange[0];
            var maxPrice = priceRange[1];
            DisplayProductManagement.updatePriceRangeFilter(minPrice, maxPrice);
        });

        // sort
        $("#SortProductOptionsSelectListItems").unbind("change").bind("change", function () {
            var sortType = $("#SortProductOptionsSelectListItems").val();
            DisplayProductManagement.updateSortBy(sortType);
        });

        // search control
        $("#SearchProduct_Btn").unbind("click").bind("click", function () {
            var searchString = $("#tm_search_query").val().trim();
            DisplayProductManagement.updateSearch(searchString);
        });
    },
    updateSelectedBrandList: function (brandId, isAdd) {
        /// <summary>
        /// Update selected brands list when user add or remove a brand in filter
        /// </summary>
        /// <param>N/A</param>
        /// <returns>N/A</returns>

        if (isAdd) {
            var isExist = false;
            for (var i = 0; i < DisplayProductManagement.model.BrandIds.length; i++) {
                if (DisplayProductManagement.model.BrandIds[i] == brandId) {
                    isExist = true;
                }
            }
            if (!isExist) {
                DisplayProductManagement.model.BrandIds.push(brandId);
            }
        } else {
            for (var i = 0; i < DisplayProductManagement.model.BrandIds.length; i++) {
                if (DisplayProductManagement.model.BrandIds[i] == brandId) {
                    DisplayProductManagement.model.BrandIds.splice(i, 1);
                }
            }
        }

        DisplayProductManagement.updateListProducts();
    },
    updatePriceRangeFilter: function (minPrice, maxPrice) {
        /// <summary>
        /// Update price range in filter model 
        /// </summary>
        /// <param>N/A</param>
        /// <returns>N/A</returns>

        DisplayProductManagement.model.BeginPrice = minPrice;
        DisplayProductManagement.model.EndPrice = maxPrice;
        DisplayProductManagement.updateListProducts();
    },
    updateSortBy: function (sortType) {
        /// <summary>
        /// Update sort type in filter model 
        /// </summary>
        /// <param>N/A</param>
        /// <returns>N/A</returns>

        DisplayProductManagement.model.SortBy = sortType ? sortType : ProductSortEnum.ProductNameAToZ;
        DisplayProductManagement.updateListProducts();
    },
    updatePaging: function (pageNumber) {
        /// <summary>
        /// Update layout when paging control changes 
        /// </summary>
        /// <param>N/A</param>
        /// <returns>N/A</returns>

        DisplayProductManagement.model.Index = pageNumber;
        DisplayProductManagement.updateListProducts();
    },
    updateSearch: function (searchString) {
        /// <summary>
        /// Update layout when search control changes 
        /// </summary>
        /// <param>N/A</param>
        /// <returns>N/A</returns>

        DisplayProductManagement.model.SearchString = searchString;
        DisplayProductManagement.updateListProducts();
    },
    updateListProducts: function () {
        /// <summary>
        /// Update list products by filters
        /// </summary>
        /// <param>N/A</param>
        /// <returns>N/A</returns>

        DisplayProductManagement.showSpin();
        $.ajax({
            type: "POST",
            url: "/Product/GetProductsByAjax",
            data: JSON.stringify(DisplayProductManagement.model),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                DisplayProductManagement.updateGuiForListProducts(result.Products);
                DisplayProductManagement.updateModelAndLayout(result);
                DisplayProductManagement.hideSpin();
            },
            error: function (ex) {
                console.log(ex.message);
                DisplayProductManagement.hideSpin();
            }
        });
    },
    updateGuiForListProducts: function (products) {
        /// <summary>
        /// Update Gui of list product panel after get data from server
        /// </summary>
        /// <param>N/A</param>
        /// <returns>N/A</returns>

        debugger
        $(".list-product-container").empty();
        if (products && products.length == 0) {
            $(".list-product-container").append(DisplayProductManagement.getNoResultMessage());
        } else {
            for (var i = 0; i < products.length; i++) {
                var itemHtml = DisplayProductManagement.genarateHtmlProductItem(products[i]);
                $(".list-product-container").append(itemHtml);
            }
        }
    },
    genarateHtmlProductItem: function (product) {
        /// <summary>
        ///Genarate html element for each item of list products panel
        /// </summary>
        /// <param>N/A</param>
        /// <returns>N/A</returns>

        var template = "";
        template += "    <div class=\"col-md-4 product-item-wrapper\">";
        template += "        <div class=\"hover14 column\">";
        template += "            <div class=\"agile_top_brand_left_grid\">";
        template += "                <div class=\"agile_top_brand_left_grid_pos\">";
        template += "                    <img src=\"~\/Content\/Images\/TestImages\/offer.png\" alt=\" \" class=\"img-responsive\" \/>";
        template += "                <\/div>";
        template += "                <div class=\"agile_top_brand_left_grid1\">";
        template += "                    <figure>";
        template += "                        <div class=\"snipcart-item block\">";
        template += "                            <div class=\"snipcart-thumb\">";
        template += "                                <a href=\"products.html\"><img title=\"" + product.Name + " \" alt=\"" + product.Name + "\" src=\"" + product.CoverImageUrl + "\" \/><\/a>";
        template += "                                <p>" + product.Name + "<\/p>";
        template += "                                <h4>" + product.Price + "<\/h4>";
        template += "                            <\/div>";
        template += "                            <div class=\"snipcart-details top_brand_home_details\">";
        template += "                                <form action=\"#\" method=\"post\">";
        template += "                                    <fieldset>";
        template += "                                        <input type=\"hidden\" name=\"cmd\" value=\"_cart\" \/>";
        template += "                                        <input type=\"hidden\" name=\"add\" value=\"1\" \/>";
        template += "                                        <input type=\"hidden\" name=\"business\" value=\" \" \/>";
        template += "                                        <input type=\"hidden\" name=\"item_name\" value=\"Fortune Sunflower Oil\" \/>";
        template += "                                        <input type=\"hidden\" name=\"amount\" value=\"20.99\" \/>";
        template += "                                        <input type=\"hidden\" name=\"discount_amount\" value=\"1.00\" \/>";
        template += "                                        <input type=\"hidden\" name=\"currency_code\" value=\"USD\" \/>";
        template += "                                        <input type=\"hidden\" name=\"return\" value=\" \" \/>";
        template += "                                        <input type=\"hidden\" name=\"cancel_return\" value=\" \" \/>";
        template += "                                        <input type=\"submit\" name=\"submit\" value=\"Add to cart\" class=\"button\" \/>";
        template += "                                    <\/fieldset>";
        template += "                                <\/form>";
        template += "                            <\/div>";
        template += "                        <\/div>";
        template += "                    <\/figure>";
        template += "                <\/div>";
        template += "            <\/div>";
        template += "        <\/div>";
        template += "    <\/div>";


        return template;

    },
    getNoResultMessage:function(){
        return "<li class='noresult-panel'>Không có sản phẩm nào được tìm thấy</li>";
    },
    updateModelAndLayout: function (model) {
        // update title bar
        $("#center_column .heading-counter").text("Có " + model.NumberOfTitlesFound + " sản phẩm");
        // update paging control
        DisplayProductManagement.initPagingControl(model.NumberOfTitlesFound, DisplayProductManagement.model.NumberOfResultsPerPage);
    },
    showSpin: function (target) {
        /// <summary>
        /// Create spin control
        /// </summary>
        /// <param>N/A</param>
        /// <returns>N/A</returns>s

        $(".list-product-container").append(DisplayProductManagement.controls.spin.spin().el);
    },
    hideSpin: function () {
        /// <summary>
        /// Hide spin control
        /// </summary>
        /// <param>N/A</param>
        /// <returns>N/A</returns>

        DisplayProductManagement.controls.spin.stop();
    }
}