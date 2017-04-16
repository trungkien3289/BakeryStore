var OrderManagement = {
    deleteItem: function (id) {
        var title = "Xóa đơn hàng";
        var message = "Bạn có chắc chắn muốn xóa đơn hàng " + id + " này không ?";
        MessageBox.showMessageBox(title, message, function () {
            $.ajax({
                url: '/Admin/Order/Delete',
                data: { id: id },
                type: 'POST',
                success: function () {
                    window.location.replace("/Admin/Order/Index");
                },
                error: function () {
                    alert("Xóa thất bại!");
                }
            });
        });
    }
}