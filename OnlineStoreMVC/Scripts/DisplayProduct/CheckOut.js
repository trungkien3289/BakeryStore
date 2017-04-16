angular.module("ecom_Checkout", [])
    .controller("ecom_CheckOut_Controller", ["$http", "$window", function ($http, $window) {
        var self = this;
        self.checkOutModel = {};
        self.products = [];

        // Start private functions
        var init = function () {
            initCart();

            // Init list products in cart
            self.products = JSON.parse(localStorage.products);
        }
        var initCart = function () {
            // Init cart
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
                idCartModal: 'cartWindow',
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
        }
        var getListProductInCart = function () {
            // Get list products in cart

            var products = JSON.parse(localStorage.products);
            var requestProducts = [];
            if (products) {
                $.each(products, function (i, product) {
                    requestProducts.push({ Id: product.id, Quantity: product.quantity });
                });
            }
            return requestProducts;
        };
        var updateCart = function () {
            localStorage.products = JSON.stringify(self.products);
            var totalProductsQuantity = 0;
            $.each(self.products, function (i, product) {
                totalProductsQuantity += parseInt(product.quantity);
            })

            $(".my-cart-badge").text(totalProductsQuantity);
        }
        // End private functions

        // Start public functions
        self.checkOut = function () {
            // get list products in cart
            var products = getListProductInCart();
            debugger
            console.log(self.checkOutModel);
            self.checkOutModel.products = getListProductInCart();

            $http({
                method: "POST",
                url: "/Order/CheckOut",
                data: self.checkOutModel
            }).then(function (response) {
                if (response.data.success) {
                    localStorage.products = JSON.stringify([]);
                    $(".my-cart-badge").text(0);
                    $window.location.href = '/Order/OrderSuccessful';
                    console.log("CheckOut successfully!");
                } else {
                    console.log("Error on server!!!");
                }
            }, function () {
                console.log("Error on server!!!");
            });
        }
        self.removeProductFromCart = function (element, id) {
            //$(element).closest('.cross').fadeOut('slow', function (c) {
            var deletedItemIndex = 0;
                for (var i = 0; i < self.products.length; i++) {
                    if (self.products[i].id == id) {
                        deletedItemIndex = i;
                        break;
                    }
                }

                self.products = self.products.splice(deletedItemIndex, 1);

                updateCart();
            //});
        }
        self.plusQuantityProduct = function (id) {
            $.each(self.products, function (i, product) {
                if (product.id == id && product.quantity < 5) {
                    product.quantity++;
                }
            });

            updateCart();
        }
        self.minusQuantityProduct = function (id, quantity) {
            $.each(self.products, function (i, product) {
                if (product.id == id && product.quantity > 1) {
                    product.quantity--;
                }
            });

            updateCart();
        }
        // End public functions

        init();
    }]);