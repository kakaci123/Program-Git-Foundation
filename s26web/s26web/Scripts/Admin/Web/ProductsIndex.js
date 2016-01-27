$(function () {
    $(".save_btn[data-id=2]").click(function () {
        $(this).attr("disabled", "disabled");
        $('.loadingIMG[data-id=2]').show();
        var id = 2;
        var name = $(".item_Name[data-id=2]").val();
        var price = $(".item_price[data-id=2]").val();
        if (name == "")
            alert("名稱不得為空");
        if (price == "")
            alert("價格不得為空");
        $.ajax({
            url: "/shb/Products/Ajax_change",
            data: { id: id , name: name , price: price},
            type: "POST",
            success: function (msg) {
                alert("儲存成功");
                location.reload();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
    $(".save_btn[data-id=4]").click(function () {
        $(this).attr("disabled", "disabled");
        $('.loadingIMG[data-id=4]').show();
        var id = 4;
        var name = $(".item_Name[data-id = 4]").val();
        var price = $(".item_price[data-id = 4]").val();
        if (name == "")
            alert("名稱不得為空");
        if (price == "")
            alert("價格不得為空");
        $.ajax({
            url: "/shb/Products/Ajax_change",
            data: { id: id, name: name, price: price },
            type: "POST",
            success: function (msg) {
                alert("儲存成功");
                location.reload();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
});