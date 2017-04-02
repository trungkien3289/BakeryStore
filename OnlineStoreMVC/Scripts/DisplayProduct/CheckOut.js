var CheckOutManagement = {
    init: function () {
        this.initCart();
        // bind events
        $("#Btn_CheckOut").unbind("click").bind("click", function () {
            CheckOutManagement.checkOut();
        });
    },
    model:{
        Products: [],
        FullName: "",
        Address: "",
        PhoneNumber: "",
        EmailAddress: "",
        Note:""
    },
    initCart: function () {

        var goToCartIcon = function ($addTocartBtn) {
            var $cartIcon = $(".my-cart-icon");
            var $image = $('<img width="30px" height="30px" src="' + $addTocartBtn.data("image") + '"/>').css({ "position": "fixed", "z-index": "999" });
            $addTocartBtn.prepend($image);
            var position = $cartIcon.position();
            $image.animate({
                top: position.top,
                left: position.left
            }, 500, "linear", function () {
                $image.remove();
            });
        }

        $('.my-cart-btn').myCart({
            classCartIcon: 'my-cart-icon',
            classCartBadge: 'my-cart-badge',
            affixCartIcon: true,
            checkoutCart: function (products) {
                $.each(products, function () {
                    console.log(this);
                });
            },
            clickOnAddToCart: function ($addTocart) {
                goToCartIcon($addTocart);
            },
            getDiscountPrice: function (products) {
                var total = 0;
                $.each(products, function () {
                    total += this.quantity * this.price;
                });
                return total * 1;
            }
        });
    },
    getListProductInCart: function () {

        var products = JSON.parse(localStorage.products);
        var requestProducts = [];
        if (products) {
            $.each(products, function (i, product) {
                requestProducts.push({ Id: product.id, Quantity: product.quantity });
            });
        }
        return requestProducts;
    },
    checkOut: function () {
        // get list products in cart
        var products = this.getListProductInCart();

        // get information of user
        this.model.FullName = $("#Txt_FullName").val();
        this.model.Address = $("#Txt_Address").val();
        this.model.EmailAddress = $("#Txt_EmailAddress").val();
        this.model.PhoneNumber = $("#Txt_PhoneNumber").val();
        this.model.Note = $("#Txa_Note").val();
        this.model.Products = products;

        $.ajax({
            type: "POST",
            url: "/Order/CheckOut",
            data: JSON.stringify(this.model),
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                if (data.success) {
                    localStorage.products = JSON.stringify([]);
                    $(".my-cart-badge").text(0);
                    console.log("CheckOut successfully!");
                } else {
                    console.log("Error on server!!!");
                }
            },
            error: function () {
                console.log("Error on server!!!");
            }
        });
    }
}