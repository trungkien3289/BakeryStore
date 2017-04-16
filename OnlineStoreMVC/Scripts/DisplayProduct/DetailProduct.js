var DetailProductManagement = {
    init: function () {
        this.bindEvents();
        this.initZoomImageControl();
        // Init popover
        $('[data-toggle="popover"]').popover({
            trigger: "hover",
            container: "body",
            placement: "auto right",
            template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
        });
    },
    bindEvents: function () {
        $("#thumbs_list_frame li").unbind("click").bind("click", function () {
            var imagePath = $(this).data("imagepath") || "/Content/Images/no-image.png";
            $("#picture-frame>img").attr("src", imagePath);
            $("#picture-frame>img").data("zoom-image", imagePath);
            DetailProductManagement.initZoomImageControl();
        });

        // search control
        $("#SearchProduct_Btn").unbind("click").bind("click", function (e) {
            searchProduct();
        });
        $("#tmsearch #tm_search_query").unbind("keypress").bind("keypress", function (e) {
            if (e.keyCode == 13) {
                searchProduct();
            }
        });

        function searchProduct() {
            var searchString = $("#tmsearch #tm_search_query").val();
            window.location.replace("/Product/SearchProduct?searchString=" + searchString);
        }
    },
    initZoomImageControl: function () {
        //$('#CoverImage').elevateZoom({
        //    zoomType: "inner",
        //    cursor: "crosshair",
        //    zoomWindowFadeIn: 500,
        //    zoomWindowFadeOut: 750
        //});

        $("#picture-frame").zoomToo({
            magnify: 1
        });
    }
}
